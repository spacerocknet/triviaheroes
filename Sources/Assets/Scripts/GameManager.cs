using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public enum Category {CAT_GEOGRAPHY = 0, CAT_SCIENCE, CAT_ART, CAT_HISTORY, CAT_SPORT, CAT_ENTERTAINMENT, CAT_CROWN};


public class GameManager : MonoBehaviour {

    public RectTransform m_MainPanel;
    public GameObject m_GameInfoPrefab;

    public List<GameObject> m_GameInfoList;

    private PlayerProfile m_PlayerProfile;

    public GameList m_GameList;

    private static GameManager m_sInstance = null;

    private int m_CurrentGame;

    private Question m_CurrentQuestion;

    private int m_CurrentQuestionType;

    private bool m_IsLastAnswerCorrect;

    private PVPState m_PVPState = new PVPState();


    public void Awake()
    {
        m_sInstance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    private GameManager()
    {
        m_sInstance = this;
        
    }

    public static GameManager Instance
    {
        get
        {
            if (m_sInstance == null)
            {
                m_sInstance = new GameManager();
            }
            return m_sInstance;
        }
    }

	// Use this for initialization
	void Start () {
        LoadPlayerProfile();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnContinueGame(int gameid)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
        cs.MoveInFromRight();
        m_CurrentGame = GameManager.Instance.GetGameIndexByID(gameid);
        GameInfo gi = GameManager.Instance.m_GameList.m_GameList[m_CurrentGame];
        cs.gameObject.GetComponent<UIPvP>().SetGameInfo(gi);        
    }

    public void OnMultiPlayer()
    {

    }

    public void OnSinglePlayer()
    {

    }

    public void OnTheirTurn()
    {
    }

    public void OnYourTurn()
    {
    }

    public void OnPastGame()
    {
    }

    public void LoadPlayerProfile()
    {
        Debug.Log("LoadPlayerProfile");
        if (System.IO.File.Exists("TriviaPlayerProfile.xml"))
        {
            m_PlayerProfile = PlayerProfile.Load();
            GameObject go = GameObject.Find("GameLoading");
            m_GameList = GameList.Load();
            go.GetComponent<LoadingScene>().SwitchToMainScene();
            Debug.Log(m_PlayerProfile.m_PlayerName);            
        }
        else
        {
            GameObject go = GameObject.Find("GameLoading");
            go.GetComponent<LoadingScene>().SwitchToRegisterScene();
            Debug.Log("2");
            m_GameList = GameList.CreateEmptyGameList();
        }
    }

    public void OnRegisterResult(string result)
    {
        var ret = JSONNode.Parse(result);
        if (ret["result"].AsBool)
        {
            Debug.Log("Register Successful");
            m_PlayerProfile = new PlayerProfile(ret["name"], ret["sex"].AsInt);
            Application.LoadLevel("MainScene");
            m_PlayerProfile.Save();
        }
        else
        {
            Debug.Log("Register Unsuccessful");
        }        
    }

    public void OnStartNewGameResult(string result)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Hide();
        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
        cs.MoveInFromRight();
        var ret = JSONNode.Parse(result);
        GameInfo gi = GameManager.Instance.m_GameList.AddNewGame(ret["opponent"]);
        cs.gameObject.GetComponent<UIPvP>().SetGameInfo(gi);
        m_CurrentGame = GameManager.Instance.m_GameList.m_GameList.Count - 1;
    }

