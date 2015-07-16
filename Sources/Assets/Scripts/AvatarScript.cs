using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AvatarScript : MonoBehaviour {

    //{"C", "G", "P", "H", "E", "N", "L", "F", "S"};

    int[] t_offsetX = {0, 8,  0, 0, 0, 10, 30, 0, 0};
    int[] t_offsetY = {0, 52, 0, 0, 0, 44, 36, 0, 0};

    int[] c_offsetX = {0, 18, 0, 0, 0, 0, 0, 0,  0};
    int[] c_offsetY = {0, 38, 0, 0, 0, 32, 28, 0, 0};

    public Image[] m_ImageList;
    public Image m_QuestionImage;

    public Image m_BackgroundImage;
    public Sprite[] m_BackgroundSprite;

    public Text m_NameText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInfo(Avatar avatar, bool isMe)
    {
        for (int i = 0; i < m_ImageList.GetLength(0); i++)
        {
            m_ImageList[i].gameObject.SetActive(true);
            m_QuestionImage.gameObject.SetActive(false);
        }

        string sex = "";
        if (avatar.m_Sex == 1)
        {
            sex = "G";
        }
        else
        {
            sex = "B";
        }

        string tier = ((int)avatar.m_Tier).ToString();
        while (tier.Length < 2) {
            tier = "0" + tier;
        }

        string job = "";
        switch (avatar.m_Jobs)
        {
            case CLASS.Medicial:
                job = "_D";
                break;
            case CLASS.Scientist:                
                job = "_S";
                break;
            case CLASS.Athlete:                
                job = "_A";
                break;
            case CLASS.Enterpreneur:                
                job = "_E";
                break;
            case CLASS.Warrior:                
                job = "_W";
                break;
            case CLASS.Musician:                
                job = "_M";
                break;
            default:
                break;
        }

        string sbody = sex + "_B" + tier;        
        m_ImageList[0].sprite = Resources.Load<Sprite>("avatar/default/" + sbody);
        
        for (int i = 0; i < avatar.m_ItemList.Count; i++)
        {
            m_ImageList[i + 1].gameObject.SetActive(false);
            if (avatar.m_ItemList[i] == 0)
            {
                string file1 = "avatar/default/" + sex + "_" + Avatar.AVATAR_PREFIX[i] + tier;
                string file2 = "avatar/default/" + "U" + "_" + Avatar.AVATAR_PREFIX[i] + tier;
                string file3 = file1 + job;
                string file4 = file2 + job;
                //if ((i == 0 || i == 3 || i == 1) && ((int)avatar.m_Tier >= 5 && (int)avatar.m_Tier <= 9))
                //{
                    //TODO: change according actuall jobs
                    //file = file + job;
                    //fileU = fileU + job;
                //}

                m_ImageList[i + 1].sprite = Resources.Load<Sprite>(file3);
                if (m_ImageList[i + 1].sprite == null)
                {
                    m_ImageList[i + 1].sprite = Resources.Load<Sprite>(file4);
                }
                if (m_ImageList[i + 1].sprite == null)
                {
                    m_ImageList[i + 1].sprite = Resources.Load<Sprite>(file1);
                }
                if (m_ImageList[i + 1].sprite == null)
                {
                    m_ImageList[i + 1].sprite = Resources.Load<Sprite>(file2);
                }
                if (m_ImageList[i + 1].sprite == null)
                {
                    //Debug.Log("Cant load: " + file);
                    m_ImageList[i + 1].gameObject.SetActive(false);
                }
                else
                {
                    //Debug.Log(file);
                    m_ImageList[i + 1].gameObject.SetActive(true);
                }
            }
            else
            {
                if (avatar.m_Tier == TIER.Toddler)
                {
                    if (i == 0 || i == 3 || i == 7 || i == 2)
                    {
                        continue;
                    }
                }
                if (avatar.m_Tier == TIER.Child)
                {
                    if (i == 0 || i == 3 || i == 7)
                    {
                        continue;
                    }
                }

                string ss = avatar.m_ItemList[i].ToString();
                while (ss.Length < 2)
                {
                    ss = "0" + ss;
                }
                ss = Avatar.AVATAR_PREFIX[i] + ss;
                if (i == 0)
                {
                    if (avatar.m_Tier == TIER.Teenager)
                    {
                        ss = "B" + "_" + ss;
                    }
                    else
                    {
                        ss = sex + "_" + ss;
                    }
                }
                else
                {
                    ss = "U_" + ss;
                }

                //Debug.Log("avatar/IAP/" + ss);
                Sprite sprite = Resources.Load<Sprite>("avatar/IAP/" + ss);

                m_ImageList[i + 1].sprite = sprite;
                if (m_ImageList[i + 1].sprite == null)
                {
                    m_ImageList[i + 1].gameObject.SetActive(false);
                }
                else
                {
                    m_ImageList[i + 1].gameObject.SetActive(true);
                }

                if (avatar.m_Tier == TIER.Toddler)
                {
                    //m_ImageList[i + 1].GetComponent<RectTransform>().anchoredPosition = m_ImageList[0].GetComponent<RectTransform>().anchoredPosition + new Vector2(t_offsetX[i], t_offsetY[i]);
                }
                else if (avatar.m_Tier == TIER.Child)
                {
                    //m_ImageList[i + 1].GetComponent<RectTransform>().anchoredPosition = m_ImageList[0].GetComponent<RectTransform>().anchoredPosition + new Vector2(c_offsetX[i], c_offsetY[i]);
                }
                else
                {
                    //m_ImageList[i + 1].GetComponent<RectTransform>().anchoredPosition = m_ImageList[0].GetComponent<RectTransform>().anchoredPosition;
                }
            }
        }

        //TODO: jobs speicial item
        if (((int)avatar.m_Tier >= 5 && (int)avatar.m_Tier <= 9))
        {
            m_ImageList[m_ImageList.GetLength(0) - 2].sprite = Resources.Load<Sprite>("avatar/default/I" + job);
            if (m_ImageList[m_ImageList.GetLength(0) - 2].sprite == null)
            {
                m_ImageList[m_ImageList.GetLength(0) - 2].gameObject.SetActive(false);
            }
            else
            {
                m_ImageList[m_ImageList.GetLength(0) - 2].gameObject.SetActive(true);
            }
        }
        else
        {
            m_ImageList[m_ImageList.GetLength(0) - 2].gameObject.SetActive(false);
        }


        string sitem = "avatar/default/" + sex + "_S" + tier + job;
        m_ImageList[m_ImageList.GetLength(0) - 1].sprite = Resources.Load<Sprite>(sitem);
        if (m_ImageList[m_ImageList.GetLength(0) - 1].sprite == null)
        {
            sitem = "avatar/default/" + "U" + "_S" + tier + job;
            m_ImageList[m_ImageList.GetLength(0) - 1].sprite = Resources.Load<Sprite>(sitem);
        }

        if (m_ImageList[m_ImageList.GetLength(0) - 1].sprite == null)
        {
            m_ImageList[m_ImageList.GetLength(0) - 1].gameObject.SetActive(false);
        }
        else
        {
            m_ImageList[m_ImageList.GetLength(0) - 1].gameObject.SetActive(true);
        }

        if (isMe)
        {
            m_BackgroundImage.sprite = m_BackgroundSprite[0];
        }
        else
        {
            m_BackgroundImage.sprite = m_BackgroundSprite[1];
        }
    }

    public void SetIsUnkonw()
    {
        for (int i = 0; i < m_ImageList.GetLength(0); i++)
        {
            m_ImageList[i].gameObject.SetActive(false);
        }
        m_QuestionImage.gameObject.SetActive(true);
    }

    public void ShowName(bool show)
    {
        m_NameText.gameObject.SetActive(false);
    }
}
