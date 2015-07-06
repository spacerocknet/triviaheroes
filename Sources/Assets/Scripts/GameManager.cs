using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public enum Category {CAT_GEOGRAPHY = 0, CAT_SCIENCE, CAT_ART, CAT_HISTORY, CAT_SPORT, CAT_ENTERTAINMENT, CAT_CROWN};

public enum Ability { ABILITY_NONE = -1, ABILITY_CLAIM = 0, ABILITY_UNDO, ABILITY_CHALLENGE, ABILITY_SWITCH, ABILITY_REMOVE, ABILITY_COPY };

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

    public bool m_IsPVE = false;    

    private PVPState m_PVPState = new PVPState();

    private string m_RegisterName;
    private int m_Sex;


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

    public void OnContinueGame(string sessionid)
    {
        m_IsPVE = false;
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
        cs.MoveInFromRight();
        m_CurrentGame = GameManager.Instance.GetGameIndexByID(sessionid); 
        cs.gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());

        GameInfo gi = GetCurrentGameInfo();
        if (gi.m_PlayerUseAbility != m_PlayerProfile.m_PlayerID && gi.m_AbilityShowed == false && gi.m_LastAbility != -1)
        {
            if (GetActiveAbility() != Ability.ABILITY_UNDO)
            {
                CanvasScript css = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                css.GetComponent<UIPopup>().Show(gi.m_PlayerUseAbility + " used " + TextManager.Instance.GetAbilityName((Ability)gi.m_LastAbility) + " ability.", 0, null, null, (int)CanvasID.CANVAS_PVP);
                gi.m_AbilityShowed = true;
            }
            else
            {
                CanvasScript css = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                UIPvP uipvp = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).GetComponent<UIPvP>();
                css.GetComponent<UIPopup>().Show(gi.m_PlayerUseAbility + " used " + TextManager.Instance.GetAbilityName((Ability)gi.m_LastAbility) + " ability. Undo it?", 1, uipvp.OnUseAbilityConfirm, null, (int)CanvasID.CANVAS_PVP);
                gi.m_AbilityShowed = true;
            }
        }
        SaveSessionList();
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
        
        if (System.IO.File.Exists(Utils.pathForDocumentsFile("TriviaPlayerProfile.xml")))
        {
            m_PlayerProfile = PlayerProfile.Load();
            AchievementList.Instance.Init();
            GameObject go = GameObject.Find("GameLoading");
            //m_GameList = GameList.Load();
            m_GameList = GameList.CreateEmptyGameList();
            go.GetComponent<LoadingScene>().SwitchToMainScene();
            
            //SimulateOtherPlayers();
            m_PlayerProfile.UpdateLives();
        }
        else
        {
            GameObject go = GameObject.Find("GameLoading");
            go.GetComponent<LoadingScene>().SwitchToRegisterScene();
            //Debug.Log("2");
            m_GameList = GameList.CreateEmptyGameList();
        }
    }

    public void OnRegisterResult(string result)
    {
        var ret = JSONNode.Parse(result);
        if (ret["result"].AsBool)
        {
            //Debug.Log("Register Successful");
            m_PlayerProfile = new PlayerProfile(ret["name"], ret["sex"].AsInt, ret["uid"]);
            Application.LoadLevel("MainScene");
            m_PlayerProfile.Save();
            AchievementList.Instance.Init();
        }
        else
        {
            //Debug.Log("Register Unsuccessful");
        }        
    }

    public void OnStartNewGameResult(string result)
    {
        Debug.Log(result);
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Hide();
        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
        cs.MoveInFromRight();
        var ret = JSONNode.Parse(result);
        if (ret["attributes"].ToString().Length < 15)
        {
            GameInfo gi = GameManager.Instance.m_GameList.AddNewGame(ret["gameSessionId"], ret["uid1"], ret["uid2"]);
            m_CurrentGame = GameManager.Instance.m_GameList.m_GameList.Count - 1;
            UpdateMyInfoOnCurrentGame();
            cs.gameObject.GetComponent<UIPvP>().SetGameInfo(gi);            
        }
        else
        {
            GameInfo gi = GameManager.Instance.m_GameList.AddExistingGame(ret["attributes"].ToString(), true);
            m_CurrentGame = GameManager.Instance.m_GameList.m_GameList.Count - 1;
            UpdateMyInfoOnCurrentGame();
            cs.gameObject.GetComponent<UIPvP>().SetGameInfo(gi);
        }                
        UpdateCurrentGameInfo();        
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
        //Debug.Log(result);
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
        //Debug.Log(result);
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
        //Debug.Log(result);        
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);        
        Question q = new Question(result);
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(q);
        SetCurrentQuestion(q);
        m_PVPState.m_CurrentQuestion++;
    }

    public void DoDoGetPVEQuestionQuestionResult(string result)
    {
        Debug.Log(result);        
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Hide();        
        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);        
        cs.MoveInFromRight();                
        Question q = new Question(result);
        cs.gameObject.GetComponent<UIQuestion>().SetPVEQuestion(q, m_PlayerProfile.m_CurrentPVEStage + 1);
        SetCurrentQuestion(q);
    }

    public PlayerProfile GetPlayerProfile()
    {
        return m_PlayerProfile;
    }

    public void SkipQuestion()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.GetComponent<UIWaiting>().SetContentType(0);
        cs.Show();        
        NetworkManager.Instance.SkipQuestion((Category)m_CurrentQuestion.m_Category);
    }

    public void OnSkipQuestionResult(string result)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.Hide();
        Question q = new Question(result);
        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cs.gameObject.GetComponent<UIQuestion>().SetQuestion(q);
        SetCurrentQuestion(q);
    }

    public void OnStartPVPGame(string friend)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.GetComponent<UIWaiting>().SetContentType(1);
        cs.Show();
        m_IsPVE = false;
        NetworkManager.Instance.DoStartNewGame(friend);
        m_PlayerProfile.SubtractLives();
        if (GetActiveAvatar().m_Tier == TIER.Elder)
        {
            m_PlayerProfile.m_ElderMatch++;
            m_PlayerProfile.Save();
        }
    }

    public void OnStartPVEGame()
    {
        if (m_PlayerProfile.m_CurrentPVEStage == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                m_PlayerProfile.m_PVEBoostUsed[i] = false;
            }
            m_PlayerProfile.Save();
        }

        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.GetComponent<UIWaiting>().SetContentType(0);
        cs.Show();        
        m_IsPVE = true;
        NetworkManager.Instance.DoGetPVEQuestion();
    }

    public void OnCategoryConfirmToPlay(Category cat)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.GetComponent<UIWaiting>().SetContentType(0);
        cs.Show();
        NetworkManager.Instance.DoCategoryConfirmToPlay(cat);
    }

    public void SetCurrentQuestion(Question q)
    {
        m_CurrentQuestion = q;
    }

    public void OnEndPvEGame(bool isWin)
    {
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_ENDGAMERESULT).Show();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_ENDGAMERESULT).GetComponent<UIEndgameResult>().SetResult(isWin);
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME).GetComponent<UINewGame>().RefreshSingle();
        if (isWin)
        {
            AchievementList.Instance.OnSingleWin();
            int reward = 0;
            int bonus = 0;
            if (GameManager.Instance.GetPlayerProfile().m_CurrentPVEStage > 0)
            {
                reward = GameConfig.Instance.GetSingleReward(GameManager.Instance.GetPlayerProfile().m_CurrentPVEStage - 1);
                bonus = Mathf.RoundToInt((float)GameManager.Instance.GetPlayerProfile().m_PayOutBonus / 100 * reward);
            }
            if (GameManager.Instance.GetPlayerProfile().m_PVEState[GameManager.Instance.GetPlayerProfile().m_CurrentPVEStage - 1] == 1)
            {
                reward = 0;
            }
            AddCoin(reward + bonus);            
        }
        else
        {
            m_PlayerProfile.SubtractLives();
        }
        if (m_PlayerProfile.m_CurrentPVEStage > 0 && isWin)
        {
            Debug.Log(m_PlayerProfile.m_CurrentPVEStage - 1);
            m_PlayerProfile.m_PVEState[m_PlayerProfile.m_CurrentPVEStage - 1] = 1;
        }
        m_PlayerProfile.m_CurrentPVEStage = 0;
        m_PlayerProfile.Save();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME).GetComponent<UINewGame>().RefreshSingle();
    }

    public void OnEndPvEGameConfirm()
    {
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_ENDGAMERESULT).Hide();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION).MoveOutToRight();
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
                switch (m_CurrentQuestion.m_Category)
                {
                        //CAT_GEOGRAPHY = 0, CAT_SCIENCE, CAT_ART, CAT_HISTORY, CAT_SPORT, CAT_ENTERTAINMENT, CAT_CROWN};
                    case 0:
                        AchievementList.Instance.OnAction(Achievement_Action.CORRECT_GEOGRAPHY);
                        break;
                    case 1:
                        AchievementList.Instance.OnAction(Achievement_Action.CORRECT_SCIENCE);
                        break;
                    case 2:
                        AchievementList.Instance.OnAction(Achievement_Action.CORRECT_ART);
                        break;
                    case 3:
                        AchievementList.Instance.OnAction(Achievement_Action.CORRECT_HISTORY);
                        break;
                    case 4:
                        AchievementList.Instance.OnAction(Achievement_Action.CORRECT_SPORT);
                        break;
                    case 5:   
                        AchievementList.Instance.OnAction(Achievement_Action.CORRECT_ENTERTAINMENT);
                        break;
                    default:
                        break;
                }
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
        if (m_IsPVE)
        {
            if (m_IsLastAnswerCorrect)
            {
                m_PlayerProfile.m_CurrentPVEStage++;
                m_PlayerProfile.Save();
                SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME).GetComponent<UINewGame>().RefreshSingle();
                CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
                cs.MoveOutToRight();
            }
            else
            {                
                SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME).GetComponent<UINewGame>().RefreshSingle();
                OnEndPvEGame(false);
                m_PlayerProfile.m_CurrentPVEStage = 0;
                m_PlayerProfile.Save();
            }            
        }
        else
        {
            if (m_PVPState.m_Type != PVPStateType.CHALLENGE || m_PVPState.m_CurrentQuestion == 5)
            {
                CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
                cs.MoveOutToRight((int)CanvasID.CANVAS_PVP);
            }


            if (m_PVPState.m_Type == PVPStateType.NORMAL)
            {
                HandleNormalAnswerEnded();
                if (!m_IsLastAnswerCorrect)
                {

                }
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
                EndTurn();
            }
            else
            {
                NetworkManager.Instance.DoTrophyChallangeNextQuestion();
            }

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
        cs.GetComponent<UISelectPieceClaim>().SetButtonText("Play");
        cs.GetComponent<UISelectPieceClaim>().IsAbilityClaim = false;
        cs.MoveInFromRight();

        cs.gameObject.GetComponent<UISelectPieceClaim>().UpdateTrophyState();
    }

    public void OnSelectChallenge()
    {
        if (CanChallenge())
        {
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECHALLENGE);
            cs.GetComponent<UISelectPieceChallenge>().IsAbilityChallenge = false;
            cs.MoveInFromRight();
            cs.gameObject.GetComponent<UISelectPieceChallenge>().SetTrophyState(GetMyTrophyList(), GetOpponentTrophyList());            
        }

        else
        {
            
        }
    }

    public void OnTrophyClaimSelected(int trophy)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.GetComponent<UIWaiting>().SetContentType(0);
        cs.Show();

        NetworkManager.Instance.DoTrophyClaimSelected(trophy);
        SetPVPState(PVPStateType.TROPHY, (Category)trophy, Category.CAT_ART);
    }    

    public void OnTrophyChallengeSelected(int mytrophy, int theirtrophy)
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_WAITING);
        cs.GetComponent<UIWaiting>().SetContentType(0);
        cs.Show();

        NetworkManager.Instance.DoTrophyChallengeSelected();
        SetPVPState(PVPStateType.CHALLENGE, (Category)mytrophy, (Category)theirtrophy);

        m_GameList.m_GameList[m_CurrentGame].m_Challenger = m_PlayerProfile.m_PlayerID;
        m_GameList.m_GameList[m_CurrentGame].m_ChallengeState = 0;

        SetMyBetTrophy(mytrophy);
        SetOpponentBetTrophy(theirtrophy);
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
        SetTrophyAcquired((int)m_Trophy);
        m_GameList.m_GameList[m_CurrentGame].m_SpinProgressA = 0;        
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
        ChecGameCompleted();
        SaveSessionList();
    }

    public void ChecGameCompleted()
    {
        if (GetMyNumberOfTrophy() == 6)
        {
            m_GameList.m_GameList[m_CurrentGame].m_IsCompleted = true;
            SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).GetComponent<UIPvP>().ShowResult();

            int reward = 200;
            int bonus = 0;
            bonus = Mathf.RoundToInt((float)GameManager.Instance.GetPlayerProfile().m_PayOutBonus / 100 * reward);
            AddCoin(reward + bonus);            

            AchievementList.Instance.OnAction(Achievement_Action.WIN_MULTI);
        }
    }

    public int GetGameIndexByID(string sessionid) {
        for (int i = 0; i < m_GameList.m_GameList.Count; i++)
        {
            if (m_GameList.m_GameList[i].m_SessionID == sessionid) {
                return i;
            }
        }
        return -1;
    }

    public void SaveSessionList()
    {
        //m_GameList.Save();
        UpdateCurrentGameInfo();
    }

    //public List<int> GetMyTrophy()
    //{
    //    List<int> l = new List<int>;
    //    for (int i = 0; i < 
    //    return m_GameList.m_GameList[m_CurrentGame].m_PieceA;
    //}

    //public List<int> GetOpponentTrophy()
    //{
    //    return m_GameList.m_GameList[m_CurrentGame].m_PieceB;
    //}

    public void EndTurn()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_Round++;
        }

        if (m_PVPState.m_Type == PVPStateType.CHALLENGE)
        {
            m_GameList.m_GameList[m_CurrentGame].m_ChallengeState++;
        }
        Debug.Log("End turn");
        m_GameList.m_GameList[m_CurrentGame].m_CurrentTurn = 3 - m_GameList.m_GameList[m_CurrentGame].m_CurrentTurn;
        

        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).MoveOutToRight();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME).MoveOutToRight();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_MAIN).MoveInFromLeft();

        SaveSessionList();
    }

    private void SimulateOtherPlayers()
    {
        for (int i = 0; i < m_GameList.m_GameList.Count; i++)
        {
            GameInfo gi = m_GameList.m_GameList[i];
            Debug.Log(gi.m_CurrentTurn);
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
                    Debug.Log("Check finish challenge");
                    //Finish challenge
                    if (m_GameList.m_GameList[m_CurrentGame].m_ChallengeState == 1)
                    {
                        m_GameList.m_GameList[m_CurrentGame].m_ChallengeState++;
                        m_GameList.m_GameList[m_CurrentGame].m_ChallengeScoreB = 0;

                        m_GameList.m_GameList[m_CurrentGame].m_PieceA[m_GameList.m_GameList[m_CurrentGame].m_BetTrophyB] = 1;
                        m_GameList.m_GameList[m_CurrentGame].m_PieceB[m_GameList.m_GameList[m_CurrentGame].m_BetTrophyB] = 0;
                    }
                    else
                    {
                        //AddOneTrophy
                        Debug.Log("Random Add Trophy");
                        if (Random.Range(0, 2) == 0)
                        {
                            Debug.Log("Add Trophy");
                            for (int k = 0; k < gi.m_PieceB.Count; k++)
                            {
                                if (gi.m_PieceB[k] == 0)
                                {
                                    gi.m_PieceB[k] = 1;
                                    break;
                                }
                            }
                        }
                    }

                    gi.m_LastAbility = (int)Ability.ABILITY_CLAIM;
                    gi.m_PlayerUseAbility = gi.m_PlayerBID;
                    gi.m_TrophyRemoved = -1;
                    for (int k = 0; k < gi.m_PieceB.Count; k++)
                    {
                        if (gi.m_PieceB[k] == 0)
                        {
                            gi.m_PieceB[k] = 1;
                            gi.m_TrophyAcquired = k;
                            break;
                        }
                    }
                    gi.m_AbilityShowed = false;
                }
            }            
        }
        
    }

    public bool CanChallenge()
    {
        List<int> myTrophy = GetMyTrophyList();
        List<int> theirTrophy = GetOpponentTrophyList();
        int my = -1;
        int their = -1;
        bool flag = false;
        for (int i = 0; i < myTrophy.Count; i++)
        {
            
            if (myTrophy[i] == 0)
            {                
            }
            else
            {                
                if (!flag)
                {                    
                    flag = true;
                    my = i;
                }
            }
        }
        flag = false;
        for (int i = 0; i < theirTrophy.Count; i++)
        {
            
            if (theirTrophy[i] == 0 || myTrophy[i] == 1)
            {
                
            }
            else
            {
                
                if (!flag)
                {
                    
                    flag = true;
                    their = i;
                }
            }
        }
        if (their != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

#region SessionInfoManupilate
    public void SetMyBetTrophy(int trophy)
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
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
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
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
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_ChallengeScoreA++;
        } else 
        {
            m_GameList.m_GameList[m_CurrentGame].m_ChallengeScoreB++;
        }
    }

    public void AddProgress()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
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
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
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
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
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
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
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

    //public List<int> GetCurrentTrophyState()
    //{
    //    //Debug.Log(m_GameList.m_GameList[m_CurrentGame].m_PlayerAID + "  " + m_PlayerProfile.m_PlayerID);
    //    if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
    //    {
    //        return m_GameList.m_GameList[m_CurrentGame].m_PieceA;
    //    }
    //    else
    //    {
    //        return m_GameList.m_GameList[m_CurrentGame].m_PieceB;
    //    }
    //}

    public List<int> GetMyTrophyList()
    {        
        List<int> l = new List<int>();
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            for (int i = 0; i < 6; i++)
            {
                l.Add(m_GameList.m_GameList[m_CurrentGame].m_PieceA[i]);
            }            
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                l.Add(m_GameList.m_GameList[m_CurrentGame].m_PieceB[i]);
            }
        }
        return l;
    }

    public List<int> GetOpponentTrophyList()
    {
        List<int> l = new List<int>();
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            for (int i = 0; i < 6; i++)
            {
                l.Add(m_GameList.m_GameList[m_CurrentGame].m_PieceB[i]);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                l.Add(m_GameList.m_GameList[m_CurrentGame].m_PieceA[i]);
            }
        }
        return l;
    }

    public string GetOpponentName()
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            return m_GameList.m_GameList[m_CurrentGame].m_PlayerBName;
        }
        else
        {
            return m_GameList.m_GameList[m_CurrentGame].m_PlayerAName;
        }
    }

    public void SetTrophyAcquired(int trophy)
    {
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            m_GameList.m_GameList[m_CurrentGame].m_PieceA[trophy] = 1;            
        }
        else
        {
            m_GameList.m_GameList[m_CurrentGame].m_PieceB[trophy] = 1;
        }
    }

    public int GetMyNumberOfTrophy()
    {
        int count = 0;
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            for (int i = 0; i < m_GameList.m_GameList[m_CurrentGame].m_PieceA.Count; i++)
            {
                if (m_GameList.m_GameList[m_CurrentGame].m_PieceA[i] == 1)
                {
                    count++;
                }
            }
            return count;
        }
        else
        {
            for (int i = 0; i < m_GameList.m_GameList[m_CurrentGame].m_PieceB.Count; i++)
            {
                if (m_GameList.m_GameList[m_CurrentGame].m_PieceB[i] == 1)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public void SetMeUseAbility()
    {
        GameInfo gi = GetCurrentGameInfo();
        gi.m_PlayerUseAbility = m_PlayerProfile.m_PlayerID;
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            gi.m_PlayerAIDAbilityUsed++;
        }
        else
        {
            gi.m_PlayerBIDAbilityUsed++;
        }
        SaveSessionList();
    }

    public void SetAbilityID(int id)
    {
        GameInfo gi = GetCurrentGameInfo();
        gi.m_LastAbility = id;
        SaveSessionList();
    }

    public void SetTrophyAcquried(int trophy)
    {
        GameInfo gi = GetCurrentGameInfo();
        gi.m_TrophyAcquired = trophy;
        SaveSessionList();
    }

    public void SetTrophyRemoved(int trophy)
    {
        GameInfo gi = GetCurrentGameInfo();
        gi.m_TrophyRemoved = trophy;
        SaveSessionList();
    }

#endregion

    public bool IsItemOwned(int type, int id)
    {
        if (id == 0)
        {
            return true;
        }
        for (int i = 0; i < m_PlayerProfile.m_ItemCat.Count; i++)
        {
            if (m_PlayerProfile.m_ItemCat[i] == type && m_PlayerProfile.m_ItemID[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public Avatar GetMyActiveAvatar()
    {
        return m_PlayerProfile.m_AvatarList[m_PlayerProfile.m_ActiveAvatar];
    }

    public Avatar GetMyAvatarInCurrentGame()
    {
        Avatar ava = Avatar.CreateDefaultAvatar();
        ava.m_Tier = (TIER)GetCurrentGameInfo().m_PlayerAIDTier;
        ava.m_Jobs = (CLASS)GetCurrentGameInfo().m_PlayerAIDJobs;
        for (int i = 0; i < 8; i++)
        {
            ava.m_ItemList[i] = GetCurrentGameInfo().m_PlayerAIDItems[i];
        }
        return ava;
    }

    public int GetAvatarCount()
    {
        return m_PlayerProfile.m_AvatarList.Count;
    }

    public int GetActiveAvatarID()
    {
        return m_PlayerProfile.m_ActiveAvatar;
    }

    public void SetActiveAvatar(int id)
    {
        m_PlayerProfile.m_ActiveAvatar = id;
        m_PlayerProfile.Save();
    }

    public void UpgradeTier(CLASS _job = CLASS.None)
    {
        TIER currentTier = m_PlayerProfile.m_AvatarList[m_PlayerProfile.m_ActiveAvatar].m_Tier;
        if (currentTier != TIER.Elder)
        {
            m_PlayerProfile.m_AvatarList[m_PlayerProfile.m_ActiveAvatar].m_Tier++;
            TIER tier = m_PlayerProfile.m_AvatarList[m_PlayerProfile.m_ActiveAvatar].m_Tier;
            if (tier == TIER.Child || tier == TIER.Teenager || tier == TIER.Young_Adult || tier == TIER.Adult_2 || tier == TIER.Adult_4)
            {
                m_PlayerProfile.m_PayOutBonus++;
            }
            if (tier == TIER.Adult_1)
            {
                m_PlayerProfile.m_AvatarList[m_PlayerProfile.m_ActiveAvatar].m_Jobs = _job;
            }
            if (tier == TIER.Adult_1 || tier == TIER.Adult_2 || tier == TIER.Adult_3 || tier == TIER.Adult_4 || tier == TIER.Adult_5)
            {
                CLASS job = m_PlayerProfile.m_AvatarList[m_PlayerProfile.m_ActiveAvatar].m_Jobs;
                if (job == CLASS.Medicial) {
                    AchievementList.Instance.OnAction(Achievement_Action.UPGRADE_DOCTER);
                } 
                else if (job == CLASS.Musician) {
                    AchievementList.Instance.OnAction(Achievement_Action.UPGRADE_MUSICIAN);
                }
                else if (job == CLASS.Athlete)
                {
                    AchievementList.Instance.OnAction(Achievement_Action.UPGRADE_AHTHLETE);
                }
                else if (job == CLASS.Enterpreneur)
                {
                    AchievementList.Instance.OnAction(Achievement_Action.UPGRADE_BUSINESS);
                }
                else if (job == CLASS.Scientist)
                {
                    AchievementList.Instance.OnAction(Achievement_Action.UPGRADE_SCIENTIST);
                }
                else if (job == CLASS.Warrior)
                {
                    AchievementList.Instance.OnAction(Achievement_Action.UPGRADE_WARRIOR);
                }
            }
        }
        else
        {
            m_PlayerProfile.AddNewAvatar();
            m_PlayerProfile.m_PayOutBonus++;
        }
        m_PlayerProfile.Save();

        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_MAIN).GetComponent<UIMain>().RefreshInfo();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_MAIN).GetComponent<UIMain>().Refresh();
    }

    public Avatar GetActiveAvatar()
    {
        return m_PlayerProfile.m_AvatarList[m_PlayerProfile.m_ActiveAvatar];
    }

    public void AddDiamond(int amount)
    {
        m_PlayerProfile.m_Diamond += amount;
        m_PlayerProfile.Save();
    }

    public void ExchangeDiamond(int amount)
    {
        m_PlayerProfile.m_Diamond -= amount;
        AddCoin(Mathf.RoundToInt(amount * GameConfig.Instance.GetExchangeRate()));        
    }

    public void NotEnoughDiamond()
    {
        //SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_OUTLIVES).Show();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_STORE).Show();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_STORE).GetComponent<UIStore>().ShowShopTab();
    }

    public void NotEnoughCoin()
    {
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_EXCHANGE).Show();
    }

    public void OnUseDiamond(int amount)
    {
        m_PlayerProfile.m_Diamond -= amount;
        m_PlayerProfile.Save();
    }

    public void OnUseCoin(int amount)
    {
        AddCoin(-amount);        
    }

    public void OnUseAbility()
    {
        Avatar ava = GameManager.Instance.GetActiveAvatar();

        switch (ava.m_Jobs)
        {
            case CLASS.Medicial:
                {
                    DoFreeClaim();                                        
                    break;
                }
            case CLASS.Athlete:
                {
                    if (CanChallenge())
                    {
                        DoFreeChallenge();
                    }
                    else
                    {
                        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                        cs.GetComponent<UIPopup>().Show(" This ability autofills your puzzle gauge for a free attempt at challenging for a puzzle piece. Avatars can only use this ability once his/her opponent has at least one puzzle piece .  ", 0, null, null, (int)CanvasID.CANVAS_PVP);
                    }
                    break;
                }
            case CLASS.Enterpreneur:
                {
                    if (CanSwitch())
                    {
                        DoSwitchPuzzle();
                    }
                    else
                    {
                        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                        cs.GetComponent<UIPopup>().Show("This ability swaps your puzzle piece set with your opponent’s set. Avatars can only use this ability once an opponent has at least one puzzle piece ", 0, null, null, (int)CanvasID.CANVAS_PVP);
                    }
                    break;
                }
            case CLASS.Musician:
                {
                    if (CanCopy())
                    {
                        DoCopy();
                    }
                    else
                    {
                        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                        cs.GetComponent<UIPopup>().Show("This ability allows you to use the same ability as your opponent. Avatars can only use this ability once opponent has used her/her avatar ability. ", 0, null, null, (int)CanvasID.CANVAS_PVP);
                    }
                    break;
                }
            case CLASS.Scientist:
                {
                    Debug.Log("Undo attempt");
                    if (CanUndo())
                    {
                        Debug.Log("DoUnDo");
                        DoUndo();
                    } else {
                        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                        cs.GetComponent<UIPopup>().Show("This ability undoes your opponent’s last ability move. Avatars can only use this ability once opponent has used her/her avatar ability.  ", 0, null, null, (int)CanvasID.CANVAS_PVP);
                    }
                    break;
                }
            case CLASS.Warrior:
                {
                    if (CanRemove())
                    {
                        DoRemovePuzzle();
                    }
                    else
                    {
                        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                        cs.GetComponent<UIPopup>().Show("This ability removes one random puzzle piece from your opponent’s set. Avatars can only use this ability once an opponent has at least one puzzle piece.", 0, null, null, (int)CanvasID.CANVAS_PVP);
                    }
                    break;
                }
            default:
                break;
        }

        //FREE CLAIM
        //ClearProgress();
        //CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECLAIM);
        //cs.GetComponent<UISelectPieceClaim>().SetButtonText("Claim");
        //cs.GetComponent<UISelectPieceClaim>().IsAbilityClaim = true;
        //cs.gameObject.GetComponent<UISelectPieceClaim>().UpdateTrophyState();
        //cs.MoveInFromRight();
        
        //FREECHALLENGE
        //if (CanChallenge())
        //{
        //    CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECHALLENGE);
        //    cs.GetComponent<UISelectPieceChallenge>().IsAbilityChallenge = true;
        //    cs.gameObject.GetComponent<UISelectPieceChallenge>().SetTrophyState(GetMyTrophy(), GetOpponentTrophy());
        //    cs.MoveInFromRight();
        //}
        //else
        //{
        //    CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
        //    cs.GetComponent<UIPopup>().Show("This ability undoes your opponent’s last ability move. Avatars can only use this ability once opponent has used her/her avatar ability.  ", 0, null, null, (int)CanvasID.CANVAS_PVP);                                       
        //}

        //SWITCH PUZZLE
        //if (CanSwitch())
        //{
        //    DoSwitchPuzzle();
        //}
        //else
        //{
        //    CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
        //    cs.GetComponent<UIPopup>().Show("This ability swaps your puzzle piece set with your opponent’s set. Avatars can only use this ability once an opponent has at least one puzzle piece ", 0, null, null, (int)canvasid.canvas_pvp);
        //}

        //REMOVE PUZZLE
        //if (CanRemove())
        //{
        //    DoRemovePuzzle();
        //}
        //else
        //{
        //    CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
        //    cs.GetComponent<UIPopup>().Show("This ability swaps your puzzle piece set with your opponent’s set. Avatars can only use this ability once an opponent has at least one puzzle piece ", 0, null, null, (int)CanvasID.CANVAS_PVP);
        //}
    }

    public void DoSwitchPuzzle()
    {
        for (int i = 0; i < m_GameList.m_GameList[m_CurrentGame].m_PieceA.Count; i++)
        {
            int tg = m_GameList.m_GameList[m_CurrentGame].m_PieceA[i];
            m_GameList.m_GameList[m_CurrentGame].m_PieceA[i] = m_GameList.m_GameList[m_CurrentGame].m_PieceB[i];
            m_GameList.m_GameList[m_CurrentGame].m_PieceB[i] = tg;
        }
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
        cs.gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
        SetMeUseAbility();
        SetAbilityID((int)Ability.ABILITY_SWITCH);
        SetTrophyRemoved(-1);
        SetTrophyAcquired(-1);
        SaveSessionList();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
    }

    public bool CanSwitch()
    {
        List<int> myTrophy = GetMyTrophyList();
        List<int> theirTrophy = GetOpponentTrophyList();
        int my = -1;
        int their = -1;
        bool flag = false;
        for (int i = 0; i < myTrophy.Count; i++)
        {

            if (myTrophy[i] == 1 || theirTrophy[i] == 1)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanRemove()
    {
        for (int i = 0; i < GetOpponentTrophyList().Count; i++)
        {
            if (GetOpponentTrophyList()[i] == 1)
            {
                return true;
            }
        }
        return false;
    }

    public void DoRemovePuzzle()
    {
        List<int> li = GetOpponentTrophyList();
        List<int> l = new List<int>();
        for (int i = 0; i < li.Count; i++)
        {
            if (li[i] == 1)
            {
                l.Add(i);
            }
        }
        int pick = l[Random.Range(0, l.Count)];
        OnAbilityRemovedSelected(pick);
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
    }

    public void OnTrophyChallengeSelectedAbility(int mytrophy, int theirtrophy)
    {
        if (GetMyTrophyList()[theirtrophy] == 0)
        {
            GetMyTrophyList()[theirtrophy] = 1;
            SetTrophyAcquried(theirtrophy);
        }
        else
        {
            SetTrophyAcquried(-1);
        }
        GetOpponentTrophyList()[theirtrophy] = 0;
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
        cs.gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
        SetMeUseAbility();
        SetAbilityID((int)Ability.ABILITY_CHALLENGE);
        SetTrophyRemoved(theirtrophy);
        SaveSessionList();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
    }

    public void OnAbilityRemovedSelected(int trophy)
    {
        GetOpponentTrophyList()[trophy] = 0;
        
        SetMeUseAbility();
        SetAbilityID((int)Ability.ABILITY_REMOVE);
        SetTrophyAcquired(-1);
        SetTrophyRemoved(trophy);
        SaveSessionList();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
    }

    public void OnAbilityClaimSelected(int trophy)
    {
        ClearProgress();
        TrophyAcquired((Category)trophy);
        GameInfo gi = GetCurrentGameInfo();
        SetMeUseAbility();
        SetAbilityID((int)Ability.ABILITY_CLAIM);
        SetTrophyAcquired(trophy);
        SetTrophyRemoved(-1);
        SaveSessionList();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
    }

    public bool CanCopy()
    {
        if (GetCurrentGameInfo().m_LastAbility != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanUndo()
    {
        if (GetCurrentGameInfo().m_LastAbility != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DoCopy()
    {
        Ability ability = (Ability)GetCurrentGameInfo().m_LastAbility;
        switch (ability)
        {
            case Ability.ABILITY_CLAIM:
                {
                    DoFreeClaim();
                    break;
                }
            case Ability.ABILITY_CHALLENGE:
                {
                    DoFreeChallenge();
                    break;
                }
            case Ability.ABILITY_REMOVE:
                {
                    DoRemovePuzzle();
                    break;
                }
            case Ability.ABILITY_SWITCH:
                {
                    DoSwitchPuzzle();
                    break;
                }
            default:
                break;
        }
    }

    public void DoUndo()
    {
        GameInfo gi = GetCurrentGameInfo();
        List<int> oppTrophy = GetOpponentTrophyList();
        List<int> myTrophy = GetMyTrophyList();

        if (gi.m_TrophyAcquired != -1)
        {
            oppTrophy[gi.m_TrophyAcquired] = 0;
        }
        if (gi.m_TrophyRemoved != -1)
        {
            myTrophy[gi.m_TrophyAcquired] = 1;
        }
        SaveSessionList();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP).gameObject.GetComponent<UIPvP>().SetGameInfo(GetCurrentGameInfo());
    }

    public void DoFreeClaim()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECLAIM);
        cs.GetComponent<UISelectPieceClaim>().SetButtonText("Claim");
        cs.GetComponent<UISelectPieceClaim>().IsAbilityClaim = true;
        cs.gameObject.GetComponent<UISelectPieceClaim>().UpdateTrophyState();
        cs.MoveInFromRight();
    }

    public void DoFreeChallenge()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECHALLENGE);
        cs.GetComponent<UISelectPieceChallenge>().IsAbilityChallenge = true;
        cs.gameObject.GetComponent<UISelectPieceChallenge>().SetTrophyState(GetMyTrophyList(), GetOpponentTrophyList());
        cs.MoveInFromRight();
    }

    public int GetNumberOfAbilityCanUse() {
        int tier = (int)GetCurrentGameInfo().m_PlayerAIDTier;
        if (tier >= 9)
        {
            return 3;
        }
        else if (tier >= 7)
        {
            return 2;
        }
        else if (tier >= 5)
        {
            return 1;
        }
        return 0;
    }

    public int GetAbilityLeft()
    {
        GameInfo gi = GetCurrentGameInfo();
        if (m_GameList.m_GameList[m_CurrentGame].m_PlayerAID == m_PlayerProfile.m_PlayerID)
        {
            return GetNumberOfAbilityCanUse() - gi.m_PlayerAIDAbilityUsed;
        }
        else
        {
            return GetNumberOfAbilityCanUse() - gi.m_PlayerBIDAbilityUsed;
        }
    }

    public void ShowHelpNewGame()
    {
        Debug.Log("Show help new game");
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_HELP).GetComponent<UIHelp>().Show(0);
        m_PlayerProfile.m_FirstTimeExperience[0] = true;
        m_PlayerProfile.Save();
    }

    public void ShowHelpBoost()
    {
        Debug.Log("Show help boost");
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_HELP).GetComponent<UIHelp>().Show(1);
        m_PlayerProfile.m_FirstTimeExperience[1] = true;
        m_PlayerProfile.Save();
    }

    public Ability GetActiveAbility()
    {
        Ability ret = Ability.ABILITY_CLAIM;
        Avatar ava = GetActiveAvatar();
        switch (ava.m_Jobs)
        {
            case CLASS.Medicial:
                {
                    ret = Ability.ABILITY_CLAIM;
                    break;
                }
            case CLASS.Athlete:
                {
                    ret = Ability.ABILITY_CLAIM;
                    break;
                }
            case CLASS.Enterpreneur:
                {
                    ret = Ability.ABILITY_SWITCH;
                    break;
                }
            case CLASS.Musician:
                {
                    ret = Ability.ABILITY_COPY;
                    break;
                }
            case CLASS.Scientist:
                {
                    ret = Ability.ABILITY_UNDO;
                    break;
                }
            case CLASS.Warrior:
                {
                    ret = Ability.ABILITY_REMOVE;
                    break;
                }
            default:
                break;

        }
        return ret;
    }

    public void ShowHelpUpgrade(bool value)
    {
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_MAIN).GetComponent<UIMain>().ShowAlertImage(value);
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER).GetComponent<UISettingSlider>().ShowAlertImage(value);
    }

    public void ShowHelpAdultUpgrade(bool value)
    {
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_MAIN).GetComponent<UIMain>().ShowAlertImage(value);
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER).GetComponent<UISettingSlider>().ShowAlertImage(value);
    }

    public void AddCoin(int amount)
    {
        m_PlayerProfile.m_Coin += amount;
        m_PlayerProfile.Save();
        CheckFirstUpgrade();
        CheckFirstAdultUpgrade();
        CheckFirstReborn();
    }

    public void CheckFirstUpgrade()
    {
        if (GetPlayerProfile().m_FirstTimeExperience[2] == false && GetPlayerProfile().m_Coin >= 0)
        {
            GameManager.Instance.ShowHelpUpgrade(true);
        }
    }

    public void CheckFirstAdultUpgrade()
    {
        if (GetPlayerProfile().m_FirstTimeExperience[3] == false && GetPlayerProfile().m_Coin >= 0 && GetActiveAvatar().m_Tier == TIER.Young_Adult)
        {
            GameManager.Instance.ShowHelpAdultUpgrade(true);
        }
    }

    public void CheckFirstReborn()
    {
        if (GetPlayerProfile().m_FirstTimeExperience[5] == false && GetPlayerProfile().m_Coin >= 0 && GetActiveAvatar().m_Tier == TIER.Elder && GetPlayerProfile().m_ElderMatch > 0)
        {
            GameManager.Instance.ShowHelpUpgrade(true);
        }
    }

    public void OnFirstTimeUserExperienceComplete(int id)
    {
        m_PlayerProfile.m_FirstTimeExperience[id] = true;
        m_PlayerProfile.Save();
    }

    public void OnRegisterCallback(string result)
    {
        Debug.Log(result);

        var ret = new JSONClass();
        ret["result"].AsBool = true;
        ret["name"] = m_RegisterName;
        ret["sex"].AsInt = m_Sex;

        var r = JSONNode.Parse(result);
        ret["uid"] = r["uid"];
        
        GameManager.Instance.OnRegisterResult(ret.ToString());
    }

    public void SetRegisterInfo(string name, int sex)
    {
        m_RegisterName = name;
        m_Sex = sex;
    }

    public void OnUpdateSessionInfoResult(string result)
    {
        Debug.Log(result);
    }

    public void OnGetAllSessionInfoResult(string result)
    {        
        var ret = JSONNode.Parse(result);
        for (int i = 0; i < ret["game_sessions"].Count; i++)
        {            
            if (ret["game_sessions"][i]["attributes"].ToString().Length > 15) {                
                GameManager.Instance.m_GameList.AddExistingGame(ret["game_sessions"][i]["attributes"].ToString());            
            }
        }
        UpdateCurrentGameInfo();
    }

    public void UpdateCurrentGameInfo()
    {
        string s = GetCurrentGameInfo().ToJsonString();
        s = s.Replace("[", "\"[");
        s = s.Replace("]", "]\"");
        
        NetworkManager.Instance.DoUpdateSessionInfo(GetCurrentGameInfo().m_SessionID, s);
    }

    public void UpdateMyInfoOnCurrentGame()
    {
        GameInfo gi = GetCurrentGameInfo();
        Debug.Log(GetPlayerProfile().m_PlayerID + " " + gi.m_PlayerAID + " " + gi.m_PlayerBID);
        if (GetPlayerProfile().m_PlayerID == gi.m_PlayerAID)
        {
            Debug.Log("Im player A");
            gi.m_PlayerAIDSex = GetPlayerProfile().m_Sex;
            gi.m_PlayerAIDTier = (int)GetPlayerProfile().GetActiveAvatar().m_Tier;
            gi.m_PlayerAIDJobs = (int)GetPlayerProfile().GetActiveAvatar().m_Jobs;
            gi.m_PlayerAName = GetPlayerProfile().m_PlayerName;
        }
        else if (GetPlayerProfile().m_PlayerID == gi.m_PlayerBID)
        {
            Debug.Log("Im player B");
            gi.m_PlayerBIDSex = GetPlayerProfile().m_Sex;
            gi.m_PlayerBIDTier = (int)GetPlayerProfile().GetActiveAvatar().m_Tier;
            gi.m_PlayerBIDJobs = (int)GetPlayerProfile().GetActiveAvatar().m_Jobs;
            gi.m_PlayerBName = GetPlayerProfile().m_PlayerName;
        }
    }
}

