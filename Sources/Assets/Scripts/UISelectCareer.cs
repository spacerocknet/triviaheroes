using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectCareer : MonoBehaviour {

    CLASS m_SelectClass = CLASS.None;
    public GameObject[] m_CheckImage;
    public UpgradesPage m_UpgradePage;

    private static string[] m_ClassText = {
        "Medical",
        "Scientist",
        "Athlete",
        "Enterpreneur",
        "Warrior",
        "Musician"
    };

    private static string[] m_AbilityDes = {
        "Use a free claim.",
        "Undo an ability move.",
        "Use a free challenge.",
        "Switch puzzle sets.",
        "Remove a puzzle piece.",
        "Copy an ability."
    };

    private static string[] m_ClassUpgradeText = {
        "Great choice! Medic’s have the ability to use a free claim.",
        "Great choice! Scientist’s have the ability to undo an ability move.",
        "Great choice! Athlete’s have the ability to use a free challenge.",
        "Great choice! Entrepreneur’s have the ability to swap puzzle sets.",
        "Great choice! Warrior’s have the ability to remove an opponent’s puzzle piece.",
        "Great choice! Musician’s have the ability to copy your opponent’s ability."
    };

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnSelectMedical()
    {
        m_SelectClass = CLASS.Medicial;
        for (int i = 0; i < m_CheckImage.GetLength(0); i++)
        {
            m_CheckImage[i].SetActive(false);
        }
        m_CheckImage[0].SetActive(true);
    }

    public void OnSelectMusicial()
    {
        m_SelectClass = CLASS.Musician;
        for (int i = 0; i < m_CheckImage.GetLength(0); i++)
        {
            m_CheckImage[i].SetActive(false);
        }
        m_CheckImage[1].SetActive(true);
    }

    public void OnSelectWarrior()
    {
        m_SelectClass = CLASS.Warrior;
        for (int i = 0; i < m_CheckImage.GetLength(0); i++)
        {
            m_CheckImage[i].SetActive(false);
        }
        m_CheckImage[2].SetActive(true);
    }

    public void OnSelectEnterpreneur()
    {
        m_SelectClass = CLASS.Enterpreneur;
        for (int i = 0; i < m_CheckImage.GetLength(0); i++)
        {
            m_CheckImage[i].SetActive(false);
        }
        m_CheckImage[3].SetActive(true);
    }

    public void OnSelecScientist()
    {
        m_SelectClass = CLASS.Scientist;
        for (int i = 0; i < m_CheckImage.GetLength(0); i++)
        {
            m_CheckImage[i].SetActive(false);
        }
        m_CheckImage[4].SetActive(true);
    }

    public void OnSelecAthlete()
    {
        m_SelectClass = CLASS.Athlete;
        for (int i = 0; i < m_CheckImage.GetLength(0); i++)
        {
            m_CheckImage[i].SetActive(false);
        }
        m_CheckImage[5].SetActive(true);
    }

    public void OnSelect()
    {
        if (m_SelectClass != CLASS.None)
        {
            GameManager.Instance.UpgradeTier(m_SelectClass);
            GetComponent<CanvasScript>().Hide();
            if (GameManager.Instance.GetPlayerProfile().m_FirstTimeExperience[3] == false)
            {
                GameManager.Instance.OnFirstTimeUserExperienceComplete(3);
                GameManager.Instance.OnFirstTimeUserExperienceComplete(4);
                SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_STORE).MoveOutToRight();

                CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                cs.GetComponent<UIPopup>().Show(m_ClassUpgradeText[(int)m_SelectClass - 1], 0, null, null, (int)CanvasID.CANVAS_PVP);
            }
            else
            {
                m_UpgradePage.Refresh();
            }
        }
    }

    public void Show()
    {
        GetComponent<CanvasScript>().Show();
    }

    public void ShowNextPopup()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
        cs.GetComponent<UIPopup>().Show("Tap your avatar to activate it’s ability. Avatar’s will glow when abilities are ready for use.", 0, null, null, (int)CanvasID.CANVAS_MAIN); 
    }
}
