using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class GameTab : MonoBehaviour {

    public Sprite[] m_SpriteList;
    GameManager m_GameManager;
    public UIMain m_UIMain;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTheirTurn()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[1];
        m_UIMain.OnTheirTurn();
    }

    public void OnYourTurn()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];
        m_UIMain.OnYourTurn();
    }

    public void OnPastGame()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[2];
        m_UIMain.OnPastGame();
    }
}
