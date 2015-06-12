using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEndgameResult : MonoBehaviour {

    public Image m_WinImage;
    public Image m_LoseImage;
    public Text m_QuestionText;
    public Text m_RewardTitleText;
    public Text m_RewardText;
    public Text m_LifeText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnYes()
    {
        
    }

    public void OnNo()
    {
        
    }

    public void OnNext()
    {
        GameManager.Instance.OnEndPvEGameConfirm();
    }

    public void SetResult(bool isWin)
    {
        if (isWin)
        {
            m_WinImage.gameObject.SetActive(true);
            m_LoseImage.gameObject.SetActive(false);
            m_RewardTitleText.gameObject.SetActive(true);
            m_RewardText.gameObject.SetActive(true);
            m_LifeText.gameObject.SetActive(false);

            int reward = 0;
            int bonus = 0;
            if (GameManager.Instance.GetPlayerProfile().m_CurrentPVEStage > 0)
            {
                reward = GameConfig.Instance.GetSingleReward(GameManager.Instance.GetPlayerProfile().m_CurrentPVEStage - 1);
                bonus = Mathf.RoundToInt((float)GameManager.Instance.GetPlayerProfile().m_PayOutBonus / 100 * reward);
            }

            if (GameManager.Instance.GetPlayerProfile().m_PVEState[GameManager.Instance.GetPlayerProfile().m_CurrentPVEStage - 1] == 1)
            {
                m_RewardText.text = "Claimed";
            }
            else
            {
                m_RewardText.text = reward.ToString() + " + " + bonus.ToString();
            }
        }
        else
        {
            m_WinImage.gameObject.SetActive(false);
            m_LoseImage.gameObject.SetActive(true);
            m_RewardTitleText.gameObject.SetActive(false);
            m_RewardText.gameObject.SetActive(false);
            m_LifeText.gameObject.SetActive(true);
        }
        m_QuestionText.text = "Question #" + GameManager.Instance.GetPlayerProfile().m_CurrentPVEStage;
    }
}
