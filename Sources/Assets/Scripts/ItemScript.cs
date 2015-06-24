using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemScript : MonoBehaviour {

    public Image m_Image;
    public Button m_BuyButton;
    private int m_Type;
    private int m_ID;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInfo(int type, int id, bool isShop, bool canBuy)
    {
        m_Type = type;
        m_ID = id;

        PlayerProfile profile = GameManager.Instance.GetPlayerProfile();

        if (profile == null)
        {
            return;
        }
        if (m_Type == -1)
        {
            m_Image.sprite = Resources.Load<Sprite>("avatar/blank");
        }
        else
        {
            if (id == 0)
            {
                string s = ((int)profile.m_CurrentTier).ToString();
                while (s.Length < 2)
                {
                    s = "0" + s;
                }
                s = Avatar.AVATAR_PREFIX[type] + s;
                if (profile.m_Sex == 1)
                {
                    s = "B_" + s;
                }
                else
                {
                    s = "G_" + s;
                }

                
                if (isShop)
                {
                    Sprite sprite = Resources.Load<Sprite>("avatar/default/" + s + "_I");
                    m_Image.sprite = sprite;
                }
                else
                {
                    string ss = "00";
                    ss = Avatar.AVATAR_PREFIX[type] + ss;                    
                    ss = "U_" + ss;                    
                    
                    Sprite sprite = Resources.Load<Sprite>("avatar/IAP/" + ss + "_I");
                    m_Image.sprite = sprite;
                }
            }
            else
            {
                string s = id.ToString();
                while (s.Length < 2)
                {
                    s = "0" + s;
                }
                s = Avatar.AVATAR_PREFIX[type] + s;
                if (type == 0)
                {
                    s = "B_" + s;
                }
                else
                {
                    s = "U_" + s;
                }
                Sprite sprite = Resources.Load<Sprite>("avatar/IAP/" + s + "_I");
                m_Image.sprite = sprite;
            }
        }

        if (canBuy)
        {
            m_BuyButton.gameObject.SetActive(true);

            PlayerProfile pl = GameManager.Instance.GetPlayerProfile();

            bool owned = false;
            for (int i = 0; i < pl.m_ItemCat.Count; i++)
            {
                if (pl.m_ItemCat[i] == m_Type && pl.m_ItemID[i] == m_ID)
                {
                    owned = true;
                    break;
                }
            }

            if (owned)
            {
                m_BuyButton.interactable = false;
                m_BuyButton.transform.FindChild("Text").GetComponent<Text>().text = "Owned";
            }
            else
            {
                m_BuyButton.interactable = true;
                m_BuyButton.transform.FindChild("Text").GetComponent<Text>().text = "$0.99";
            }
        }
        else
        {
            m_BuyButton.gameObject.SetActive(false);
        }
    }

    public void OnBuy()
    {
        Debug.Log("Buy: " + m_Type + " " + m_ID);

        PlayerProfile pl = GameManager.Instance.GetPlayerProfile();

        pl.m_ItemCat.Add(m_Type);
        pl.m_ItemID.Add(m_ID);

        pl.Save();

        m_BuyButton.interactable = false;
        m_BuyButton.transform.FindChild("Text").GetComponent<Text>().text = "Owned";
    }
}
