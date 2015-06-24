using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SingleState : MonoBehaviour {

    int m_Index;

    public Image m_ImageCoin;
    public Text m_PrizeText;
    public Image m_CircleImage;
    public Image m_LineImage;

    public Sprite[] m_SpriteList;

    public GameObject m_Pivot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SetIndex(int index)
    {
        m_Index = index;
        if (index == GameConfig.Instance.GetNumberOfPvEStage())
        {
            transform.FindChild("ImageCircle").gameObject.GetComponent<Image>().enabled = false;
        }
        m_PrizeText.text = GameConfig.Instance.GetSingleReward(index - 1).ToString();
    }

    public void SetCompleted()
    {
    }

    public void Refresh()
    {
        m_Pivot.SetActive(false);
        PlayerProfile pl = GameManager.Instance.GetPlayerProfile();
        Debug.Log("PVEStage: " + pl.m_CurrentPVEStage);
        if (m_Index <= pl.m_CurrentPVEStage)
        {
            m_CircleImage.sprite = m_SpriteList[3];
            if (m_Index == pl.m_CurrentPVEStage)
            {
                m_LineImage.sprite = m_SpriteList[1];                
            }
            else
            {
                m_LineImage.sprite = m_SpriteList[1];
            }
        }
        else
        {
            if (m_Index == pl.m_CurrentPVEStage + 1)
            {
                m_Pivot.SetActive(true);
            }
            m_CircleImage.sprite = m_SpriteList[2];
            m_LineImage.sprite = m_SpriteList[0];
        }

        if (pl.m_PVEState[m_Index - 1] == 1)
        {
            m_ImageCoin.sprite = m_SpriteList[4];
        }        
    }
}
