using UnityEngine;
using System.Collections;

public enum CanvasID { CANVAS_MAIN = 0, CANVAS_NEWGAME, CANVAS_PVP, CANVAS_WAITING, CANVAS_GOTPUZZLE, CANVAS_GOTTROPHY, CANVAS_QUESTION, CANVAS_SETTING_SLIDER,
CANVAS_PROFILE, CANVAS_STORE, CANVAS_CROWNSELECT, CANVAS_SELECTPIECECLAIM, CANVAS_SELECTPIECECHALLENGE, CANVAS_POPUP, CANVAS_ENDGAMECONFIRM, 
    CANVAS_ENDGAMERESULT, CANVAS_EXCHANGE, CANVAS_OUTLIVES, CANVAS_HELP, CANVAS_SELECTCAREER
};

public class SceneManager : MonoBehaviour{

    public CanvasScript[] m_CanvasList;

    private static SceneManager m_sInstance = null;

    private bool m_bFirstUpdate = true;

    public void Awake()
    {
        m_sInstance = this;
    }

    private SceneManager()
    {
        m_sInstance = this;
    }

    public void Start()
    {

    }

    public static SceneManager Instance
    {
        get
        {
            if (m_sInstance == null)
            {
                m_sInstance = new SceneManager();
            }
            return m_sInstance;
        }
    }

    public CanvasScript GetCanvasByID(CanvasID id)
    {        
        return m_CanvasList[(int)id];
    }

    public void SetActiveScene(CanvasScript cs)
    {
        for (int i = 0; i < m_CanvasList.GetLength(0); i++)
        {
            if (m_CanvasList[i] != cs)
            {
                m_CanvasList[i].SetActive(false);
            }
        }
    }

    void Update() {

        if (m_bFirstUpdate) {
            for (int i = 0; i < m_CanvasList.GetLength(0); i++)
            {
                m_CanvasList[i].gameObject.SetActive(false);
                m_CanvasList[0].gameObject.SetActive(true);
            }
            m_bFirstUpdate = false;

            if (GameManager.Instance.GetPlayerProfile().m_FirstTimeExperience[0] == false)
            {
                GameManager.Instance.ShowHelpNewGame();
            }

            GameManager.Instance.CheckFirstUpgrade();
            GameManager.Instance.CheckFirstAdultUpgrade();
            GameManager.Instance.CheckFirstReborn();
        }
    }

}
