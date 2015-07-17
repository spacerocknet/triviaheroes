﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConfig {

    static int[] m_SingleReward = {10, 20, 40, 80, 160, 320, 640, 1250, 2500, 5000, 10000};
    static string[] m_LoadingTips = {
                                        "You get to use each boost once for free during matches",
                                        "Trivia tournament prizes can only be claimed once at each tier",
                                        "Use the 50/50 boost to remove two incorrect option choices",
                                        "User the extra time boost to add an extra 15 seconds to the countdown timer",
                                        "Use the survey boost to show how other people have answered this question",
                                        "Use the skip boost to get a new question",
                                        "Get three questions right in a row for a chance to claim or challenge for a puzzle piece",
                                        "Collect all 6 puzzle pieces to win a multiplayer match",
                                        "Help me, help you. Don’t be shy about asking your friends for lives",
                                        "Upgrade your avatar to unlock new abilities"
                                    };
    static List<long> m_EXPToNextLevel = new List<long>();

    List<List<int>> m_ItemCost;
    List<List<int>> m_AvatarUpgradeCost;


    private static GameConfig m_sInstance = null;

    //private static int m_StartGem = 50;
    private static int m_StartGem = 999999;

    private GameConfig()
    {
        m_sInstance = this;
        LoadExpTable();
        LoadUpgradeCostTable();
        LoadItemCost();
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

    public void LoadUpgradeCostTable()
    {
        TextAsset txt = (TextAsset)Resources.Load("UpgradeCostTable", typeof(TextAsset));

        string[] linesInFile = txt.text.Split('\n');

        m_AvatarUpgradeCost = new List<List<int>>();
        for (int i = 0; i < linesInFile.GetLength(0); i++)
        {

            string line = linesInFile[i];
            //Debug.Log(line);
            string[] numbers = line.Split(' ');
            List<int> l = new List<int>();
            for (int j = 0; j < numbers.GetLength(0); j++)
            {                
                l.Add(int.Parse(numbers[j]));
            }
            m_AvatarUpgradeCost.Add(l);            
        }
    }

    public void LoadItemCost()
    {
        TextAsset txt = (TextAsset)Resources.Load("ItemCost", typeof(TextAsset));

        string[] linesInFile = txt.text.Split('\n');

        m_ItemCost = new List<List<int>>();
        for (int i = 0; i < linesInFile.GetLength(0); i++)
        {

            string line = linesInFile[i];
            //Debug.Log(line);
            string[] numbers = line.Split(' ');
            List<int> l = new List<int>();
            for (int j = 0; j < numbers.GetLength(0); j++)
            {
                l.Add(int.Parse(numbers[j]));
            }
            m_ItemCost.Add(l);
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

    public string GetRandomTips()
    {
        int idx = Random.RandomRange(0, m_LoadingTips.GetLength(0));
        return m_LoadingTips[idx];
    }

    public int GetUpgradeCost(int tier, int ava)
    {
        return m_AvatarUpgradeCost[tier][ava];
    }

    public int GetItemPrice(int type, int id)
    {
        return m_ItemCost[type][id - 1];
    }

    public float GetExchangeRate()
    {
        return 27.7114286f;
    }

    public int GetStartGem()
    {
        return m_StartGem;
    }
}
