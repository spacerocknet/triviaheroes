using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectPieceClaim : MonoBehaviour {

    public Image[] m_TrophyImage;
    public Sprite[] m_TrophySprite;
    public Image[] m_CheckImage;
    int m_CurrentSelected = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateTrophyState()
    {
        int[] trophy = GameManager.Instance.GetCurrentTrophyState();
        for (int i = 0; i < 6; i++)
        {
            if (trophy[i] == 0)
            {
                m_TrophyImage[i].sprite = m_TrophySprite[i];
                
            }
            else
            {
                m_TrophyImage[i].sprite = m_TrophySprite[6 + i];                
            }
            m_CheckImage[i].enabled = false;
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
            GameManager.Instance.OnTrophyClaimSelected(m_CurrentSelected);
        }
    }
}
