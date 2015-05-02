using UnityEngine;
using System.Collections;

public class UICrownSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClaim()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_CROWNSELECT);
        cs.MoveOutToRight();
        GameManager.Instance.OnSelectClaim();
    }

    public void OnChallenge()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_CROWNSELECT);
        cs.MoveOutToRight();
        GameManager.Instance.OnSelectChallenge();
    }
}
