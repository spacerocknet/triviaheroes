using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UISelectPieceChallenge : MonoBehaviour {

    public Image[] m_TrophyImage;
    public Sprite[] m_TrophySprite;
    public Image[] m_CheckImage;
    int m_MyTrophy;
    int m_TheirTrophy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlay()
    {
        if (m_MyTrophy != -1 && m_TheirTrophy != -1)
        {
            SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECHALLENGE).MoveOutToRight();
            GameManager.Instance.OnTrophyChallengeSelected(m_MyTrophy, m_TheirTrophy);
        }
    }

    public void OnMyTrophySelected(int trophy)
    {
        m_MyTrophy = trophy - 1;
        for (int i = 0; i < 6; i++)
        {
            if (i + 1 == trophy)
            {
                m_CheckImage[i].enabled = true;
            }
            else
            {
                m_CheckImage[i].enabled = false;
            }
        }        
    }

    public void OnTheirTrophySelected(int trophy)
    {
        m_TheirTrophy = trophy - 1;
        m_MyTrophy = trophy - 1;
        for (int i = 6; i < 12; i++)
        {
            if (i + 1 == trophy + 6)
            {
                m_CheckImage[i].enabled = true;
            }
            else
            {
                m_CheckImage[i].enabled = false;
            }
        } 
    }

    public void SetTrophyState(List<int> myTrophy, List<int> theirTrophy)
    {
        m_MyTrophy = -1;
        m_TheirTrophy = -1;
        bool flag = false;
        for (int i = 0; i < myTrophy.Count; i++) {
            m_CheckImage[i].enabled = false;
            if (myTrophy[i] == 0) {
                m_TrophyImage[i].sprite = m_TrophySprite[i + 6];
                m_TrophyImage[i].GetComponent<Button>().interactable = false;
            } else {
                m_TrophyImage[i].sprite = m_TrophySprite[i];
                m_TrophyImage[i].GetComponent<Button>().interactable = true;
                if (!flag)
                {
                    m_CheckImage[i].enabled = true;
                    flag = true;
                    m_MyTrophy = i;
                }
            }
            
        }
        flag = false;
        for (int i = 0; i < theirTrophy.Count; i++)
        {
            m_CheckImage[i + 6].enabled = false;
            if (theirTrophy[i] == 0)
            {
                m_TrophyImage[i + 6].sprite = m_TrophySprite[i + 6];
                m_TrophyImage[i + 6].GetComponent<Button>().interactable = false;
            }
            else
            {
                m_TrophyImage[i + 6].sprite = m_TrophySprite[i];
                m_TrophyImage[i + 6].GetComponent<Button>().interactable = true;
                if (!flag)
                {
                    m_CheckImage[i + 6].enabled = true;
                    flag = true;
                    m_TheirTrophy = i;
                }
            }            
        }
    }
}

