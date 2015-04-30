using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendList  {

    public List<string> m_FriendList = new List<string>();

    public static FriendList CreateRandomFriendList()
    {
        FriendList fl;
        fl = new FriendList();
        fl.m_FriendList.Add("Mage");
        fl.m_FriendList.Add("Warrior");
        fl.m_FriendList.Add("Rogue");
        fl.m_FriendList.Add("Druid");
        fl.m_FriendList.Add("Warlock");
        fl.m_FriendList.Add("Shaman");
        fl.m_FriendList.Add("Hunter");
        fl.m_FriendList.Add("Paladin");
        fl.m_FriendList.Add("Priest");
        return fl;
    }
}
