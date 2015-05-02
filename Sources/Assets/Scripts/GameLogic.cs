using UnityEngine;
using System.Collections;

public class SingleGame
{
    public int m_CurrentQuestion;
}

public enum GameMode { GAMEMODE_PVP, GAMEMODE_PVE };

public class GameLogic : MonoBehaviour
{
    private static GameLogic m_sInstance = null;

    private GameMode m_GameMode;

    public void Awake()
    {
        m_sInstance = this;
    }

    private GameLogic()
    {
        m_sInstance = this;
    }

    public static GameLogic Instance
    {
        get
        {
            if (m_sInstance == null)
            {
                m_sInstance = new GameLogic();
            }
            return m_sInstance;
        }
    }

    public void SetSelectedMode(GameMode mode)
    {
        m_GameMode = mode;
    }

    public void OnStartPVEGame()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cs.MoveInFromRight();
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(GetRandomQuestion());
    }




    Question GetRandomQuestion()
    {
        Question q = new Question("");
        q.m_Question = "Blah blah blah";
        q.m_Answer0 = "Answer 1";
        q.m_Answer1 = "Answer 2";
        q.m_Answer2 = "Answer 3";
        q.m_Answer3 = "Answer 4";
        return q;
    }
}