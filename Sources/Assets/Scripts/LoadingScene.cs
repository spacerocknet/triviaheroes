using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadingScene : MonoBehaviour {

    public Text m_LoadingTips;
    float m_Timer;
    AsyncOperation m_LoadLevelAsync = null;
    public float m_DisplayTime;
	// Use this for initialization
	void Start () {
        //GET("http://54.163.250.79:9000/v1/cat/game/1");

        //Dictionary<string, string> dict = new Dictionary<string, string>();
        //dict.Add("platform", "IOS");
        //dict.Add("os", "4.3");
        //dict.Add("model", "Note1");
        //dict.Add("phone", "14042309331");
        //dict.Add("device_uuid", "test_udid");
        //dict.Add("type", "mobile");
        //POST("http://54.163.250.79:9000/v1/user/addnoinfo", dict);

        //Dictionary<string, string> dict = new Dictionary<string, string>();
        //dict.Add("category", "Sports");
        //dict.Add("num", "1");        
        m_LoadingTips.text = GameConfig.Instance.GetRandomTips();
	}

    public WWW GET(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
        return www;
    }

    public WWW POST(string url, Dictionary<string, string> post)
    {
        var encoding = new System.Text.UTF8Encoding();
        var postHeader = new Hashtable();
        
        
        string jsonString = "{" ;

        foreach (KeyValuePair<string, string> kvp in post)
        {
            jsonString = jsonString + "\"" + kvp.Key + "\":" + "\"" + kvp.Value + "\",";
        }

        jsonString = jsonString.Substring(0, jsonString.Length - 1);
        jsonString = jsonString + "}";

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "Application/json");
        header.Add("Content-Length", jsonString.Length.ToString());
        WWW www = new WWW(url, encoding.GetBytes(jsonString), header);        
        //WWWForm form = new WWWForm();
        //foreach (KeyValuePair<string, string> post_arg in post)
        //{
        //    form.AddField(post_arg.Key, post_arg.Value);
        //}
        //WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        ////Debug.Log(www.progress);
        yield return www;

        // check for errors
        if (www.error == null)
        {
            //Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            //Debug.Log("WWW Error: " + www.error);
        }
    }
	
	// Update is called once per frame
	void Update () {
        m_Timer += Time.deltaTime;        
        if (m_Timer > m_DisplayTime && m_LoadLevelAsync != null && m_LoadLevelAsync.progress >= 0.9f)
        {
            m_LoadLevelAsync.allowSceneActivation = true;
        }
	}

    IEnumerator LoadNextScene(string nextScene)
    {
        m_LoadLevelAsync = Application.LoadLevelAsync(nextScene);
        m_LoadLevelAsync.allowSceneActivation = false;                
        yield return m_LoadLevelAsync;        
    }

    public void SwitchToMainScene()
    {
        StartCoroutine(LoadNextScene("MainScene"));
    }

    public void SwitchToRegisterScene()
    {
        StartCoroutine(LoadNextScene("RegisterScene"));
    }
}
