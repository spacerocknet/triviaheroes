using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class GameTab : MonoBehaviour {

    public Sprite[] m_SpriteList;
    GameManager m_GameManager;

	// Use this for initialization
	void Start () {
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTheirTurn()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[1];

        m_GameManager.ReloadTheirTurnList();
    }

    public void OnYourTurn()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];

        m_GameManager.ReloadYourTurnList();
    }

    public void OnPastGame()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[2];

        m_GameManager.ReloadPassGameList();
    }
}
