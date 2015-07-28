using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;
using SimpleJSON;

public class GameInfo
{
    //To be removed
    //public int m_GameID = 0;
    public string m_SessionID = "-1";
    public string m_PlayerAID = "-1";
    public string m_PlayerAName = "-1";
    public string m_PlayerBName = "-1"; 
    //Avatar Info
    public int m_PlayerAIDTier;
    public int m_PlayerAIDJobs;
    public int m_PlayerAIDSex;
    public List<int> m_PlayerAIDItems = new List<int>();
    //Avatar Info
    public int m_PlayerBIDTier;
    public int m_PlayerBIDJobs;
    public int m_PlayerBIDSex;
    public List<int> m_PlayerBIDItems = new List<int>();

    public string m_PlayerBID = "-1";    
    public int m_Round = 1;

    public List<int> m_PieceA = new List<int>();// = {0, 0, 0, 0, 0, 0};

    public List<int> m_PieceB = new List<int>();// = { 0, 0, 0, 0, 0, 0 };    
    public int m_SpinProgressA;// = 0;
    public int m_SpinProgressB;// = 0;
    public int m_CurrentTurn = 1;
    public int m_TurnType = 1;

    public string m_Challenger = "-1"; //1 - playerA is the challenger; 2 - playerB is the challenger
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

    public int m_PlayerAIDAbilityUsed = 0;
    public int m_PlayerBIDAbilityUsed = 0;
    public string m_PlayerUseAbility = "-1";
    public int m_LastAbility = -1;
    public int m_TrophyAcquired = -1;
    public int m_TrophyRemoved = -1;
    public bool m_AbilityShowed = false;

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

    public string ToJsonString()
    {
        string s = JsonConvert.SerializeObject(this);
        s = s.Replace("\"\"", "\"");
        return s;
    }

    static public GameInfo FromJsonString(string s, bool join = false)
    {
        List<int> l;
        var ret = JSONNode.Parse(s);

        GameInfo gi = new GameInfo();
        gi.m_Round = ret["m_Round"].AsInt;
        l = GetArray(ret["m_ChallengeAnswer"]);
        for (int i = 0; i < 6; i++)
        {
            gi.m_ChallengeAnswer.Add(l[i]);
        }
        gi.m_ChallengeScoreB = ret["m_ChallengeScoreB"].AsInt;
        l = GetArray(ret["m_PlayerBIDItems"]);
        for (int i = 0; i < 8; i++)
        {
            gi.m_PlayerBIDItems.Add(l[i]);
        }
        l = GetArray(ret["m_ChallengeQuestion"]);
        for (int i = 0; i < 6; i++)
        {
            gi.m_ChallengeQuestion.Add(l[i]);
        }
        gi.m_CurrentTurn = ret["m_CurrentTurn"].AsInt;
        gi.m_PlayerAIDAbilityUsed = ret["m_PlayerAIDAbilityUsed"].AsInt;
        l = GetArray(ret["m_PlayerAIDItems"]);
        for (int i = 0; i < 8; i++)
        {
            gi.m_PlayerAIDItems.Add(l[i]);
        }
        gi.m_TrophyAcquired = ret["m_TrophyAcquired"].AsInt;
        gi.m_PlayerAIDJobs = ret["m_PlayerAIDJobs"].AsInt;
        l = GetArray(ret["m_PieceA"]);
        for (int i = 0; i < 6; i++)
        {
            gi.m_PieceA.Add(l[i]);
        }
        gi.m_BetTrophyB = ret["m_BetTrophyB"].AsInt;
        gi.m_ChallengeState = ret["m_ChallengeState"].AsInt;
        gi.m_PlayerAID = Truncate(ret["m_PlayerAID"]);
        l = GetArray(ret["m_PieceB"]);
        for (int i = 0; i < 6; i++)
        {
            gi.m_PieceB.Add(l[i]);
        }
        gi.m_PlayerBIDTier = ret["m_PlayerBIDTier"].AsInt;
        gi.m_Challenger = Truncate(ret["m_Challenger"]);
        gi.m_PlayerBIDJobs = ret["m_PlayerBIDJobs"].AsInt;
        gi.m_AbilityShowed = ret["m_AbilityShowed"].AsBool;
        gi.m_PlayerBIDSex = ret["m_PlayerBIDSex"].AsInt;
        gi.m_PlayerAIDTier = ret["m_PlayerAIDTier"].AsInt;
        gi.m_ChallengeScoreA = ret["m_ChallengeScoreA"].AsInt;
        gi.m_LastAbility = ret["m_LastAbility"].AsInt;
        gi.m_SpinProgressB = ret["m_SpinProgressB"].AsInt;
        gi.m_SpinProgressA = ret["m_SpinProgressA"].AsInt;
        gi.m_IsCompleted = ret["m_IsCompleted"].AsBool;
        gi.m_BetTrophyA = ret["m_BetTrophyA"].AsInt;
        gi.m_PlayerBID = Truncate(ret["m_PlayerBID"]);
        gi.m_PlayerBIDAbilityUsed = ret["m_PlayerBIDAbilityUsed"].AsInt;
        gi.m_SessionID = Truncate(ret["m_SessionID"]);
        gi.m_TurnType = ret["m_TurnType"].AsInt;
        gi.m_CurrentRound = ret["m_CurrentRound"].AsInt;
        gi.m_TrophyRemoved = ret["m_TrophyRemoved"].AsInt;
        gi.m_PlayerUseAbility = Truncate(ret["m_PlayerUseAbility"]);
        gi.m_PlayerAIDSex = ret["m_PlayerAIDSex"].AsInt;

        gi.m_PlayerAName = Truncate(ret["m_PlayerAName"]);
        gi.m_PlayerBName = Truncate(ret["m_PlayerBName"]);                

        if (join)
        {
            gi.m_PlayerBID = GameManager.Instance.GetPlayerProfile().m_PlayerID;
        }

        return gi;
    }

