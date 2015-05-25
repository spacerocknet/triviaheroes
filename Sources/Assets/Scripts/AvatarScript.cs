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

    public void SetInfo(int tier, int job, List<int> item, string _sex)
    {
        string sex = "B";
        string s = tier.ToString();
        while (s.Length < 2) {
            s = "0" + s;
        }
        s = sex + "_B" + s;
        m_ImageList[0].sprite = Resources.Load<Sprite>("avatar/default/" + s);
        
        for (int i = 0; i < item.Count; i++)
        {
            string stier = tier.ToString();
            while (stier.Length < 2) {
                stier = "0" + stier;
            }
            if (item[i] == 0)
            {
                m_ImageList[i + 1].sprite = Resources.Load<Sprite>("avatar/default/" + sex + "_" + Avatar.AVATAR_PREFIX[i] + stier);
                if (m_ImageList[i + 1].sprite == null)
                {
                    m_ImageList[i + 1].gameObject.SetActive(false);
                }
            }
            else
            {
                string ss = item[i].ToString();
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
    }
}
