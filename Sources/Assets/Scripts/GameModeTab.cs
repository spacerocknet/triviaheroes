using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class GameModeTab : MonoBehaviour {

    public GameObject m_MultiPanel;
    public GameObject m_SinglePanel;
    public GameObject m_FriendPrefab;
    public GameObject m_SingleStatePrefab;
    public Sprite[] m_SpriteList;
    GameManager m_GameManager;
    public UINewGame m_UINewGame;

    // Use this for initialization
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMultiplayer()
    {
        
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[1];
        m_UINewGame.OnMultiPlayer();
        RefreshMultiTab();
        Vector2 v = m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, v.y);
        v = m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000, v.y);
        m_UINewGame.SetSelectedMode(GameMode.GAMEMODE_PVP);
    }

    public void OnSingle()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];
        m_UINewGame.OnSinglePlayer();
        Vector3 v = m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition;
        m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-2000, v.y, v.z);
        v = m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition;
        m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, v.y, v.z);
        RefreshSingleTab();
        m_UINewGame.SetSelectedMode(GameMode.GAMEMODE_PVE);
    }

}
