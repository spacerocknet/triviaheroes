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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInfo(Avatar avatar)
    {
        
        string sex = "";
        if (avatar.m_Sex == 0)
        {
            Debug.Log("Girl");
            sex = "G";
        }
        else
        {
            Debug.Log("Boy");
            sex = "B";
        }
        string s = ((int)avatar.m_Tier).ToString();
        while (s.Length < 2) {
            s = "0" + s;
        }
        s = sex + "_B" + s;

        //Debug.Log("avatar/default/" + s);
        m_ImageList[0].sprite = Resources.Load<Sprite>("avatar/default/" + s);
        
        for (int i = 0; i < avatar.m_ItemList.Count; i++)
        {
            m_ImageList[i + 1].gameObject.SetActive(false);


            string stier = ((int)avatar.m_Tier).ToString();
            while (stier.Length < 2) {
                stier = "0" + stier;
            }
            if (avatar.m_ItemList[i] == 0)
            {
                string file = "avatar/default/" + sex + "_" + Avatar.AVATAR_PREFIX[i] + stier;
                if ((i == 0 || i == 3) && ((int)avatar.m_Tier >= 5 && (int)avatar.m_Tier <= 9))
                {
                    //TODO: change according actuall jobs
                    file = file + "_D";
                }


                m_ImageList[i + 1].sprite = Resources.Load<Sprite>(file);
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
                    ss = sex + "_" + ss;
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
            m_ImageList[m_ImageList.GetLength(0) - 1].sprite = Resources.Load<Sprite>("avatar/default/I_" + "D");
            if (m_ImageList[m_ImageList.GetLength(0) - 1].sprite == null)
            {
                m_ImageList[m_ImageList.GetLength(0) - 1].gameObject.SetActive(false);
            }
            else
            {
                m_ImageList[m_ImageList.GetLength(0) - 1].gameObject.SetActive(true);
            }
        }
        else
        {
            m_ImageList[m_ImageList.GetLength(0) - 1].gameObject.SetActive(false);
        }
    }
}