    private static string Truncate(string s) {
        if (s[0] == '\"' && s[s.Length - 1] == '\"')
        {
            s = s.Substring(1, s.Length - 2);
        }
        return s;
    }

    static private List<int> GetArray(string s)
    {
        s = Truncate(s);
        List<int> l = new List<int>();
        s = s.Substring(1, s.Length - 2);        
        string[] subs = s.Split(',');
        for (int i = 0; i < subs.GetLength(0); i++)
        {
            l.Add(int.Parse(subs[i]));
        }
        return l;
    }

    public bool IsMyTurn(string uid)
    {
        if (uid == m_PlayerAID)
        {
            return m_CurrentTurn == 1;
        }
        if (uid == m_PlayerBID)
        {
            return m_CurrentTurn == 2;
        }
        return false;
    }

    public string GetOpponentName(string uid)
    {
        if (uid == m_PlayerAID)
        {
            return m_PlayerBName;
        }
        if (uid == m_PlayerBID)
        {
            return m_PlayerAName;
        }
        return "";
    }

    public Avatar GetOpponentAvatar(string uid) {
        Avatar ava = Avatar.CreateDefaultAvatar();
        if (uid == m_PlayerAID)
        {
            ava.m_Tier = (TIER)m_PlayerBIDTier;
            ava.m_Jobs = (CLASS)m_PlayerBIDJobs;
            for (int i = 0; i < 8; i++)
            {
                ava.m_ItemList[i] = m_PlayerBIDItems[i];
            }
        }
        if (uid == m_PlayerBID)
        {
            ava.m_Tier = (TIER)m_PlayerAIDTier;
            ava.m_Jobs = (CLASS)m_PlayerAIDJobs;
            for (int i = 0; i < 8; i++)
            {
                ava.m_ItemList[i] = m_PlayerAIDItems[i];
            }
        }
        return ava;
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

    public GameInfo AddNewGame(string sessionid, string uid1, string uid2)
    {
        Debug.Log("NewGameID: " + sessionid);
        GameInfo gi = new GameInfo();
        gi.m_PlayerAID = uid1;
        gi.m_PlayerBID = uid2;

        if (uid2 == null || uid2 == "")
        {
            gi.m_PlayerBName = "Waiting Opponent";            
        }
        
        gi.m_SessionID = sessionid;
        for (int i = 0; i < 6; i++)
        {
            gi.m_PieceA.Add(0);
            gi.m_PieceB.Add(0);
            gi.m_ChallengeQuestion.Add(0);
            gi.m_ChallengeAnswer.Add(0);
        }

        gi.m_PlayerAIDSex = GameManager.Instance.GetPlayerProfile().m_Sex;
        gi.m_PlayerAIDTier = (int)GameManager.Instance.GetPlayerProfile().GetActiveAvatar().m_Tier;
        gi.m_PlayerAIDJobs = (int)GameManager.Instance.GetPlayerProfile().GetActiveAvatar().m_Jobs;
        for (int i = 0; i < 8; i++)
        {
            gi.m_PlayerAIDItems.Add(GameManager.Instance.GetPlayerProfile().GetActiveAvatar().m_ItemList[i]);
            gi.m_PlayerBIDItems.Add(0);
        }
        
        m_GameList.Add(gi);
        Save();
        
        return gi;
    }
    

    public GameInfo AddExistingGame(string sessioninfo, bool join = false)
    {
        GameInfo gi = GameInfo.FromJsonString(sessioninfo, join);
        if (GameManager.Instance.UpdateMyInfoIfMissing(gi))
        {
            GameManager.Instance.UpdateSpecificSession(gi);
        }
        bool found = false;
        for (int i = 0; i < m_GameList.Count; i++)
        {
            if (m_GameList[i].m_SessionID == gi.m_SessionID && gi.IsMyTurn(GameManager.Instance.GetPlayerID()) && !m_GameList[i].IsMyTurn(GameManager.Instance.GetPlayerID()))
            {                
                m_GameList[i] = gi;
                found = true;
                break;
            }
            if (m_GameList[i].m_SessionID == gi.m_SessionID)
            {                
                found = true;
                break;
            }
        }
        if (!found)
        {
            m_GameList.Add(gi);
        }
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


