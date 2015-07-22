using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook;
using Facebook.MiniJSON;
using System.Linq;
using System.Threading;
//using SimpleJSON;
using UnityEngine.Advertisements;

public class Request
{
    public string friendName;
    public string friendID;    
    public string requestID;    
}

public class Friend
{
    public string id;
    public string name;
    public int level;

    public string pic_url;
}


public class FaceBookManager : MonoBehaviour {

    private static FaceBookManager m_sInstane;

    public delegate void OnFaceBookLogined();

    OnFaceBookLogined m_LoginCallback;

    private string m_FBUserName;

    private List<Request> m_AskRequestList;
    private List<Request> m_SentRequestList;

    void Awake()
    {
        m_sInstane = this;
    }

    public static FaceBookManager Instance
    {
        get
        {
            if (m_sInstane == null)
            {
                m_sInstane = new FaceBookManager();
            }
            return m_sInstane;
        }
    }

	// Use this for initialization
	void Start () {
        m_AskRequestList = new List<Request>();
        m_SentRequestList = new List<Request>();
        DontDestroyOnLoad(gameObject);
        FB.Init(OnInitCompleted);        
	}

	
	// Update is called once per frame
	void Update () {
	}

    public void LoginFacebook(OnFaceBookLogined callback)
    {
        m_LoginCallback = callback;
        if (!FB.IsLoggedIn)
        {
            FBLogin();
        }
        else
        {
            GetMyInfo();
        }
    }

    private void FBLogin()
    {
        FB.Login("user_about_me, user_relationships, user_birthday, user_location, user_friends", FBLoginCallback);
    }

    private void FBLoginCallback(FBResult result)
    {
        if (FB.IsLoggedIn)
        {
            GetMyInfo();            
        }
        else
        {
            Debug.Log("Login failed");
        }
    }    

    private void OnInitCompleted()
    {
        if (FB.IsLoggedIn)
        {
            
        }
        else
        {
            
        }
    }

    public void SendInvite()
    {
        //Debug.Log("HAHA");
        FB.AppRequest(
            message: "Help me this level!",

            callback: InviteCallBack
            );
        
    }

    void InviteCallBack(FBResult result)
    {
        Debug.Log("InviteCallBack: " + result.Text);
        if (result.Error != null)
        {
          
            return;
        }
        else
        { 
            try
            {
                
            }
            catch (Exception e)
            {                
            }
        }

    }

    void DeleteCallBack(FBResult result)
    {
        Debug.Log("DeleteCallback: " + result.Text);
        if (result.Error != null)
        {

            return;
        }
        else
        {
            try
            {

            }
            catch (Exception e)
            {
            }
        }

    }

    public void GetMyInfo()
    {
        FB.API("/me?fields=id,first_name,last_name,friends.limit(100).fields(name,id,picture.width(256).height(256))", Facebook.HttpMethod.GET, GetInfoCallback);
    }

    public void GetInfoCallback(FBResult result)
    {
        Debug.Log(result.Text);
        var ret = SimpleJSON.JSON.Parse(result.Text);
        m_FBUserName = ret["first_name"] + " " + ret["last_name"];
        List<object> friends = Util.DeserializeJSONFriends(result.Text);              
        if (m_LoginCallback != null)
        {
            m_LoginCallback();
        }
    }

    public string GetFBUserName()
    {
        //return "Ngọc Huỳnh";
        return m_FBUserName;
    }

    public void SendFriendRequest()
    {
        if (FB.IsLoggedIn)
        {
            FB.AppRequest("Please send me lives!!", null, new List<object>() { "app_users" }, null, null, "AskForLives", null, InviteCallBack);            
        }
        else
        {
            LoginFacebook(SendFriendRequest);
        }
    }

    public void CheckRequest()
    {
        FB.API("/me/apprequests", Facebook.HttpMethod.GET, CheckRequestCallback);
    }

    public void CheckRequestCallback(FBResult result)
    {
        Debug.Log(result.Text);
        m_AskRequestList.Clear();
        m_SentRequestList.Clear();
        var ret = SimpleJSON.JSON.Parse(result.Text);
        for (int i = 0; i < ret["data"].Count; i++)
        {
            Request r = new Request();
            r.friendName = ret["data"][i]["from"]["name"];
            r.friendID = ret["data"][i]["from"]["id"];
            r.requestID = ret["data"][i]["id"];
            string s = ret["data"][i]["data"];            
            if (s.Equals("AskForLives"))
            {                
                m_AskRequestList.Add(r);
            }
            else if (s.Equals("FullLives"))
            {                
                m_SentRequestList.Add(r);                
            }
        }        
        if (m_AskRequestList.Count > 0)
        {
            GameManager.Instance.ShowRequestPopup();
        }
    }

    public List<Request> GetRequestList()
    {
        return m_AskRequestList;
    }

    public void AcceptAllRequest()
    {
        string[] recipient = new string[m_AskRequestList.Count];
        for (int i = 0; i < m_AskRequestList.Count; i++)
        {
            recipient[i] = m_AskRequestList[i].friendID;
        }
        FB.AppRequest("Lives sent to you!!", recipient, new List<object>() { "app_users" }, null, null, "FullLives", null, InviteCallBack);
        DeleteAllRequest();
    }

    public void DeleteAllRequest()
    {
        for (int i = 0; i < m_AskRequestList.Count; i++)
        {
            FB.API("/" + m_AskRequestList[i].requestID, Facebook.HttpMethod.DELETE, DeleteCallBack);            
        }
    }

    public List<Request> GetSentRequestList()
    {
        return m_SentRequestList;
    }

    public void SendLivesToFriend()
    {
        FB.AppRequest("Lives sent to you!!", null, new List<object>() { "app_users" }, null, null, "FullLives", null, InviteCallBack);
    }
}
