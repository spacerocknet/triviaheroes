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
    private int m_LastAnswerChoice;

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
        cs.gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());        
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
            SimulateOtherPlayers();
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
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Hide();
        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cs.MoveInFromRight();
        Question q = new Question(result);
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(q);
        SetCurrentQuestion(q);
        SetPVPState(PVPStateType.NORMAL, Category.CAT_ART, Category.CAT_ART);
    }

    public void OnTrophyClaimSelectedResult(string result)
    {
        Debug.Log(result);
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Hide();
        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cs.MoveInFromRight();
        Question q = new Question(result);
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(q);
        SetCurrentQuestion(q);
    }

    public void OnTrophyChallengeSelectedResult(string result)
    {
        Debug.Log(result);
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Hide();
        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cs.MoveInFromRight();
        Question q = new Question(result);
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(q);
        SetCurrentQuestion(q);
        m_PVPState.m_CurrentQuestion = 0;
    }

    public void DoTrophyChallangeNextQuestionResult(string result) {
        Debug.Log(result);        
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);        
        Question q = new Question(result);
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(q);
        SetCurrentQuestion(q);
        m_PVPState.m_CurrentQuestion++;
    }

    public PlayerProfile GetPlayerProfile()
    {
        return m_PlayerProfile;
    }

    public void OnStartPVPGame(string friend)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Show();
        NetworkManager.Instance.DoStartNewGame(friend);
    }

    public void OnCategoryConfirmToPlay(Category cat)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Show();
        NetworkManager.Instance.DoCategoryConfirmToPlay(cat);
    }

    public void SetCurrentQuestion(Question q)
    {
        m_CurrentQuestion = q;
    }

    public void OnAnswerSelect(int select)
    {
        if (select == m_CurrentQuestion.m_CorrectAnswer)
        {
            //Right
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
            cs.gameObject.GetComponent<UIQuestion>().ShowAnswer(select, m_CurrentQuestion.m_CorrectAnswer);
            m_IsLastAnswerCorrect = true;
            m_LastAnswerChoice = select;
        }
        else
        {
            //Wrong
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
            cs.gameObject.GetComponent<UIQuestion>().ShowAnswer(select, m_CurrentQuestion.m_CorrectAnswer);
            m_IsLastAnswerCorrect = false;
            m_LastAnswerChoice = select;
        }
    }

    public void OnShowAnswerEnded()
    {
        if (m_PVPState.m_Type != PVPStateType.CHALLENGE || m_PVPState.m_CurrentQuestion == 5)
        {
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
            cs.MoveOutToRight();
        }


        if (m_PVPState.m_Type == PVPStateType.NORMAL)
        {
            HandleNormalAnswerEnded();                
        }
        else if (m_PVPState.m_Type == PVPStateType.TROPHY)
        {
            HandleTrophyAnswerEnded();            
        }
        else if (m_PVPState.m_Type == PVPStateType.CHALLENGE)
        {
            HandleChallengeAnswerEnded();
        }

    }

    void HandleNormalAnswerEnded()
    {
        if (m_IsLastAnswerCorrect)
        {
            AddProgress();
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
            cs.gameObject.GetComponent<UIPvP>().UpdateProgress(GetMySpinProgress());
        }
        else
        {
            EndTurn();
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
            cs.gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
        }
        
    }

    void HandleTrophyAnswerEnded()
    {
        if (m_IsLastAnswerCorrect)
        {
            ClearProgress();
            TrophyAcquired(m_PVPState.m_TargetTrophy);
        }
        else
        {
            ClearProgress();
            EndTurn();
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
            cs.gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
        }
    }

    void HandleChallengeAnswerEnded()
    {
        SetChallengeQuestion(m_PVPState.m_CurrentQuestion, m_CurrentQuestion.m_QID, m_LastAnswerChoice);
        

        if (m_IsLastAnswerCorrect)
        {
            AddMyChallengeScore();            
        }

        if (m_PVPState.m_CurrentQuestion == 5)
        {
        }
        else
        {
            NetworkManager.Instance.DoTrophyChallangeNextQuestion();
        }
        EndTurn();
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
        cs.gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
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

        NetworkManager.Instance.DoTrophyChallengeSelected();
        SetPVPState(PVPStateType.CHALLENGE, (Category)mytrophy, (Category)theirtrophy);

        m_GameList.m_GameList[m_CurrentGame].m_Challenger = m_PlayerProfile.m_PlayerID;

        SetMyBetTrophy(mytrophy);
        SetOpponentBetTrophy(theirtrophy);
    }

    public void SetPVPState(PVPStateType type, Category targetTrophy, Category betTrophy)
    {
        m_PVPState.m_Type = type;
        m_PVPState.m_BetTrophy = targetTrophy;
        m_PVPState.m_BetTrophy = betTrophy;
        m_PVPState.m_CurrentQuestion = 0;        
    }

    public void TrophyAcquired(Category m_Trophy)
    {
        m_GameList.m_GameList[m_CurrentGame].m_PieceA[(int)m_Trophy] = 1;
        m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA = 0;
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
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

    public void EndTurn()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_Round++;
        }

        if (m_PVPState.m_Type == PVPStateType.CHALLENGE)
        {
            m_GameList.m_GameList[m_CurrentGame].m_ChallengeState++;
        }
        
        m_GameList.m_GameList[m_CurrentGame].m_CurrentTurn = 3 - m_GameList.m_GameList[m_CurrentGame].m_CurrentTurn;
        m_GameList.Save();
    }

    private void SimulateOtherPlayers()
    {
        for (int i = 0; i < m_GameList.m_GameList.Count; i++)
        {
            GameInfo gi = m_GameList.m_GameList[i];
            if (gi.m_CurrentTurn == 2)
            {
                if (Random.Range(0, 5) > 0)
                {
                    gi.m_CurrentTurn = 1;
                    //AddOneSpin
                    if (Random.Range(0, 2) == 0)
                    {
                        gi.m_SpinProgressB++;
                        gi.m_SpinProgressB = Mathf.Clamp(gi.m_SpinProgressB, 0, 2);
                    }
                    //AddOneTrophy
                    if (Random.Range(0, 2) == 1)
                    {
                        for (int k = 0; k < gi.m_PieceB.Count; k++)
                        {
                            if (gi.m_PieceB[k] == 0)
                            {
                                gi.m_PieceB[k] = 1;
                                break;
                            }
                        }
                    }
                    //Finish challenge
                    if (m_GameList.m_GameList[m_CurrentGame].m_ChallengeState == 1)
                    {
                        m_GameList.m_GameList[m_CurrentGame].m_ChallengeState++;
                        m_GameList.m_GameList[m_CurrentGame].m_ChallengeScoreB = 0;
                    }
                }
            }            
        }
        m_GameList.Save();
    }