    public void OnCategoryConfirmToPlayResult(string result)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cs.MoveInFromRight();
        Question q = new Question(result);
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(q);
        SetCurrentQuestion(q);
        SetPVPState(PVPStateType.NORMAL, Category.CAT_ART, Category.CAT_ART);
    }

    public void OnTrophyClaimSelectedResult(string result)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Hide();
        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cs.MoveInFromRight();
        Question q = new Question(result);
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(q);
        SetCurrentQuestion(q);
    }

    public PlayerProfile GetPlayerProfile()
    {
        return m_PlayerProfile;
    }

    public void OnStartPVPGame(string friend)
    {
        //CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        //cs.MoveInFromRight();
        //cs.gameObject.GetComponent<UIQuestion>().SetQuestion(GetRandomQuestion());

        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Show();
        NetworkManager.Instance.DoStartNewGame(friend);
    }

    public void OnCategoryConfirmToPlay(Category cat)
    {
        NetworkManager.Instance.DoCategoryConfirmToPlay(cat);
    }

    public void AddProgress()
    {
        m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA++;
        SaveSessionList();
    }

    public void FulfillProgress()
    {
        m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA = 3;
        SaveSessionList();
    }

    public void ClearProgress()
    {
        m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA = 0;
        SaveSessionList();
    }

    public void SetCurrentQuestion(Question q)
    {
        m_CurrentQuestion = q;
    }

    public void OnAnswerSelect(int select)
    {
        if (select == 0)
        {
            //Right
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
            cs.gameObject.GetComponent<UIQuestion>().ShowAnswer(select, m_CurrentQuestion.m_CorrectAnswer);
            m_IsLastAnswerCorrect = true;
        }
        else
        {
            //Wrong
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
            cs.gameObject.GetComponent<UIQuestion>().ShowAnswer(select, m_CurrentQuestion.m_CorrectAnswer);
            m_IsLastAnswerCorrect = false;
        }
    }

    public void OnShowAnswerEnded()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cs.MoveOutToRight();

        if (m_IsLastAnswerCorrect)
        {
            if (m_PVPState.m_Type == PVPStateType.NORMAL)
            {
                AddProgress();
                cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
                cs.gameObject.GetComponent<UIPvP>().UpdateProgress(m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA);
            }
            else if (m_PVPState.m_Type == PVPStateType.TROPHY)
            {
                TrophyAcquired(m_PVPState.m_TargetTrophy);
            }
        }
        else
        {
        }
    }

    public void OnFullProgress()
    {
        m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA = 0;
        SaveSessionList();
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_CROWNSELECT);
        cs.MoveInFromRight();
    }

    public void OnSelectClaim()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECLAIM);
        cs.MoveInFromRight();

        cs.gameObject.GetComponent<UISelectPieceClaim>().UpdateTrophyState();
    }

    public void OnSelectChallenge()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECHALLENGE);
        cs.MoveInFromRight();

        cs.gameObject.GetComponent<UISelectPieceChallenge>().SetTrophyState(GetMyTrophy(), GetOpponentTrophy());
    }

    public List<int> GetCurrentTrophyState()
    {
        return m_GameList.m_GameList[m_CurrentGame].m_PieceA;
    }

    public void OnTrophyClaimSelected(int trophy)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Show();

        NetworkManager.Instance.DoTrophyClaimSelected(trophy);
        SetPVPState(PVPStateType.TROPHY, (Category)trophy, Category.CAT_ART);
    }

    public void OnTrophyChallengeSelected(int mytrophy, int theirtrophy)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Show();

        //NetworkManager.Instance.DoTrophyClaimSelected(trophy);
        SetPVPState(PVPStateType.CHALLENGE, (Category)mytrophy, (Category)theirtrophy);
    }

    public void SetPVPState(PVPStateType type, Category targetTrophy, Category betTrophy)
    {
        m_PVPState.m_Type = type;
        m_PVPState.m_TargetTrophy = targetTrophy;
        m_PVPState.m_BetTrophy = betTrophy;
        m_PVPState.m_CurrentQuestion = 0;
    }

    public void TrophyAcquired(Category m_Trophy)
    {
        m_GameList.m_GameList[m_CurrentGame].m_PieceA[(int)m_Trophy] = 1;
        m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA = 0;
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(m_GameList.m_GameList[m_CurrentGame]);
        SaveSessionList();
    }

    public int GetGameIndexByID(int gameid) {
        for (int i = 0; i < m_GameList.m_GameList.Count; i++)
        {
            if (m_GameList.m_GameList[i].m_GameID == gameid) {
                return i;
            }
        }
        return -1;
    }

    public void SaveSessionList()
    {
        m_GameList.Save();
    }

    public List<int> GetMyTrophy()
    {
        return m_GameList.m_GameList[m_CurrentGame].m_PieceA;
    }

    public List<int> GetOpponentTrophy()
    {
        return m_GameList.m_GameList[m_CurrentGame].m_PieceB;
    }
}
