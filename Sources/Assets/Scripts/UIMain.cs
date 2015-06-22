using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMain : MonoBehaviour {

    public Text m_TextName;
    public Text m_TextLives;
    public Text m_TextLivesTimer;
    public Text m_TextCoin;
    public Text m_TextDiamond;
    public Text m_TextEXP;
    public GameObject m_GameInfoPrefab;

    public List<GameObject> m_GameInfoList;

    public RectTransform m_MainPanel;

    private int m_CurrentTab;

    public Image m_ExpImage;

    public AvatarScript m_Avatar;

    public StoreTab m_StoreTab;

	// Use this for initialization
	void Start () {
       
        m_GameInfoList = new List<GameObject>();
        
        m_CurrentTab = 1;
  
        RefreshInfo();
        Refresh();


	}
	
	// Update is called once per frame
	void Update () {
        UpdateLivesTimer();
	}

    public void OnSetting()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveInFromLeftFar();
    }

    public void OnAddDiamond()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_STORE);
        cv.MoveInFromRight();
        m_StoreTab.OnCurrency();
    }

    public void OnNewGame()
    {
        //Debug.Log("On New Game");
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME);
        cs.MoveInFromRight();

        //cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_MAIN);
        //cs.MoveOutToLeft();
    }

    public void OnYourTurn()
    {
        ReloadYourTurnList();
        m_CurrentTab = 1;
    }

    public void OnTheirTurn()
    {
        ReloadTheirTurnList();
        m_CurrentTab = 2;
    }

    public void OnPastGame()
    {
        ReloadPassGameList();
        m_CurrentTab = 3;
    }

    public void RefreshInfo()
    {
        //Debug.Log(GameManager.Instance.GetPlayerProfile().m_PlayerName);
        //m_TextName.text = GameManager.Instance.GetPlayerProfile().m_PlayerName;
        m_TextLives.text = GameManager.Instance.GetPlayerProfile().m_Lives.ToString();
        m_TextCoin.text = GameManager.Instance.GetPlayerProfile().m_Coin.ToString();
        m_TextDiamond.text = GameManager.Instance.GetPlayerProfile().m_Diamond.ToString();
        int level = GameManager.Instance.GetPlayerProfile().GetCurrentLevel();
        m_TextEXP.text = "Lv" + level + " " + GameManager.Instance.GetPlayerProfile().GetLevelEXP() + "/" + GameConfig.Instance.GetLevelEXP(level - 1) + " EXP";
        m_ExpImage.GetComponent<RectTransform>().localScale = new Vector3((float)GameManager.Instance.GetPlayerProfile().GetLevelEXP() / GameConfig.Instance.GetLevelEXP(level - 1), 1, 1);

        if (GameManager.Instance.GetPlayerProfile().m_Lives < 5)
        {
            m_TextLivesTimer.gameObject.SetActive(true);
            UpdateLivesTimer();
        }
        else
        {
            m_TextLivesTimer.gameObject.SetActive(false);
        }

        
    }

    public void DeleteGameList()
    {
        for (int i = 0; i < m_GameInfoList.Count; i++)
        {
            GameObject.Destroy(m_GameInfoList[i]);
        }
        m_GameInfoList.Clear();
    }

    public void ReloadYourTurnList()
    {
        DeleteGameList();
        int num = 0;
        GameList gl = GameManager.Instance.m_GameList;
        for (int i = 0; i < gl.m_GameList.Count; i++)
        {
            if (gl.m_GameList[i].m_CurrentTurn == 1 && !gl.m_GameList[i].m_IsCompleted)
            {
                GameObject go = (GameObject)GameObject.Instantiate(m_GameInfoPrefab);
                go.transform.SetParent(m_MainPanel.gameObject.transform);
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(0, -1200 - num * 200, 0);
                rt.localScale = new Vector3(1, 1, 1);
                m_GameInfoList.Add(go);
                go.GetComponent<GameInfoScript>().SetGameInfo(gl.m_GameList[i]);
                num++;
            }
        }
        m_MainPanel.sizeDelta = new Vector2(1440, 1200 + num * 200);
    }

    public void ReloadTheirTurnList()
    {        
        DeleteGameList();
        int num = 0;
        GameList gl = GameManager.Instance.m_GameList;
        //Debug.Log("reload");
        for (int i = 0; i < gl.m_GameList.Count; i++)
        {
            if (gl.m_GameList[i].m_CurrentTurn == 2 && !gl.m_GameList[i].m_IsCompleted)
            {
                
                GameObject go = (GameObject)GameObject.Instantiate(m_GameInfoPrefab);
                go.transform.parent = m_MainPanel.gameObject.transform;
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(0, -1200 - num * 200, 0);
                rt.localScale = new Vector3(1, 1, 1);
                m_GameInfoList.Add(go);
                go.GetComponent<GameInfoScript>().SetGameInfo(gl.m_GameList[i]);
                num++;
            }
        }
        m_MainPanel.sizeDelta = new Vector2(1440, 1200 + num * 200);
    }

    public void ReloadPassGameList()
    {
        DeleteGameList();
        int num = 0;
        GameList gl = GameManager.Instance.m_GameList;
        for (int i = 0; i < gl.m_GameList.Count; i++)
        {
            if (gl.m_GameList[i].m_IsCompleted)
            {
                
                GameObject go = (GameObject)GameObject.Instantiate(m_GameInfoPrefab);
                go.transform.parent = m_MainPanel.gameObject.transform;
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(0, -1200 - num * 200, 0);
                rt.localScale = new Vector3(1, 1, 1);
                m_GameInfoList.Add(go);
                go.GetComponent<GameInfoScript>().SetGameInfo(gl.m_GameList[i]);
                num++;
            }
        }
        m_MainPanel.sizeDelta = new Vector2(1440, 1200 + num * 200);
    }

    public void Refresh()
    {
        switch (m_CurrentTab)
        {
            case 1:
                ReloadYourTurnList();
                break;
            case 2:
                ReloadTheirTurnList();
                break;
            case 3:
                ReloadYourTurnList();
                break;
            default:
                break;
        }

        PlayerProfile pl = GameManager.Instance.GetPlayerProfile();        
        m_Avatar.SetInfo(GameManager.Instance.GetMyActiveAvatar());

        RefreshInfo();
    }

    public void UpdateLivesTimer()
    {
        DateTime dtnow = DateTime.Now;
        TimeSpan time = dtnow - GameManager.Instance.GetPlayerProfile().m_LastTimeAddLive;
        int sec = (int)time.TotalSeconds;
        
        if (sec > 1 * 60)
        {
            GameManager.Instance.GetPlayerProfile().m_LastTimeAddLive = DateTime.Now;
            GameManager.Instance.GetPlayerProfile().AddLives();
            sec = 0;
            m_TextLives.text = GameManager.Instance.GetPlayerProfile().m_Lives.ToString();
        }

        m_TextLives.text = GameManager.Instance.GetPlayerProfile().m_Lives.ToString();
        sec = 1 * 60 - sec;
        if (GameManager.Instance.GetPlayerProfile().m_Lives < 5)
        {
            m_TextLivesTimer.gameObject.SetActive(true);
            m_TextLivesTimer.text = ((int)(sec / 60)).ToString() + ":" + ((int)(sec % 60)).ToString();
        } else {
            m_TextLivesTimer.gameObject.SetActive(false);
        }
    }

    public void OnAvatarPress()
    {
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PROFILE).MoveInFromRight();
    }
}

