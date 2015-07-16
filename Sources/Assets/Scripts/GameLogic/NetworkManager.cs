using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class NetworkManager : MonoBehaviour {

    public delegate void OnServerCallBack(string result);

    private static NetworkManager m_sInstance = null;

    private static string SERVER_IP = "http://52.4.79.61:9000/v1";

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
        //Debug.Log("HIHI");
        GameManager.Instance.OnStartNewGameResult(ret.ToString());
        
    }

    public void DoRegister(string name, int sex)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            dict.Add("platform", "IOS");
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            dict.Add("platform", "Android");
        }
        dict.Add("os", SystemInfo.operatingSystem);
        dict.Add("model", SystemInfo.deviceModel);        
        dict.Add("device_uuid", SystemInfo.deviceUniqueIdentifier + "17");
        dict.Add("type", "mobile");
        dict.Add("name", name);
        dict.Add("sex", sex.ToString());
        Debug.Log("Register");
        POST(SERVER_IP + "/user/addnoinfo", dict, GameManager.Instance.OnRegisterCallback);
        //StartCoroutine(ActuallyDoRegister(name, sex));
    }

    public void DoStartNewGame(string friend)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("uid", GameManager.Instance.GetPlayerProfile().m_PlayerID);
        POST(SERVER_IP + "/gamesession/createorjoin", dict, GameManager.Instance.OnStartNewGameResult);  
    }

    public void DoUpdateSessionInfo(string sessionid, string sessioninfo)
    {
        
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("uid", GameManager.Instance.GetPlayerProfile().m_PlayerID);
        dict.Add("game_session_id", sessionid);
        dict.Add("state", "0");
        dict.Add("attributes", sessioninfo);
        POST(SERVER_IP + "/gamesession/updategamesessionstate", dict, GameManager.Instance.OnUpdateSessionInfoResult);
    }

    public void DoGetAllSessionInfo()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        Debug.Log(GameManager.Instance.GetPlayerProfile().m_PlayerID);
        dict.Add("uid", GameManager.Instance.GetPlayerProfile().m_PlayerID);
        POST(SERVER_IP + "/gamesession/getgamesessions", dict, GameManager.Instance.OnGetAllSessionInfoResult);
    }

    public void SkipQuestion(Category cat)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST(SERVER_IP + "/quiz/request", dict, GameManager.Instance.OnSkipQuestionResult);   
    }

    public void DoCategoryConfirmToPlay(Category cat)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST(SERVER_IP + "/quiz/request", dict, GameManager.Instance.OnCategoryConfirmToPlayResult);   
    }

    public void DoTrophyClaimSelected(int trophy)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST(SERVER_IP + "/quiz/request", dict, GameManager.Instance.OnTrophyClaimSelectedResult);
    }

    public void DoTrophyChallengeSelected()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST(SERVER_IP + "/quiz/request", dict, GameManager.Instance.OnTrophyChallengeSelectedResult);        
    }

    public void DoTrophyChallengeAccepted()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST(SERVER_IP + "/quiz/request", dict, GameManager.Instance.OnTrophyChallengeSelectedResult);
    }

    public void DoTrophyChallangeNextQuestion()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST(SERVER_IP + "/quiz/request", dict, GameManager.Instance.DoTrophyChallangeNextQuestionResult);        
    }

    public void DoGetPVEQuestion()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("category", "Sports");
        dict.Add("num", "1");
        POST(SERVER_IP + "/quiz/request", dict, GameManager.Instance.DoDoGetPVEQuestionQuestionResult);   
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
            if (kvp.Key != "attributes")
            {
                jsonString = jsonString + "\"" + kvp.Key + "\":" + "\"" + kvp.Value + "\",";
            }
            else
            {
                jsonString = jsonString + "\"" + kvp.Key + "\":" + kvp.Value + ",";
            }
        }

        jsonString = jsonString.Substring(0, jsonString.Length - 1);
        jsonString = jsonString + "}";
        Debug.Log(jsonString);
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "Application/json");
        header.Add("Content-Length", jsonString.Length.ToString());
        WWW www = new WWW(url, encoding.GetBytes(jsonString), header);

        StartCoroutine(WaitForRequest(www, callback));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www, OnServerCallBack callback)
    {
        float startTime = Time.time;
       
        //Debug.Log(www.progress);
        yield return www;

        // check for errors
        if (www.error == null)
        {
            while (Time.time - startTime < 3)
            {
                yield return new WaitForEndOfFrame();
            }
            callback(www.text);
        } else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    } 
}
