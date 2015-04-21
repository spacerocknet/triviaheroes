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

    // Use this for initialization
    void Start()
    {
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMultiplayer()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[1];
        Debug.Log("On Multi");
        RefreshMultiTab();

        Vector2 v = m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, v.y);

        v = m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000, v.y);
    }

    public void OnSingle()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];
        Debug.Log("On Single");

        Vector3 v = m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition;
        m_MultiPanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-2000, v.y, v.z);

        v = m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition;
        m_SinglePanel.transform.parent.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, v.y, v.z);

        RefreshSingleTab();


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
