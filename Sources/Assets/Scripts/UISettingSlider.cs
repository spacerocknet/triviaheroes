using UnityEngine;
using System.Collections;

public class UISettingSlider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBack()
    {
        Debug.Log("Back");
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_SETTING_SLIDER);
        cv.MoveOutToLeftFar();
    }
}
