﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISettingSlider : MonoBehaviour {

    public Image m_AlertImage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBack()
    {
        //Debug.Log("Back");
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveOutToLeftFar();
    }

    public void OnStore()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveOutToLeft();
        cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_STORE);
        cv.MoveInFromRight();
        
        
        if (m_AlertImage.gameObject.activeInHierarchy)
        {
            
            cv.GetComponent<UIStore>().ShowUpgradeTab();
            GameManager.Instance.ShowHelpUpgrade(false);
            GameManager.Instance.OnFirstTimeUserExperienceComplete(2);

            if (GameManager.Instance.GetActiveAvatar().m_Tier == TIER.Young_Adult)
            {
                GameManager.Instance.OnFirstTimeUserExperienceComplete(3);
                CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                cs.GetComponent<UIPopup>().Show("As an adult you can select a career. Career paths unlock a new ability for your avatar.", 0, null, null, (int)CanvasID.CANVAS_STORE); 
            }

            if (GameManager.Instance.GetActiveAvatar().m_Tier == TIER.Elder)
            {
                GameManager.Instance.OnFirstTimeUserExperienceComplete(5);
                CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                cs.GetComponent<UIPopup>().Show("True mastery has been attained. It is up to you to train the new generation.", 0, null, null, (int)CanvasID.CANVAS_STORE);
            }
        }

        cv.GetComponent<UIStore>().Refresh();
    }

    public void OnProfile() 
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveOutToLeft();
        cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PROFILE);
        cv.MoveInFromRight();
        cv.GetComponent<UIProfile>().Refresh();
    }

    public void ShowAlertImage(bool value)
    {
        m_AlertImage.gameObject.SetActive(value);
    }

    public void OnFeedBack()
    {
        SendEmail();
    }

    public void OnFAQ()
    {
        Application.OpenURL("http://spacerock.net/triviaheroes/faq");
    }

    void SendEmail()
    {
        string email = "support@spacerock.net";
        string subject = MyEscapeURL("Trivia Heroes – user feedback");
        string body = MyEscapeURL("MSG\r\n\r\n\r\nUser ID: " + GameManager.Instance.GetPlayerProfile().m_PlayerName + "\r\nDevice ID: " + SystemInfo.deviceUniqueIdentifier + "\r\nOS version: " + SystemInfo.operatingSystem);

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void OnFacebook()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_INBOX);
        cs.Show();
        cs.GetComponent<UIInbox>().Refresh();
    }
}