#region SessionInfoManupilate
    public void SetMyBetTrophy(int trophy)
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_BetTrophyA = trophy;
            
        }
        else
        {
            m_GameList.m_GameList[m_CurrentGame].m_BetTrophyB = trophy;
        }
    }

    public void SetOpponentBetTrophy(int trophy)
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_BetTrophyB = trophy;
        }
        else
        {
            m_GameList.m_GameList[m_CurrentGame].m_BetTrophyA = trophy;
        }
    }

    public void AddMyChallengeScore()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_ChallengeScoreA++;
        } else 
        {
            m_GameList.m_GameList[m_CurrentGame].m_ChallengeScoreB++;
        }
    }

    public void AddProgress()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA++;
        }
        else
        {
            m_GameList.m_GameList[m_CurrentGame].m_SpinProgressB++;
        }        
        SaveSessionList();
    }

    public void FulfillProgress()
    {        
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA = 3;
        }
        else
        {
            m_GameList.m_GameList[m_CurrentGame].m_SpinProgressB = 3;
        }    
        SaveSessionList();
    }

    public void ClearProgress()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA = 0;
        }
        else
        {
            m_GameList.m_GameList[m_CurrentGame].m_SpinProgressB = 0;
        }   
        SaveSessionList();
    }

    public int GetMySpinProgress()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            return m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA;
        }
        else
        {
            return m_GameList.m_GameList[m_CurrentGame].m_SpinProgressB;
        }   
    }

    public void SetChallengeQuestion(int index, int qid, int answer)
    {
        m_GameList.m_GameList[m_CurrentGame].m_ChallengeAnswer[index] = answer;
        m_GameList.m_GameList[m_CurrentGame].m_ChallengeQuestion[index] = qid;
    }

    public GameInfo GetCurrentGameInfo()
    {
        return m_GameList.m_GameList[m_CurrentGame];
    }

    public List<int> GetCurrentTrophyState()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            return m_GameList.m_GameList[m_CurrentGame].m_PieceA;
        }
        else
        {
            return m_GameList.m_GameList[m_CurrentGame].m_PieceB;
        }
    }

    public void SetTrophyAcquired(int trophy)
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerA == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_PieceA[trophy] = 1;
        }
        else
        {
            m_GameList.m_GameList[m_CurrentGame].m_PieceB[trophy] = 1;
        }
    }
#endregion
}
