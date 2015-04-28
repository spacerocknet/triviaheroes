using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class UINewGame : MonoBehaviour {

    public GameObject m_MainCanvas;
    public GameObject m_NewGameCanvas;
    public GameObject m_GameMainCanvas;

    private GameMode m_SelecteMode;

	// Use this for initialization
	void Start () {
	
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
        
        CanvasScript cs = m_NewGameCanvas.GetComponent<CanvasScript>();
        cs.MoveOutToLeft();

        if (m_SelecteMode == GameMode.GAMEMODE_PVE)
        {
            GameLogic.Instance.OnStartPVEGame();
        }
        else if (m_SelecteMode == GameMode.GAMEMODE_PVP)
        {
        }
    }

    public void SetSelectedMode(GameMode mode)
    {
        m_SelecteMode = mode;
        GameLogic.Instance.SetSelectedMode(mode); 
    }

    public void OnMultiplayer()
    {
        RefreshMultiTab();
        Vector2 v = m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, v.y);
        v = m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000, v.y);
        SetSelectedMode(GameMode.GAMEMODE_PVP);
    }

    public void OnSingle()
    {
        RefreshSingleTab();
        Vector3 v = m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition;
        m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-2000, v.y, v.z);
        v = m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition;
        m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, v.y, v.z);        
        SetSelectedMode(GameMode.GAMEMODE_PVE);
    }

    public void RefreshMultiTab()
    {
        int num = 20;
        for (int i = num - 1; i >= 0; i--)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_FriendPrefab);
            go.transform.parent = m_MultiPanel.transform;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -90 - i * 180, 0);
            rt.localScale = new Vector3(1, 1, 1);
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
    
}
