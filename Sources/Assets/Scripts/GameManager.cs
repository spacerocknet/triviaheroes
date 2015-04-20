using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public RectTransform m_MainPanel;
    public GameObject m_GameInfoPrefab;

    public List<GameObject> m_GameInfoList;

    public GameObject m_MainCanvas;
    public GameObject m_NewGameCanvas;
    public GameObject m_PvPCanvs;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnNewGame()
    {
        Debug.Log("On New Game");
        CanvasScript cs = m_NewGameCanvas.GetComponent<CanvasScript>();
        cs.MoveInFromRight();

        cs = m_MainCanvas.GetComponent<CanvasScript>();
        cs.MoveOutToLeft();
    }

    public void OnContinueGame(string gameid)
    {
        Debug.Log("On Continue Game");
        CanvasScript cs = m_PvPCanvs.GetComponent<CanvasScript>();
        cs.MoveInFromRight();
        cs = m_MainCanvas.GetComponent<CanvasScript>();
        cs.MoveOutToLeft();
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

    public void ReloadPassGameList() {
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
