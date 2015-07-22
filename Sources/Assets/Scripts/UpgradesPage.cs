using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradesPage : MonoBehaviour {

    public GameObject[] m_TierList;

    public Sprite m_ProgressBarEnable;
    public Sprite m_ProgressBarDisabled;
    public Sprite m_CircleEnabled;
    public Sprite m_CircleDisabled;

    public HorizontalScrollSnap m_TierPageView;

    public AvatarScript m_AvatarScript;
    public Text m_Title;
    public Text m_Description;
    public Text m_AbilityText;

    public Button m_NextButton;
    public Button m_BackButton;
    public Button m_UpgradeButton;
    public Text m_UpgradeCostText;

    public Image m_CoinImage;



    private int m_ClassID;
    private int m_Tier;

    private static string[] m_TierDescription = {
        "The potential is strong in this one.",
        "Still building a strong foundation. +1% payout boost.",
        "Still needs more training. +1% payout boost.",
        "Training is almost complete; just needs more experience. +1% payout boost.",
        "You are now a full fledged Trivia Hero.",
        "Even Heroes need to sharpen their skills. +1% payout boost.",
        "Practice makes perfect.",
        "Working towards achieving mastery. +1% payout boost.",
        "Sharpen your abilities through peer competition.",
        " Elders are masters in all subjects.",
        "Train a new generation."
    };

    private static string[] m_ClassDescription = {
        "Medical professionals can always be trusted to do the right thing, even if the decision is tough.",
        "Scientists seek to understand life’s mysteries and live to explore everything.",
        "Athletes live for the thrill of the moment and are adventurous and outgoing.",
        "Entrepreneurs posses a natural exuberance and want to make a difference in the world. +Switch puzzle set ability.",
        "Warriors are defined by their ability to survive against any opposition.",
        "Musicians are overflowing with natural talent and can turn any fantasy into reality",        
    };

    private static string[] m_ClassBonusText = {
        "+Free claim ability",
        "+Undo ability move",
        "+Free challenge ability",
        "+Switch puzzle set ability",
        "+Remove puzzle ability",
        "+Copy ability",        
    };

    private static string[] m_TierText = {
        "Toodler",
        "Child",
        "Teenager",
        "Younger",
        "Adult 1",
        "Adult 2",
        "Adult 3", 
        "Adult 4",
        "Adult 5",
        "Elder",
        "Train a new generation"
    };
    
    private static string[] m_ClassText = {
        "Medical",
        "Scientist",
        "Athlete",
        "Enterpreneur",
        "Warrior",
        "Musician"
    };

    int m_UpgradeCost;

	// Use this for initialization
	void Start () {
        m_TierPageView.SetItemCallBack(OnTierSelected, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Refresh()
    {        
        Avatar v = GameManager.Instance.GetMyActiveAvatar();
        int tier = (int)v.m_Tier;
        for (int i = 0; i < 11; i++)
        {
            Image imgBar = m_TierList[i].transform.FindChild("Image 1").GetComponent<Image>();
            Image imgCircle = m_TierList[i].transform.FindChild("Image").GetComponent<Image>();

            if (i < tier)
            {
                imgCircle.sprite = m_CircleEnabled;
            }
            else
            {
                imgCircle.sprite = m_CircleDisabled;
            }

            if (i < tier)
            {
                imgBar.sprite = m_ProgressBarEnable;
            }
            else
            {
                imgBar.sprite = m_ProgressBarDisabled;
            }

            if (i == tier)
            {
                m_TierList[i].transform.FindChild("Image/Glow").gameObject.SetActive(true);
            }
            else
            {
                m_TierList[i].transform.FindChild("Image/Glow").gameObject.SetActive(false);
            }
        }

        Avatar avatar = GameManager.Instance.GetActiveAvatar();        
        Debug.Log((int)avatar.m_Tier);
        m_TierPageView.ScrollTo((int)avatar.m_Tier);
        m_ClassID = (int)avatar.m_Jobs - 1;
        OnTierSelected(0, (int)avatar.m_Tier);
    }

    public void OnUpgrade()
    {
        if (GameManager.Instance.GetPlayerProfile().m_Coin > m_UpgradeCost)
        {
            if (m_Tier == 4)
            {
                SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SELECTCAREER).GetComponent<UISelectCareer>().Show();
            }
            else if (m_Tier < 10)
            {
                GameManager.Instance.UpgradeTier((CLASS)(m_ClassID + 1));
            }
            else
            {
                GameManager.Instance.GetPlayerProfile().AddNewAvatar();
            }
            Refresh();
        }
        else
        {
            CanvasScript css = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
            css.GetComponent<UIPopup>().Show("Insufficient coin, do you want to exchange diamond?", 0, GameManager.Instance.ShowExchangePopup, null, (int)CanvasID.CANVAS_STORE);
        }
    }

    public void OnNextClass()
    {
        Debug.Log("Next");
        m_ClassID++;
        m_ClassID = Mathf.Clamp(m_ClassID, 0, 5);
        OnTierSelected(0, m_Tier);
    }

    public void OnBackClass()
    {
        Debug.Log("Back");
        m_ClassID--;
        m_ClassID = Mathf.Clamp(m_ClassID, 0, 5);
        OnTierSelected(0, m_Tier);
    }

    public void OnTierSelected(int cat, int tier)
    {
        if (tier > (int)GameManager.Instance.GetActiveAvatar().m_Tier || (tier == 4 && (int)GameManager.Instance.GetActiveAvatar().m_Tier == 4))
        {
            m_AvatarScript.SetIsUnkonw();
        }
        else
        {
            Avatar ava = Avatar.CreateDefaultAvatar();
            ava.m_Tier = (TIER)(tier + 1);
            ava.m_Jobs = (CLASS)(m_ClassID + 1);
            ava.m_Sex = GameManager.Instance.GetPlayerProfile().m_Sex;
            m_AvatarScript.SetInfo(ava, true);
        }

        m_Tier = tier;

        m_Title.text = m_TierText[tier];

        if (tier >= 4 && tier <= 8)
        {
            Debug.Log(m_ClassID);
            if (m_ClassID != -1)
            {
                m_Description.text = m_TierDescription[tier] + " " + m_ClassDescription[m_ClassID];
                m_Title.text = m_TierText[tier] + " " + m_ClassText[m_ClassID];
                if (tier == 4 || tier == 6 || tier == 8)
                {
                    m_AbilityText.text = m_ClassBonusText[m_ClassID];
                }
                else
                {
                    m_AbilityText.text = "";
                }
            }
            else
            {
                m_Description.text = m_TierDescription[tier];
                m_Title.text = m_TierText[tier];
                m_AbilityText.text = "";
            }
        }
        else
        {
            m_Description.text = m_TierDescription[tier];
            m_Title.text = m_TierText[tier];
            m_AbilityText.text = "";
        }

        m_UpgradeCost = 500;
        Avatar avatar = GameManager.Instance.GetActiveAvatar();
        if (m_Tier < 10)
        {
            m_UpgradeCostText.text = GameConfig.Instance.GetUpgradeCost(m_Tier, avatar.m_ID).ToString();
            m_UpgradeCost = GameConfig.Instance.GetUpgradeCost(m_Tier, avatar.m_ID);
        }
        else
        {
            m_UpgradeCostText.text = "500";
        }
        if ((int)avatar.m_Tier == m_Tier)
        {
            m_UpgradeButton.interactable = true;
            m_UpgradeButton.gameObject.SetActive(true);
            if (GameManager.Instance.GetPlayerProfile().m_Coin > m_UpgradeCost)
            {
                m_CoinImage.color = Color.white;
            }
            else
            {
                m_CoinImage.color = Color.red;
            }
        }
        else
        {
            m_UpgradeButton.interactable = false;
            m_UpgradeButton.gameObject.SetActive(false);
        }

    }

    void UpdateNavigateButton() {
        if (m_Tier >= 4 && m_Tier <= 8)
        {
            m_BackButton.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(true);
        }
        else
        {
            m_BackButton.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
        }

        if (m_ClassID == 0)
        {
            m_BackButton.gameObject.SetActive(false);
        }
        if (m_ClassID == 5)
        {
            m_NextButton.gameObject.SetActive(false);
        }
    }
}
