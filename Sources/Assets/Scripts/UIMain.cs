using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIMain : MonoBehaviour {

    public Text m_TextName;
    public Text m_TextLives;
    public Text m_TextCoin;
    public Text m_TextDiamond;
    public Text m_TextEXP;
    public GameObject m_GameInfoPrefab;

    public List<GameObject> m_GameInfoList;

    public RectTransform m_MainPanel;

	// Use this for initialization
	void Start () {
        RefreshInfo();
        ReloadYourTurnList();
        m_GameInfoList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnSetting()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveInFromLeftFar();
    }

    public void OnAddDiamond()
    {
    }

    public void OnNewGame()
    {
        Debug.Log("On New Game");
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME);
        cs.MoveInFromRight();

        cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_MAIN);
        cs.MoveOutToLeft();
    }

    public void OnYourTurn()
    {
        ReloadYourTurnList();
    }

    public void OnTheirTurn()
    {
        ReloadTheirTurnList();
    }

    public void OnPastGame()
    {
        ReloadPassGameList();
    }

    public void RefreshInfo()
    {
        Debug.Log(GameManager.Instance.GetPlayerProfile().m_PlayerName);
        m_TextName.text = GameManager.Instance.GetPlayerProfile().m_PlayerName;
        m_TextLives.text = GameManager.Instance.GetPlayerProfile().m_Lives.ToString();
        m_TextCoin.text = GameManager.Instance.GetPlayerProfile().m_Coin.ToString();
        m_TextDiamond.text = GameManager.Instance.GetPlayerProfile().m_Diamond.ToString();
        m_TextEXP.text = GameManager.Instance.GetPlayerProfile().m_Level.ToString() + " " + GameManager.Instance.GetPlayerProfile().m_LevelEXP.ToString() + "/" + GameManager.Instance.GetPlayerProfile().m_ExpToLevelUP.ToString();
    }

    public void DeleteGameList()
    {
        for (int i = 0; i < m_GameInfoList.Count; i++)
        {
            GameObject.Destroy(m_GameInfoList[i]);
        }
    }

    public void ReloadYourTurnList()
    {
        DeleteGameList();
        int num = 10;
        for (int i = 0; i < num; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_GameInfoPrefab);
            go.transform.parent = m_MainPanel.gameObject.transform;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, -1200 - i * 200, 0);
            rt.localScale = new Vector3(1, 1, 1);
            m_GameInfoList.Add(go);
        }
        m_MainPanel.sizeDelta = new Vector2(1440, 1200 + num * 200);
    }

    public void ReloadTheirTurnList()
    {
        DeleteGameList();
        int num = 20;
        for (int i = 0; i < num; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_GameInfoPrefab);
            go.transform.parent = m_MainPanel.gameObject.transform;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, -1200 - i * 200, 0);
            rt.localScale = new Vector3(1, 1, 1);
            m_GameInfoList.Add(go);
        }
        m_MainPanel.sizeDelta = new Vector2(1440, 1200 + num * 200);
    }

    public void ReloadPassGameList()
    {
        DeleteGameList();
        int num = 1;
        for (int i = 0; i < num; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_GameInfoPrefab);
            go.transform.parent = m_MainPanel.gameObject.transform;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, -1200 - i * 200, 0);
            rt.localScale = new Vector3(1, 1, 1);
            m_GameInfoList.Add(go);
        }
        m_MainPanel.sizeDelta = new Vector2(1440, 1200 + num * 200);
    }
}
