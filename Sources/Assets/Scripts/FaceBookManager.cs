using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook;
using Facebook.MiniJSON;
using System.Linq;
using System.Threading;
using SimpleJSON;
using UnityEngine.Advertisements;

public class Request
{
    public string from;
    public string name;    
    public string to;
    public string helper_name;
    public string solution;
    public int level;
    public int state;
    public string MyToString()
    {
        return from + " " + name + " " + to + " " + level + " " + state;
    }
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
            callback();
        }
    }

    private void FBLogin()
    {
        FB.Login("user_about_me, user_relationships, user_birthday", FBLoginCallback);
    }

    private void FBLoginCallback(FBResult result)
    {
        if (FB.IsLoggedIn)
        {
            IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.Text) as IDictionary;
            m_FBUserName = dict["name"].ToString();
            Debug.Log("FB Username: " + m_FBUserName);
            if (m_LoginCallback != null)
            {
                m_LoginCallback();
            }                        
        }
        else
        {
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
        if (result.Error != null)
        {
          
            return;
        }
        else
        { 
            try
            {
                var ret = JSON.Parse(result.Text);
                
                for (int i = 0; i < ret["to"].Count; i++)
                {
                    string from = FB.UserId;
                    string to = ret["to"][i];                    
                }
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
        var ret = JSON.Parse(result.Text);        
        List<object> friends = Util.DeserializeJSONFriends(result.Text);              
        for (int i = 0; i < friends.Count; i++)
        {                     
            //Debug.Log("Friend: " + (string)fd["name"] + " " + (string)fd["id"]);
        }
    }

    public string GetFBUserName()
    {
        return m_FBUserName;
    }
}
