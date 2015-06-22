using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UISelectPieceClaim : MonoBehaviour {

    public Image[] m_TrophyImage;
    public Sprite[] m_TrophySprite;
    public Image[] m_CheckImage;
    int m_CurrentSelected = -1;

    public Text m_ButtonText;
    bool m_IsAbilityClaim;
    //bool m_IsAbilityRemove;

    public bool IsAbilityClaim
    {
        get
        {
            return m_IsAbilityClaim;
        }
        set
        {
            m_IsAbilityClaim = value;
        }
    }

    //public bool IsAbilityRemove
    //{
    //    get
    //    {
    //        return m_IsAbilityRemove;
    //    }
    //    set
    //    {
    //        m_IsAbilityRemove = value;
    //    }
    //}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateTrophyState()
    {
        bool flag = false;
        List<int> trophy = GameManager.Instance.GetCurrentTrophyState();
        for (int i = 0; i < 6; i++)
        {
            if (trophy[i] == 0)
            {
                m_TrophyImage[i].sprite = m_TrophySprite[i];
                m_TrophyImage[i].GetComponent<Button>().interactable = true;
                m_CheckImage[i].enabled = false;
                if (!flag)
                {
                    m_CheckImage[i].enabled = true;
                    flag = true;
                    m_CurrentSelected = i;
                }
            }
            else
            {
                m_TrophyImage[i].sprite = m_TrophySprite[6 + i];
                m_TrophyImage[i].GetComponent<Button>().interactable = false;
                m_CheckImage[i].enabled = false;
            }            
        }
    }

    public void UpdateOpponentState()
    {
        bool flag = false;
        List<int> trophy = GameManager.Instance.GetOpponentTrophyList();
        for (int i = 0; i < 6; i++)
        {
            if (trophy[i] == 1)
            {
                m_TrophyImage[i].sprite = m_TrophySprite[i];
                m_TrophyImage[i].GetComponent<Button>().interactable = true;
                m_CheckImage[i].enabled = false;
                if (!flag)
                {
                    m_CheckImage[i].enabled = true;
                    flag = true;
                    m_CurrentSelected = i;
                }
            }
            else
            {
                m_TrophyImage[i].sprite = m_TrophySprite[6 + i];
                m_TrophyImage[i].GetComponent<Button>().interactable = false;
                m_CheckImage[i].enabled = false;
            }
        }
    }

    void UpdateSelected(int selected)
    {
        
        m_CurrentSelected = selected;
        for (int i = 0; i < 6; i++)
        {
            if (i == selected)
            {
                m_CheckImage[i].enabled = true;
            }
            else
            {
                m_CheckImage[i].enabled = false;
            }
        }
    }

    public void OnTrophySelected0()
    {
        UpdateSelected(0);
    }

    public void OnTrophySelected1()
    {
        UpdateSelected(1);
    }

    public void OnTrophySelected2()
    {
        UpdateSelected(2);
    }

    public void OnTrophySelected3()
    {
        UpdateSelected(3);
    }

    public void OnTrophySelected4()
    {
        UpdateSelected(4);
    }

    public void OnTrophySelected5()
    {
        UpdateSelected(5);
    }

    public void OnPlay()
    {
        if (m_CurrentSelected != -1)
        {
            if (m_IsAbilityClaim)
            {
                SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECLAIM).MoveOutToRight();
                GameManager.Instance.OnAbilityClaimSelected(m_CurrentSelected);
            } else
            //if (m_IsAbilityRemove) {
            //    Debug.Log("Remove trophy");
            //    SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECLAIM).MoveOutToRight();
            //    GameManager.Instance.OnAbilityRemovedSelected(m_CurrentSelected);            
            //} else 
            {
                SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTPIECECLAIM).MoveOutToRight();
                GameManager.Instance.OnTrophyClaimSelected(m_CurrentSelected);
            }
        }
    }

    public void SetButtonText(string s)
    {
        m_ButtonText.text = s;
    }

    
}
