using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAchievement : MonoBehaviour {

    public Text m_Title;
    public Text m_Description;
    public Image m_Progress;
    public Text m_Counter;
    public Image m_Stars;
    public Sprite[] m_StarsSprite;

    public Button m_ClaimButton;
    int m_ID;
    Achievement m_Achievement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInfo(Achievement a, int id)
    {
        m_Title.text = a.m_Title;
        m_Description.text = a.m_Description;
        m_Counter.text = a.m_Counter + "/" + a.m_Requirement[2];
        m_Progress.GetComponent<RectTransform>().localScale = new Vector3((float)a.m_Counter / a.m_Requirement[2], 1, 1);
        m_ID = id;
        m_Achievement = a;

        m_ClaimButton.gameObject.SetActive(false);

        if (a.m_Counter >= a.m_Requirement[2])
        {
            m_Stars.sprite = m_StarsSprite[3];
            if (GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[id] < 3)
            {
                m_ClaimButton.gameObject.SetActive(true);
            }
        } else if (a.m_Counter >= a.m_Requirement[1])
        {
            m_Stars.sprite = m_StarsSprite[2];
            if (GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[id] < 2)
            {
                m_ClaimButton.gameObject.SetActive(true);
            }
        } else if (a.m_Counter >= a.m_Requirement[0])
        {
            if (GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[id] < 1)
            {
                m_ClaimButton.gameObject.SetActive(true);
            }
            m_Stars.sprite = m_StarsSprite[1];
        } else {
            m_Stars.sprite = m_StarsSprite[0];            
        }
    }

    public void OnClaim()
    {
        int reward = 0;
        if (m_Achievement.m_Counter >= m_Achievement.m_Requirement[0])
        {
            if (GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[m_ID] < 1)
            {
                GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[m_ID] = 1;
                reward += 10;
            }            
        }
        if (m_Achievement.m_Counter >= m_Achievement.m_Requirement[1])
        {
            if (GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[m_ID] < 2)
            {
                GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[m_ID] = 2;
                reward += 20;
            }
        }
        if (m_Achievement.m_Counter >= m_Achievement.m_Requirement[2])
        {
            if (GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[m_ID] < 3)
            {
                GameManager.Instance.GetPlayerProfile().m_AchievementBonusReceived[m_ID] = 3;
                reward += 30;
            }
        }
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
        cs.GetComponent<UIPopup>().Show(reward + " diamond added to your account", 0, null, null, (int)CanvasID.CANVAS_STORE);               
        GameManager.Instance.AddDiamond(reward);
        m_ClaimButton.gameObject.SetActive(false);
    }
}
