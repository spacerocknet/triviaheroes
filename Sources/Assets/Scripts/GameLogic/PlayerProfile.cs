using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("PlayerProfile")]
public class PlayerProfile{
    public string m_PlayerID;
    public string m_PlayerName;
    public int m_Level;
    public int m_LevelEXP;
    public int m_ExpToLevelUP;
    public int m_TotalEXP;
    public int m_Lives;
    public int m_Coin;
    public int m_Diamond;
    public int m_Sex; //0 - male, 1 - female, 2 - unisex
    public DateTime m_LastTimeAddLive;
    public FriendList m_FriendList;

    public PlayerProfile()
    {
        m_PlayerName = "NULL";
        m_PlayerID = "NULL";
        m_Level = 0;
        m_LevelEXP = 0;
        m_TotalEXP = 0;
        m_Lives = 5;
        m_Coin = 0;
        m_Diamond = 0;
        m_LastTimeAddLive = DateTime.Now;
        m_ExpToLevelUP = 100;

        m_FriendList = FriendList.CreateRandomFriendList();
    }

    public PlayerProfile(string name, int sex)
    {
        m_PlayerName = name;
        m_PlayerID = name;
        m_Level = 0;
        m_LevelEXP = 0;
        m_TotalEXP = 0;
        m_Lives = 5;
        m_Coin = 0;
        m_Diamond = 0;
        m_Sex = sex;
        m_LastTimeAddLive = DateTime.Now;
        m_ExpToLevelUP = 100;

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

}
