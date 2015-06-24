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
        Debug.Log("ON CHALLENGE");
        if (GameManager.Instance.CanChallenge())
        {
            Debug.Log("CAN CHALLENGE");
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_CROWNSELECT);
            cs.MoveOutToRight();
            GameManager.Instance.OnSelectChallenge();
        }
        else
        {
            Debug.Log("CANT CHALLENGE");
            CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_CROWNSELECT);
            cs.GetComponent<UIPopup>().Show(TextManager.Instance.GetTextByKey("CantChallenge"), 0, null, null, (int)CanvasID.CANVAS_CROWNSELECT);                                       
        }
    }
}
