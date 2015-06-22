using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class GameInfo
{
    public int m_GameID = 0;
    public string m_PlayerA = "";
    public string m_PlayerB = "";    
    public int m_Round = 1;

    public List<int> m_PieceA = new List<int>();// = {0, 0, 0, 0, 0, 0};

    public List<int> m_PieceB = new List<int>();// = { 0, 0, 0, 0, 0, 0 };    
    public int m_SpinProgressA;// = 0;
    public int m_SpinProgressB;// = 0;
    public int m_CurrentTurn = 1;
    public int m_TurnType = 1;

    public string m_Challenger; //1 - playerA is the challenger; 2 - playerB is the challenger
    public int m_ChallengeScoreA = 0;
    public int m_ChallengeScoreB = 0;
    public int m_BetTrophyA = 0;
    public int m_BetTrophyB = 0;
    public List<int> m_ChallengeQuestion = new List<int>();// = { 0, 0, 0, 0, 0, 0 };
    public List<int> m_ChallengeAnswer = new List<int>();// = { 0, 0, 0, 0, 0, 0 };
    public int m_ChallengeState = 0;

    public int m_CurrentRound = 0;
    //public DateTime m_LastPlayedTime = DateTime.Now;
    public bool m_IsCompleted = false;

    public int m_PlayerAAbilityUsed;
    public int m_PlayerBAbilityUsed;
    public string m_PlayerUseAbility;
    public int m_LastAbility;
    public int m_TrophyAcquired;
    public int m_TrophyRemoved;

    public int GetScoreA()
    {
        int count = 0;
        for (int i = 0; i < m_PieceA.Count; i++)
        {
            if (m_PieceA[i] == 1)
            {
                count++;
            }
        }
        return count;
    }

    public int GetScoreB()
    {
        int count = 0;
        for (int i = 0; i < m_PieceB.Count; i++)
        {
            if (m_PieceB[i] == 1)
            {
                count++;
            }
        }
        return count;
    }
}

[XmlRoot("GameList")]
public class GameList
{

    [XmlArray("GameList")]
    [XmlArrayItem("GameInfo")]
    public List<GameInfo> m_GameList = new List<GameInfo>();

    public static GameList CreateEmptyGameList()
    {
        GameList m_GameList = new GameList();
        return m_GameList;
    }

    public GameInfo AddNewGame(string opponent)
    {
        int max = 0;
        for (int i = 0; i < m_GameList.Count; i++)
        {
            if (max < m_GameList[i].m_GameID)
            {
                max = m_GameList[i].m_GameID;
            }
        }
        GameInfo gi = new GameInfo();
        gi.m_PlayerA = GameManager.Instance.GetPlayerProfile().m_PlayerName;
        gi.m_PlayerB = opponent;
        for (int i = 0; i < 6; i++)
        {
            gi.m_PieceA.Add(0);
            gi.m_PieceB.Add(0);
            gi.m_ChallengeQuestion.Add(0);
            gi.m_ChallengeAnswer.Add(0);
            gi.m_GameID = max + 1;
        }        
        m_GameList.Add(gi);
        Save();
        return gi;
    }

    public void Save(string path = "TriviaSessionList.xml")
    {
        path = Utils.pathForDocumentsFile(path);
        var serializer = new XmlSerializer(typeof(GameList));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static GameList Load(string path = "TriviaSessionList.xml")
    {
        path = Utils.pathForDocumentsFile(path);
        try
        {
            var serializer = new XmlSerializer(typeof(GameList));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as GameList;
            }
        }
        catch (Exception e)
        {
            return CreateEmptyGameList();
        }
    }
}


