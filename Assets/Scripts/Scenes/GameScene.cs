using System.Collections.Generic;
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
        // Add the count of blocks to m_blockNumbers
        AddBlockNumbers(_blocks0, m_blockNumbers[0]);
        AddBlockNumbers(_blocks1, m_blockNumbers[1]);
        AddBlockNumbers(_blocks2, m_blockNumbers[2]);
        AddBlockNumbers(_blocks3, m_blockNumbers[3]);

        for(int i = 0; i < _bottomBlocks.Length; i++)
        {
            m_bottomStockNumber.Add(1);
        }

        InitClearableBlocks();
        UpdateBlockColor();
        UpdateBottomColor();
    }

    private void AddBlockNumbers(SpriteRenderer[] blocks, List<int> blockNumbers)
    {
        int count = blocks.Length;
        blockNumbers.Add(count);
    }

    private void InitClearableBlocks()
    {
        while (GetTotalBlockCount() > 0)
        {
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

    public void LineTouched(int lineIndex)
    {
        Debug.Log("Line Touched : " + lineIndex);
    }
}