using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIOutLives : MonoBehaviour {

    public Transform m_Panel;
    public GameObject m_InviteFriendPrefab;
    List<InviteFriendPrefab> m_FriendList = new List<InviteFriendPrefab>();

	// Use this for initialization
	void Start () {
        //RefreshFriendList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void RefreshFriendList()
    {
        m_FriendList.Clear();
        FriendList fl = GameManager.Instance.GetPlayerProfile().m_FriendList;
        for (int i = 0; i < fl.m_FriendList.Count; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_InviteFriendPrefab);
            go.transform.SetParent(m_Panel);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -90 - i * 180, 0);
            rt.localScale = new Vector3(1, 1, 1);
            m_FriendList.Add(go.GetComponent<InviteFriendPrefab>());            
        }
        m_Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, fl.m_FriendList.Count * 180);
    }

    public void OnRequest() 
    {
        FaceBookManager.Instance.SendFriendRequest();
        GetComponent<CanvasScript>().Hide();
    }

    public void OnBuy()
    {
        GetComponent<CanvasScript>().Hide();
        for (int i = 0; i < 5; i++)
        {
            GameManager.Instance.GetPlayerProfile().AddLives();
            GameManager.Instance.GetPlayerProfile().Save();
        }
    }

    public void OnHide()
    {
        GetComponent<CanvasScript>().Hide();
    }
}
