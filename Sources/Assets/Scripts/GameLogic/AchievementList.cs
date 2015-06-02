using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;

public class AchievementList
{
    List<Achievement> m_AchievementList;

    private static AchievementList m_sInstance = null;

    private AchievementList()
    {
        m_sInstance = this;        
    }

    public static AchievementList Instance
    {
        get
        {
            if (m_sInstance == null)
            {
                m_sInstance = new AchievementList();
            }
            return m_sInstance;
        }
    }

    public void Init()
    {
        m_AchievementList = new List<Achievement>();

        TextAsset txt = (TextAsset)Resources.Load("AchivementTable", typeof(TextAsset));

        string[] linesInFile = txt.text.Split('\n');

        int idx = 0;
        int aid = 0;
        while (idx < linesInFile.GetLength(0)) {
            string title = linesInFile[idx++];
            string des = linesInFile[idx++];
            Debug.Log(linesInFile[idx]);
            string[] numbers = linesInFile[idx++].Split(' ');
            
            List<int> req = new List<int>();
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                req.Add(int.Parse(numbers[i]));
            }
            Achievement_Action action = (Achievement_Action)Enum.Parse(typeof(Achievement_Action), linesInFile[idx++]);
            Achievement a = new Achievement();
            a.SetInfo(title, des, req, action);
            a.SetCounter(GameManager.Instance.GetPlayerProfile().m_AchievementCounter[aid]);
            m_AchievementList.Add(a);                       
            aid++;
        }
        
    }

    public int GetAchievementCount()
    {
        return m_AchievementList.Count;
    }

    public Achievement GetAchievementBy(int id)
    {
        return m_AchievementList[id];
    }

    public void OnAction(Achievement_Action action)
    {
        PlayerProfile pl = GameManager.Instance.GetPlayerProfile();
        for (int i = 0; i < m_AchievementList.Count; i++)
        {
            m_AchievementList[i].OnAction(action);
            pl.m_AchievementCounter[i] = m_AchievementList[i].GetCounter();
        }
        pl.Save();
    }

    public void OnSingleWin()
    {
        OnAction(Achievement_Action.WIN_SINGLE);
    }

    public void OnMultiWin()
    {
        OnAction(Achievement_Action.WIN_MULTI);
    }
}
