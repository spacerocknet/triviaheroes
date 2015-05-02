using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfo
{
    public int m_GameID;
    public string m_Opponent;
    public int m_Round = 1;
    public int[] m_Piece1 = {0, 0, 0, 0, 0, 0};
    public int[] m_Piece2 = {0, 0, 0, 0, 0, 0};
    public int m_Turn;
    public int m_SpinProgress;
}

public class GameList
{

    public List<GameInfo> m_GameList = new List<GameInfo>();

    public static GameList CreateEmptyGameList()
    {
        GameList m_GameList = new GameList();
        return m_GameList;
    }

    public GameInfo AddNewGame(string opponent)
    {
        GameInfo gi = new GameInfo();
        gi.m_Opponent = opponent;
        m_GameList.Add(gi);
        return gi;
    }
}
