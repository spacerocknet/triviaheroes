using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class ProfileTab : MonoBehaviour {

    public Sprite[] m_SpriteList;
    public GameObject m_AchievementTab;
    public GameObject m_HistoryTab;
    public GameObject m_CustomizeTab;
    

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

        m_AchievementTab.gameObject.SetActive(false);
        m_CustomizeTab.gameObject.SetActive(false);
        m_HistoryTab.gameObject.SetActive(true);
    }

    public void OnYourTurn()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];

        m_AchievementTab.gameObject.SetActive(true);
        m_CustomizeTab.gameObject.SetActive(false);
        m_HistoryTab.gameObject.SetActive(false);
    }

    public void OnPastGame()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[2];

        m_AchievementTab.gameObject.SetActive(false);
        m_CustomizeTab.gameObject.SetActive(true);
        m_HistoryTab.gameObject.SetActive(false);
    }
}
