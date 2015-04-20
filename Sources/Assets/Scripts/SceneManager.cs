using UnityEngine;
using System.Collections;

public enum CanvasID { CANVAS_MAIN = 0, CANVAS_NEWGAME, CANVAS_PVP, CANVAS_WAITING, CANVAS_GOTPUZZLE, CANVAS_GOTTROPHY, CANVAS_QUESTION, CANVAS_SETTING_SLIDER, CANVAS_CROWNSELECT, CANVAS_SELECT_TROPHY };

public class SceneManager : MonoBehaviour{

    public CanvasScript[] m_CanvasList;
    private static SceneManager m_sInstance = null;

    public void Awake()
    {
        m_sInstance = this;
    }

    private SceneManager()
    {
        m_sInstance = this;
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
        Debug.Log((int)id);
        return m_CanvasList[(int)id];
    }

}
