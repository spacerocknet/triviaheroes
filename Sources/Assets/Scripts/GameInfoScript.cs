using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameInfoScript : MonoBehaviour {

    public string m_SessionID;
    public GameObject m_GameManager;

    public Text m_NameText;
    public Text m_RoundText;
    public Text m_TimerText;
    public Text m_ScoreAText;
    public Text m_ScoreBText;
    public Image m_ScoreImage;
    public RectTransform m_LeftPos;
    public RectTransform m_RightPos;

    public AvatarScript m_AvatarScript;

	// Use this for initialization
	void Start () {
        m_GameManager = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGameSelect()
    {
        //Debug.Log("Game selected");
        GameManager gm = m_GameManager.GetComponent<GameManager>();
        gm.OnContinueGame(m_SessionID);
    }

    public void SetGameInfo(GameInfo gi)
    {
        m_NameText.text = gi.GetOpponentName(GameManager.Instance.GetPlayerID());
        m_RoundText.text = "Round: " + gi.m_Round.ToString();
        int sa = gi.GetScoreA();
        int sb = gi.GetScoreB();
        int sum = sa + sb;
        float percent;
        if (sb == 0)
        {
            percent = 0.5f;
        }
        else
        {
            percent = (float)sa / sum;
        }
        m_ScoreAText.text = gi.GetScoreA().ToString();
        m_ScoreBText.text = gi.GetScoreB().ToString();
        m_ScoreImage.rectTransform.anchoredPosition = m_LeftPos.anchoredPosition + (m_RightPos.anchoredPosition - m_LeftPos.anchoredPosition) * percent;
        m_SessionID = gi.m_SessionID;

        m_AvatarScript.SetInfo(gi.GetOpponentAvatar(GameManager.Instance.GetPlayerID()), false);
    }

    
}
