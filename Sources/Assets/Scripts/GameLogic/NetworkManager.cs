using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class NetworkManager : MonoBehaviour {

    public delegate void OnServerCallBack(string result);

    private static NetworkManager m_sInstance = null;    

    public void Awake()
    {
        m_sInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    private NetworkManager()
    {
        m_sInstance = this;
    }

    public static NetworkManager Instance
    {
        get
        {
            if (m_sInstance == null)
            {
                m_sInstance = new NetworkManager();
            }
            return m_sInstance;
        }
    }

    IEnumerator ActuallyDoRegister(string name, int sex)
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
        var ret = new JSONClass();
        ret["result"].AsBool = true;
        ret["name"] = name;
        ret["sex"].AsInt = sex;
        GameManager.Instance.OnRegisterResult(ret.ToString());
    }

    IEnumerator ActuallyDoStartNewGame(string friend)
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
        var ret = new JSONClass();
        if (friend == "Random")
        {
            ret["opponent"] = "RandomOpponent";
        }
        else
        {
            ret["opponent"] = friend;
        }
        Debug.Log("HIHI");
        GameManager.Instance.OnStartNewGameResult(ret.ToString());
    }

    public void DoRegister(string name, int sex)
    {
        StartCoroutine(ActuallyDoRegister(name, sex));
    }

    public void DoStartNewGame(string friend)
    {
        StartCoroutine(ActuallyDoStartNewGame(friend));
    }

    public void DoCategoryConfirmToPlay(Category cat)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST("http://54.163.250.79:9000/v1/quiz/request", dict, GameManager.Instance.OnCategoryConfirmToPlayResult);   
    }

    public void DoTrophyClaimSelected(int trophy)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST("http://54.163.250.79:9000/v1/quiz/request", dict, GameManager.Instance.OnTrophyClaimSelectedResult);
    }

    public void DoTrophyChallengeSelected()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST("http://54.163.250.79:9000/v1/quiz/request", dict, GameManager.Instance.OnTrophyChallengeSelectedResult);        
    }

    public void DoTrophyChallangeNextQuestion()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST("http://54.163.250.79:9000/v1/quiz/request", dict, GameManager.Instance.DoTrophyChallangeNextQuestionResult);        
    }

    public void OnRegisterResult()
    {
    }

    public void GetQuestion()
    {
    }

    public WWW GET(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www, null));
        return www;
    }

    public WWW POST(string url, Dictionary<string, string> post, OnServerCallBack callback)
    {
        var encoding = new System.Text.UTF8Encoding();
        var postHeader = new Hashtable();


        string jsonString = "{";

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

        StartCoroutine(WaitForRequest(www, callback));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www, OnServerCallBack callback)
    {
        Debug.Log(www.progress);
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            callback(www.text);            
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
