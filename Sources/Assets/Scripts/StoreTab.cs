using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class StoreTab : MonoBehaviour {

    public Sprite[] m_SpriteList;
    public GameObject m_CurrencyTab;
    public GameObject m_UpgradesTab;
    public GameObject m_CustomizeTab;
    

	// Use this for initialization
	void Start () {
        OnCurrency();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCurrency()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];
        m_CurrencyTab.SetActive(true);
        m_UpgradesTab.SetActive(false);
        m_CustomizeTab.SetActive(false);
    }

    public void OnUpgrade()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[1];
        m_CurrencyTab.SetActive(false);
        m_UpgradesTab.SetActive(true);
        m_CustomizeTab.SetActive(false);
    }

    public void OnCustomize()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[2];
        m_CurrencyTab.SetActive(false);
        m_UpgradesTab.SetActive(false);
        m_CustomizeTab.SetActive(true);
    }
}
