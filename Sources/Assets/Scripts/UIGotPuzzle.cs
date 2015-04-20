using UnityEngine;
using System.Collections;

public class UIGotPuzzle : MonoBehaviour {

    public GameObject m_GameMainCanvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnRespin()
    {
        CanvasScript cv = gameObject.GetComponent<CanvasScript>();
        cv.MoveOutToRight();
        UIGameMain gm = m_GameMainCanvas.GetComponent<UIGameMain>();
        gm.Spin();
    }

    public void OnPlay()
    {
        CanvasScript cv = gameObject.GetComponent<CanvasScript>();
        cv.MoveOutToRight();
        cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_QUESTION);
        cv.MoveInFromRight();
    }
}
