using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameScene : BaseScene
{
    public Sprite[] _colors;
    public SpriteRenderer[] _blocks0;
    public SpriteRenderer[] _blocks1;
    public SpriteRenderer[] _blocks2;
    public SpriteRenderer[] _blocks3;
    private List<List<int>> m_blockNumbers = new List<List<int>>();
    private List<int> m_bottomStockNumber = new List<int>();
    public SpriteRenderer[] _bottomBlocks;
    private const int BLOCK_TYPES = 7; // m = 7 as mentioned before

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        
        for (int i = 0; i < 4; i++)
        {
            m_blockNumbers.Add(new List<int>());
        }

        for(int i = 0; i < _bottomBlocks.Length; i++)
        {
            m_bottomStockNumber.Add(1);
        }

        InitClearableBlocks();
        UpdateBlockColor();
        UpdateBottomColor();
    }

    private void InitClearableBlocks()
    {
        while (GetTotalBlockCount() > 0)
        {
            Debug.Log("GetTotalBlockCount() : " + GetTotalBlockCount());
            int number = Random.Range(2, BLOCK_TYPES + 2); // Random number between 2 and 8 (inclusive)
            for (int i = 0; i < 3; i++)
            {
                AddBlockToRandomList(number);

                
                
                if (GetTotalBlockCount() == 0) break;
            }
        }

        // Reverse each list in m_blockNumbers
        for (int i = 0; i < m_blockNumbers.Count; i++)
        {
            m_blockNumbers[i].Reverse();
        }
    }

    private void AddBlockToRandomList(int number)
    {
        List<int> availableLists = new List<int>();
        for (int i = 0; i < m_blockNumbers.Count; i++)
        {
            if (m_blockNumbers[i].Count < GetBlocksArray(i).Length)
            {
                availableLists.Add(i);
            }
        }

        if (availableLists.Count > 0)
        {
            int randomListIndex = availableLists[Random.Range(0, availableLists.Count)];
            m_blockNumbers[randomListIndex].Add(number);
        }
    }

    private int GetTotalBlockCount()
    {
        return (_blocks0.Length + _blocks1.Length + _blocks2.Length + _blocks3.Length) - 
               (m_blockNumbers[0].Count + m_blockNumbers[1].Count + m_blockNumbers[2].Count + m_blockNumbers[3].Count);
    }

    private void UpdateBlockColor()
    {
        for (int i = 0; i < m_blockNumbers.Count; i++)
        {
            List<int> blockNumbers = m_blockNumbers[i];
            SpriteRenderer[] blocks = GetBlocksArray(i);

            for (int j = 0; j < blockNumbers.Count; j++)
            {
                if (j < blocks.Length) // Ensure we don't go out of bounds
                {
                    int colorIndex = blockNumbers[j];
                    if (colorIndex >= 0 && colorIndex < _colors.Length)
                    {
                        blocks[j].sprite = _colors[colorIndex];
                    }
                }
            }
        }
    }

    private void UpdateBottomColor()
    {
        for(int i = 0; i < m_bottomStockNumber.Count; i++)
        {
            if(i < _bottomBlocks.Length)
            {
                int colorIndex = m_bottomStockNumber[i];
                if (colorIndex >= 0 && colorIndex < _colors.Length)
                {
                    _bottomBlocks[i].sprite = _colors[colorIndex];
                }
            }
        }
    }

    private SpriteRenderer[] GetBlocksArray(int index)
    {
        switch (index)
        {
            case 0: return _blocks0;
            case 1: return _blocks1;
            case 2: return _blocks2;
            case 3: return _blocks3;
            default: return null;
        }
    }

    public override void Clear()
    {
        // Implementation for Clear method if needed
    }

    public async void LineTouched(int lineIndex)
    {
        Debug.Log("Line Touched : " + lineIndex);

        var popNumber = m_blockNumbers[lineIndex][0];

        if (popNumber >= 2)
        {
            var bottomMax = true;

            for (int i = m_bottomStockNumber.Count - 1; i >= 0; i--)
            {
                if (m_bottomStockNumber[i] == 0 || m_bottomStockNumber[i] == 1)
                {
                    m_bottomStockNumber[i] = popNumber;
                    bottomMax = false;
                    break;
                }
            }

            if (bottomMax == false)
            {
                m_blockNumbers[lineIndex].RemoveAt(0);
                m_blockNumbers[lineIndex].Add(1);

                UpdateBottomColor();
                UpdateBlockColor();

                await CheckAndRemoveBottomNumbers();

                UpdateBottomColor();
                UpdateBlockColor();

                // Check bottom max
                if(true == CheckBottomMax())
                {
                    var emptyLine = CheckEmptyBlockLine();
                    if(emptyLine == -1)
                    {
                        // GameOver
                    }
                    else if(emptyLine != -1)
                    {
                        MoveBlocksDown(emptyLine);
                        UpdateBlockColor();
                        UpdateBottomColor();
                    }
                }
            }
        }
    }

    private void MoveBlocksDown(int emptyLine)
    {
        // Move blocks down from the line above the empty line
        for (int i = emptyLine; i < m_blockNumbers.Count - 1; i++)
        {
            m_blockNumbers[i] = new List<int>(m_blockNumbers[i + 1]); // Move the blocks from line above
        }

        // Now move the bottom blocks up to fill the last row of m_blockNumbers
        for (int i = 0; i < m_blockNumbers[m_blockNumbers.Count - 1].Count; i++)
        {
            if (i < _bottomBlocks.Length)
            {
                m_blockNumbers[m_blockNumbers.Count - 1][i] = m_bottomStockNumber[i];
            }
        }

        // Clear the bottom row after moving them
        for (int i = 0; i < m_bottomStockNumber.Count; i++)
        {
            m_bottomStockNumber[i] = 1; // Set to default value or whatever you want the empty state to be
        }
    }

    private bool CheckBottomMax()
    {
       if(m_bottomStockNumber[0] >= 2)
       {
            Debug.Log("Max Bottom!");
            return true;
       }

       return false;
    }

    private int CheckEmptyBlockLine()
    {
        for (int i = m_blockNumbers.Count - 1; i >= 0; i--)
        {
            if (m_blockNumbers[i][0] == 0 || m_blockNumbers[i][0] == 1)
            {
                return i;
            }
        }

        return -1;
    }

    private async UniTask CheckAndRemoveBottomNumbers()
    {
        bool removed;

        do
        {
            removed = false;
            for (int i = m_bottomStockNumber.Count - 1; i >= 2; i--)
            {
                if (m_bottomStockNumber[i] >= 2)
                {

                    if (m_bottomStockNumber[i] == m_bottomStockNumber[i - 1] &&
                        m_bottomStockNumber[i] == m_bottomStockNumber[i - 2])
                    {
                        await BlockRemoveAnimation(i);

                        // Set the numbers to zero to remove them
                        m_bottomStockNumber.RemoveAt(i);
                        m_bottomStockNumber.RemoveAt(i - 1);
                        m_bottomStockNumber.RemoveAt(i - 2);

                        m_bottomStockNumber.Insert(0, 1);
                        m_bottomStockNumber.Insert(0, 1);
                        m_bottomStockNumber.Insert(0, 1);

                        

                        removed = true;
                        break; // Restart checking from the beginning
                    }
                }
            }
        } while (removed);

        return ; 
    }

    private async UniTask BlockRemoveAnimation(int startIndex)
    {
        _bottomBlocks[startIndex].GetComponent<Animation>().Play("Alpha01");
        _bottomBlocks[startIndex - 1].GetComponent<Animation>().Play("Alpha01");
        _bottomBlocks[startIndex - 2].GetComponent<Animation>().Play("Alpha01");

        await UniTask.Delay(500);

        return ;
    }

    public void ResetButtonClicked()
    {
        Managers.Scene.LoadScene(Define.Scene.Game);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }
}