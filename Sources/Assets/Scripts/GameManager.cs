using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class GameManager : MonoBehaviour {

    public RectTransform m_MainPanel;
    public GameObject m_GameInfoPrefab;

    public List<GameObject> m_GameInfoList;

    private PlayerProfile m_PlayerProfile;

    private static GameManager m_sInstance = null;

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

    public void OnContinueGame(string gameid)
    {
        Debug.Log("On Continue Game");
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
        if (System.IO.File.Exists("TriviaPlayerProfile.xml"))
        {
            m_PlayerProfile = PlayerProfile.Load();
            GameObject go = GameObject.Find("GameLoading");
            go.GetComponent<LoadingScene>().SwitchToMainScene();
            Debug.Log(m_PlayerProfile.m_PlayerName);
        }
        else
        {
            GameObject go = GameObject.Find("GameLoading");
            go.GetComponent<LoadingScene>().SwitchToRegisterScene();
            Debug.Log("2");
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

    public PlayerProfile GetPlayerProfile()
    {
        return m_PlayerProfile;
    }
}
