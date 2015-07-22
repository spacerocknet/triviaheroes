using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIInbox : MonoBehaviour {

    public GameObject m_FriendRequestPrefab;
    public GameObject m_Panel;    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Refresh() 
    {
        List<Image> imgList = new List<Image>();
        List<string> urlList = new List<string>();

        List<Request> requestList = FaceBookManager.Instance.GetSentRequestList();

        int num = requestList.Count;
        for (int i = 0; i < 10; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_FriendRequestPrefab);
            go.transform.SetParent(m_Panel.transform);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(10, -200 - i * 180, 0);
            rt.localScale = new Vector3(1, 1, 1);

            go.transform.FindChild("Name").GetComponent<Text>().text = requestList[i].friendName;
            imgList.Add(go.transform.FindChild("Avatar").GetComponent<Image>());
            urlList.Add("http://graph.facebook.com/" + requestList[i].friendID + "/picture?type=square");
        }
        m_Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Panel.GetComponent<RectTransform>().sizeDelta.x, num * 180);
        StartCoroutine(LoadAvatar(imgList, urlList));
    }

    IEnumerator LoadAvatar(List<Image> list, List<string> urlList)
    {
        for (int i = 0; i < urlList.Count; i++)
        {
            WWW www = new WWW(urlList[i]);
            yield return www;
            Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            list[i].sprite = sprite;
        }
    }

    public void OnSendLives()
    {
        FaceBookManager.Instance.SendLivesToFriend();
    }
}
