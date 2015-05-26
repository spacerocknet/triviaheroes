using UnityEngine;
using System.Collections;

public class UIStore : MonoBehaviour {

    public UpgradesPage m_UpgradePages;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBack()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_STORE);
        cv.MoveOutToRight();

        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_MAIN).GetComponent<UIMain>().Refresh();
    }

    public void Refresh()
    {
        m_UpgradePages.Refresh();
    }
}
