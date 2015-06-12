using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;

public enum TIER { Toddler = 1, Child, Teenager, Young_Adult, Adult_1, Adult_2, Adult_3, Adult_4, Adult_5, Elder };
public enum CLASS { None = 0, Medicial = 1, Scientist, Athlete, Enterpreneur, Warrior, Musician};

[XmlRoot("PlayerProfile")]
public class PlayerProfile{
    public string m_PlayerID;
    public string m_PlayerName;
    public int m_Level;
    //public int m_LevelEXP;
    //public int m_ExpToLevelUP;
    public long m_TotalEXP;
    public int m_Lives;
    public int m_Coin;
    public int m_Diamond;
    public int m_Sex; //0 - male, 1 - female, 2 - unisex
    public TIER m_CurrentTier;
    public CLASS m_CurrentClass;
    public DateTime m_LastTimeAddLive;
    public FriendList m_FriendList;
    public List<int> m_ItemCat;
    public List<int> m_ItemID;
    [XmlArray("Avatar")]
    [XmlArrayItem("Avatar")]
    public List<Avatar> m_AvatarList;
    public int m_ActiveAvatar;
    public int m_CurrentPVEStage = 0;
    public List<int> m_PVEState;
    public List<int> m_AchievementCounter;
    public List<int> m_AchievementBonusReceived;
    public int m_PayOutBonus;

    public PlayerProfile()
    {
        m_PlayerName = "NULL";
        m_PlayerID = "NULL";
        m_Level = 0;
        
        m_TotalEXP = 0;
        m_Lives = 5;
        m_Coin = 0;
        m_Diamond = 0;
        m_LastTimeAddLive = DateTime.Now;
        
        m_CurrentTier = TIER.Young_Adult;
        m_ItemCat = new List<int>();
        m_ItemID = new List<int>();
        m_AvatarList = new List<Avatar>();
        m_ActiveAvatar = 0;
        m_PayOutBonus = 1;
        for (int i = 0; i < 8; i++)
        {
            //m_ItemPicked.Add(0);
        }

        m_FriendList = FriendList.CreateRandomFriendList();
    }

    public PlayerProfile(string name, int sex)
    {
        m_PlayerName = name;
        m_PlayerID = name;
        m_Level = 0;
        
        m_TotalEXP = 0;
        m_Lives = 5;
        m_Coin = 0;
        m_Diamond = 0;
        m_Sex = sex;
        m_LastTimeAddLive = DateTime.Now;
        
        m_CurrentTier = TIER.Young_Adult;
        m_ItemCat = new List<int>();
        m_ItemID = new List<int>();
        m_AvatarList = new List<Avatar>();
        m_ActiveAvatar = 0;
        m_PayOutBonus = 1;
        m_PVEState = new List<int>();
        m_AchievementCounter = new List<int>();
        m_AchievementBonusReceived = new List<int>();

        for (int i = 0; i < 23; i++)
        {
            m_AchievementCounter.Add(0);
            m_AchievementBonusReceived.Add(0);
        }

        for (int i = 0; i < GameConfig.Instance.GetNumberOfPvEStage(); i++)
        {
            m_PVEState.Add(0);
        }
        
        m_AvatarList.Add(Avatar.CreateDefaultAvatar());

        m_FriendList = FriendList.CreateRandomFriendList();
    }

    public void Save(string path = "TriviaPlayerProfile.xml")
    {
        path = Utils.pathForDocumentsFile(path);
        var serializer = new XmlSerializer(typeof(PlayerProfile));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static PlayerProfile Load(string path = "TriviaPlayerProfile.xml")
    {
        path = Utils.pathForDocumentsFile(path);
        var serializer = new XmlSerializer(typeof(PlayerProfile));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as PlayerProfile;
        }
    }

    public void ItemSelected(int cat, int id)
    {
        m_AvatarList[m_ActiveAvatar].m_ItemList[cat] = id;
    }

    public int GetCurrentLevel()
    {
        Debug.Log(m_TotalEXP);
        int level = 1;
        long exp = m_TotalEXP;
        while (exp >= GameConfig.Instance.GetLevelEXP(level - 1))
        {           
            exp = exp - GameConfig.Instance.GetLevelEXP(level - 1);
            Debug.Log(exp);
            level++;
        }
        return level;
    }

    public long GetLevelEXP()
    {
        int level = 1;
        long exp = m_TotalEXP;
        while (exp >= GameConfig.Instance.GetLevelEXP(level - 1))
        {
            exp = exp - GameConfig.Instance.GetLevelEXP(level - 1);
            level++;
        }
        return exp;
    }

    public void AddLives()
    {
        Debug.Log("ADD LIVES");
        m_LastTimeAddLive = DateTime.Now;        
        m_Lives++;
        m_Lives = Mathf.Clamp(m_Lives, 0, 5);
    }

    public void SubtractLives()
    {
        Debug.Log("SUBTRACT LIVES");
        if (m_Lives == 5)
        {
            m_LastTimeAddLive = DateTime.Now;
        }
        m_Lives--;
        m_Lives = Mathf.Clamp(m_Lives, 0, 5);
    }

    public void UpdateLives()
    {
        DateTime now = DateTime.Now;

        TimeSpan s = now - m_LastTimeAddLive;

        m_Lives = m_Lives + (int)s.TotalSeconds / (1 * 60);
        m_Lives = Mathf.Clamp(m_Lives, 0, 5);

        s = TimeSpan.FromSeconds((int)s.TotalSeconds % (1 * 60));

        m_LastTimeAddLive = now - s;

        Save();
    }

    public void AddNewAvatar()
    {
        Avatar av = Avatar.CreateDefaultAvatar();
        av.m_ID = m_AvatarList.Count;
        m_AvatarList.Add(av);
        m_ActiveAvatar = m_AvatarList.Count - 1;
        Save();
        AchievementList.Instance.OnAction(Achievement_Action.COLLECT_AVATAR);
    }
    
}
