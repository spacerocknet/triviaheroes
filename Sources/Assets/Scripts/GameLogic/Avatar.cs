using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Avatar  {

    public enum AVATAR_PIECE { SHIRTS = 0, GLASSES, PETS, HAIR, EYES, NOSE, LIPS, FACIAL_HAIR };
    static public string[] AVATAR_PREFIX = {"C", "G", "P", "H", "E", "N", "L", "F"};

    public List<int> m_ItemList;

    static Avatar CreateDefaultAvatar()
    {
        Avatar av = new Avatar();
        for (int i = 0; i < 8; i++)
        {
            av.m_ItemList.Add(0);
        }
        return av;
    }
}
