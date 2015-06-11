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

    public void OnBuyDiamondPack(int pack)
    {
        int[] amount = {50, 130, 275, 570, 1500, 4000};
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
        cs.Show((int)CanvasID.CANVAS_STORE);
        cs.GetComponent<UIPopup>().SetText("Purchase successfull. " + amount[pack - 1] + " diamond added to your account");

        GameManager.Instance.AddDiamond(amount[pack - 1]);
    }

    public void OnWatchVideo()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
        cs.Show((int)CanvasID.CANVAS_STORE);
        cs.GetComponent<UIPopup>().SetText("Watch Video successfull. " + 15 + " diamond added to your account");

        GameManager.Instance.AddDiamond(15);
    }
}
