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

    private int m_ClassID;
    private int m_Tier;

    private static string[] m_TierDescription = {
        "No one is born a Trivia Hero, but the potential is strong in this one. +1% payout boost.",
        "In the child phase we build the foundation to conquer future challenges. +1% payout boost.",
        "Teenagers must channel their youthful energy and complete secondary education before they are ready for the world. +1% payout boost.",
        "Training is almost complete; the young adult just needs more experience. +1% payout boost.",
        "Disciplined and experienced, you are now a full fledged Trivia Hero.",
        "Even Heroes need to consistently sharpen their skills. +1% payout boost",
        "Perfect your abilities with experience and practice.",
        "It’s a long road to achieving mastery but Trivia Heroes will see it through. +1% payout boost",
        "Sharpen your abilities through peer competition.",
        "Elders exhibit true mastery over all subjects"
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
        "Elder"
    };
    
    private static string[] m_ClassText = {
        "Medical",
        "Scientist",
        "Athlete",
        "Enterpreneur",
        "Warrior",
        "Musician"
    };

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
        for (int i = 0; i < 10; i++)
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

            if (i < tier - 1)
            {
                imgBar.sprite = m_ProgressBarEnable;
            }
            else
            {
                imgBar.sprite = m_ProgressBarDisabled;
            }
        }
        
        OnTierSelected(0, 0);
    }

    public void OnUpgrade()
    {
        GameManager.Instance.UpgradeTier();
        Refresh();
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
        Avatar ava = Avatar.CreateDefaultAvatar();
        ava.m_Tier = (TIER)(tier + 1);
        m_AvatarScript.SetInfo(ava);

        m_Tier = tier;

        m_Title.text = m_TierText[tier];

        if (tier >= 4 && tier <= 8)
        {
            m_Description.text = m_TierDescription[tier] + " " + m_ClassDescription[m_ClassID];
            m_Title.text = m_TierText[tier] + " " + m_ClassText[m_ClassID];
            m_AbilityText.text = m_ClassBonusText[m_ClassID];
            m_BackButton.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(true);
        }
        else
        {
            m_Description.text = m_TierDescription[tier];
            m_Title.text = m_TierText[tier];
            m_AbilityText.text = "";
            m_ClassID = 0;
            m_BackButton.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
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
