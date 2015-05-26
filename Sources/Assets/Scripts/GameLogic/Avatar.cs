using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;


[XmlRoot("PlayerAvatar")]
public class Avatar  {

    public enum AVATAR_PIECE { SHIRTS = 0, GLASSES, PETS, HAIR, EYES, NOSE, LIPS, FACIAL_HAIR };
    static public string[] AVATAR_PREFIX = {"C", "G", "P", "H", "E", "N", "L", "F", "S"};

    public List<int> m_ItemList;
    public int m_Sex = 0; //Female default
    public int m_ID = 0; //ID of player's avatar
    public TIER m_Tier = TIER.Toddler; //Toodler
    public CLASS m_Jobs = CLASS.None; //No jobs

    public static Avatar CreateDefaultAvatar()
    {
        Avatar av = new Avatar();
        av.m_ItemList = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            av.m_ItemList.Add(0);
        }
        return av;
    }

    public string ToString()
    {
        return "Sex: " + m_Sex + " Tier: " + m_Tier + " Jobs: " + m_Jobs;
    }
}
