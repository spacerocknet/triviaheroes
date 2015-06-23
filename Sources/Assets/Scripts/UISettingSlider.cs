using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISettingSlider : MonoBehaviour {

    public Image m_AlertImage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBack()
    {
        //Debug.Log("Back");
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveOutToLeftFar();
    }

    public void OnStore()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveOutToLeft();
        cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_STORE);
        cv.MoveInFromRight();
        
        
        if (m_AlertImage.gameObject.activeInHierarchy)
        {
            
            cv.GetComponent<UIStore>().ShowUpgradeTab();
            GameManager.Instance.ShowHelpUpgrade(false);
            GameManager.Instance.OnFirstTimeUserExperienceComplete(2);
        }

        cv.GetComponent<UIStore>().Refresh();
    }

    public void OnProfile() 
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveOutToLeft();
        cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PROFILE);
        cv.MoveInFromRight();
        cv.GetComponent<UIProfile>().Refresh();
    }

    public void ShowAlertImage(bool value)
    {
        m_AlertImage.gameObject.SetActive(value);
    }
}
