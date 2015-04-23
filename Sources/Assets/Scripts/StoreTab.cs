using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class StoreTab : MonoBehaviour {

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

    public void OnCurrency()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];

        
    }

    public void OnUpgrade()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[1];

        
    }

    public void OnCustomize()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[2];

        
    }
}
