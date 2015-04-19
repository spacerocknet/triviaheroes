using UnityEngine;
using System.Collections;

public class UIGotTrophy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnNext()
    {

        //CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PVP);
        CanvasScript cv = gameObject.GetComponent<CanvasScript>();
        cv.MoveOutToRight();

        cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cv.MoveOutToRight();
    }
}
