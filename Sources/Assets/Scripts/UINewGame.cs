using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class UINewGame : MonoBehaviour {

    public GameObject m_MainCanvas;
    public GameObject m_NewGameCanvas;
    public GameObject m_GameMainCanvas;

    public GameObject m_FriendPrefab;
    public GameObject m_SingleStatePrefab;

    public GameObject m_MultiPanel;
    public GameObject m_SinglePanel;

    private GameMode m_SelecteMode;

    List<GameObject> m_FriendObjectList = new List<GameObject>();

    string m_SelectedFriend;

	// Use this for initialization
	void Start () {
        OnMultiPlayer();
        m_SelectedFriend = "Random";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBack()
    {
        CanvasScript cs = m_NewGameCanvas.GetComponent<CanvasScript>();
        cs.MoveOutToRight();

        cs = m_MainCanvas.GetComponent<CanvasScript>();
        cs.MoveInFromLeft();

        
    }

    public void StartGame()
    {

        if (m_SelecteMode == GameMode.GAMEMODE_PVE)
        {
            //CanvasScript cs = m_NewGameCanvas.GetComponent<CanvasScript>();
            //cs.MoveOutToLeft();
            GameManager.Instance.OnStartPVEGame();
        }
        else if (m_SelecteMode == GameMode.GAMEMODE_PVP)
        {
            GameManager.Instance.OnStartPVPGame(m_SelectedFriend);
        }
    }

    public void SetSelectedMode(GameMode mode)
    {
        m_SelecteMode = mode;
        GameLogic.Instance.SetSelectedMode(mode); 
    }

    public void OnMultiPlayer()
    {
        RefreshMultiTab();
        SetSelectedMode(GameMode.GAMEMODE_PVP);
    }

    public void OnSinglePlayer()
    {
        RefreshSingleTab();       
        SetSelectedMode(GameMode.GAMEMODE_PVE);
    }

    public void RefreshMultiTab()
    {
        FriendList fl = GameManager.Instance.GetPlayerProfile().m_FriendList;
        for (int i = 0; i < m_FriendObjectList.Count; i++)
        {
            GameObject.Destroy(m_FriendObjectList[i]);
        }
        m_FriendObjectList.Clear();
        int num = fl.m_FriendList.Count + 1;
        for (int i = 0; i < num; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_FriendPrefab);
            go.transform.parent = m_MultiPanel.transform;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -90 - i * 180, 0);
            rt.localScale = new Vector3(1, 1, 1);
            m_FriendObjectList.Add(go);

            if (i == 0)
            {
                go.GetComponent<FriendPrefab>().SetInfo("Random");
                go.GetComponent<FriendPrefab>().ShowSelected(true);
            }
            else
            {
                go.GetComponent<FriendPrefab>().SetInfo(fl.m_FriendList[i - 1]);
                go.GetComponent<FriendPrefab>().ShowSelected(false);
            }
        }
        m_MultiPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, num * 180);
    }

    public void RefreshSingleTab()
    {
        int num = 20;
        for (int i = num - 1; i >= 0; i--)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_SingleStatePrefab);
            go.transform.parent = m_SinglePanel.transform;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -90 - i * 180, 0);
            rt.localScale = new Vector3(1, 1, 1);

            go.GetComponent<SingleState>().SetIndex(i);

        }
        m_SinglePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, num * 180);
    }

    public void OnFriendSelected(GameObject friend)
    {
        
        for (int i = 0; i < m_FriendObjectList.Count; i++)
        {
            if (m_FriendObjectList[i] != friend)
            {
                m_FriendObjectList[i].GetComponent<FriendPrefab>().ShowSelected(false);
            }
            else
            {
                if (i == 0)
                {
                    m_SelectedFriend = "Random";
                }
                else
                {
                     FriendList fl = GameManager.Instance.GetPlayerProfile().m_FriendList;
                     m_SelectedFriend = fl.m_FriendList[i - 1];
                     Debug.Log(friend.name + " " + i + " " + m_SelectedFriend);
                }
            }
        }
    }
}
