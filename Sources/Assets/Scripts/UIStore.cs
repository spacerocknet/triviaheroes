using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UIStore : MonoBehaviour {

    public UpgradesPage m_UpgradePages;

    public StoreTab m_StoreTab;

	// Use this for initialization
	void Start () {
	    if (Advertisement.isSupported) {
            if (Application.platform == RuntimePlatform.Android)
            {
                Advertisement.Initialize("55499");
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Advertisement.Initialize("55499");
            }
        } else {
            Debug.Log("Platform not supported");
        }
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
        cs.GetComponent<UIPopup>().Show("Purchase successfull. " + amount[pack - 1] + " diamond added to your account", 0, null, null, (int)CanvasID.CANVAS_STORE);                                       
            
        GameManager.Instance.AddDiamond(amount[pack - 1]);
    }

    public void OnWatchVideo()
    {
        Advertisement.Show(null, new ShowOptions
        {
            resultCallback = result =>
            {
                Debug.Log(result.ToString());
                if (result.ToString() == "Finished")
                {
                    CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
                    cs.GetComponent<UIPopup>().Show("Watch Video successfull. " + 15 + " diamond added to your account", 0, null, null, (int)CanvasID.CANVAS_STORE);
                    GameManager.Instance.AddDiamond(15);
                }
            }
        });
        //CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_POPUP);
        //cs.GetComponent<UIPopup>().Show("Watch Video successfull. " + 15 + " diamond added to your account", 0, null, null, (int)CanvasID.CANVAS_STORE);
        //GameManager.Instance.AddDiamond(15);
    }

    public void OnExchange()
    {
        CanvasScript cs = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_EXCHANGE);
        cs.Show();
        cs.GetComponent<UIExchange>().SetInfo(GameManager.Instance.GetPlayerProfile().m_Diamond, GameConfig.Instance.GetExchangeRate());
    }

    public void ShowUpgradeTab()
    {
        m_StoreTab.OnUpgrade();
    }

    public void ShowShopTab()
    {
        m_StoreTab.OnCurrency();
    }
}
