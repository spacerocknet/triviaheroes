using UnityEngine;
using System.Collections;

public class UIQuestion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnAnswer1()
    {
        
    }

    public void OnAnswer2()
    {
    }

    public void OnAnswer3()
    {
    }

    public void OnAnswer4()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_GOTTROPHY);
        cv.MoveInFromRight();
    }
}
