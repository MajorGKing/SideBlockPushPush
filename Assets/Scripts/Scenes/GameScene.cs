using System.Collections;
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
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        //Managers.UI.ShowSceneUI<UI_Inven>();

        //Dictionary<int, Stat> dict = Managers.Data.StatDict;
        for (int i = 0; i < 4; i++)
        {
            m_blockNumbers.Add(new List<int>());
        }
        // Add the count of blocks to m_blockNumbers
        AddBlockNumbers(_blocks0, m_blockNumbers[0]);
        AddBlockNumbers(_blocks1, m_blockNumbers[1]);
        AddBlockNumbers(_blocks2, m_blockNumbers[2]);
        AddBlockNumbers(_blocks3, m_blockNumbers[3]);

        InitRandomBlocks();
        UpdateBlockColor();

    }

    private void AddBlockNumbers(SpriteRenderer[] blocks, List<int> blockNumbers)
    {
        int count = blocks.Length;
        blockNumbers.Add(count);
    }

    private void InitRandomBlocks()
    {
        System.Random rand = new System.Random();

        for (int i = 0; i < m_blockNumbers.Count; i++)
        {
            m_blockNumbers[i].Clear(); // Clear any existing data

            foreach (SpriteRenderer block in GetBlocksArray(i))
            {
                int randomValue = rand.Next(2, 9); // Random value between 2 and 8 (inclusive)
                m_blockNumbers[i].Add(randomValue);
            }
        }
    }

    // Second method: UpdateBlockColor
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

    }
}
