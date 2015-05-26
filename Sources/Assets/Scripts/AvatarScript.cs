using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AvatarScript : MonoBehaviour {

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
            sex = "B";
        }
        else
        {
            sex = "G";
        }
        string s = ((int)avatar.m_Tier).ToString();
        while (s.Length < 2) {
            s = "0" + s;
        }
        s = sex + "_B" + s;        

        m_ImageList[0].sprite = Resources.Load<Sprite>("avatar/default/" + s);
        
        for (int i = 0; i < avatar.m_ItemList.Count; i++)
        {
            string stier = ((int)avatar.m_Tier).ToString();
            while (stier.Length < 2) {
                stier = "0" + stier;
            }
            if (avatar.m_ItemList[i] == 0)
            {
                string file = "avatar/default/" + sex + "_" + Avatar.AVATAR_PREFIX[i] + stier;
                if ((i == 0 || i == 3) && ((int)avatar.m_Tier >= 5 && (int)avatar.m_Tier <=9))
                {                    
                    //TODO: change according actuall jobs
                    file = file + "_D";
                }
                m_ImageList[i + 1].sprite = Resources.Load<Sprite>(file);
                if (m_ImageList[i + 1].sprite == null)
                {
                    m_ImageList[i + 1].gameObject.SetActive(false);
                }
                else
                {
                    m_ImageList[i + 1].gameObject.SetActive(true);
                }
            }
            else
            {
                string ss = avatar.m_ItemList[i].ToString();
                while (ss.Length < 2)
                {
                    ss = "0" + ss;
                }
                ss = Avatar.AVATAR_PREFIX[i] + ss;
                if (i == 0)
                {
                    ss = "B_" + ss;
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
