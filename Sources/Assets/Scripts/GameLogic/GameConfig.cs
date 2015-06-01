using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConfig {

    static int[] m_SingleReward = {10, 20, 40, 80, 160, 320, 640, 1250, 2500, 5000, 10000};
    static List<long> m_EXPToNextLevel = new List<long>();

    private static GameConfig m_sInstance = null;

    private GameConfig()
    {
        m_sInstance = this;
        LoadExpTable();
    }

    public static GameConfig Instance
    {
        get
        {
            if (m_sInstance == null)
            {
                m_sInstance = new GameConfig();
            }
            return m_sInstance;
        }
    }

    public int GetSingleReward(int level)
    {
        return m_SingleReward[level];
    }

    public int GetNumberOfPvEStage()
    {
        return m_SingleReward.GetLength(0);
    }

    public void LoadExpTable()
    {
        TextAsset txt = (TextAsset)Resources.Load("ExpTable", typeof(TextAsset));

        string[] linesInFile = txt.text.Split('\n');

        for (int i = 0; i < linesInFile.GetLength(0); i++)
        {
            
            string line = linesInFile[i];
            //Debug.Log(line);
            string[] numbers = line.Split(' ');
            m_EXPToNextLevel.Add(int.Parse(numbers[1]));
            //Debug.Log(long.Parse(numbers[1]));
        }
    }

    public int GetNumberOfLevel()
    {
        return m_EXPToNextLevel.Count;
    }

    public long GetLevelEXP(int level)
    {
        return m_EXPToNextLevel[level];
    }
}
