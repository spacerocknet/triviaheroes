using UnityEngine;
using System.Collections;

public class GameInfoScript : MonoBehaviour {

    public int m_GameID;
    public GameObject m_GameManager;

	// Use this for initialization
	void Start () {
        m_GameManager = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGameSelect()
    {
        Debug.Log("Game selected");
        GameManager gm = m_GameManager.GetComponent<GameManager>();
        gm.OnContinueGame("gameid");
    }
}
