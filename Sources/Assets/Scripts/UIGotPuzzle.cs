using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGotPuzzle : MonoBehaviour {

    public GameObject m_GameMainCanvas;
    public Image m_CategoryImage;
    public Sprite[] m_CatSprite;
    Category m_CurrentCat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnRespin()
    {
        CanvasScript cv = gameObject.GetComponent<CanvasScript>();
        cv.MoveOutToRight((int)CanvasID.CANVAS_PVP);
        UIPvP gm = m_GameMainCanvas.GetComponent<UIPvP>();
        gm.Respin();
    }

    public void OnPlay()
    {
        CanvasScript cv = gameObject.GetComponent<CanvasScript>();
        cv.MoveOutToRight();
        GameManager.Instance.OnCategoryConfirmToPlay(m_CurrentCat);
    }

    public void SetCategory(Category category)
    {
        m_CategoryImage.sprite = m_CatSprite[(int)category];
        m_CurrentCat = category;
    }
}

