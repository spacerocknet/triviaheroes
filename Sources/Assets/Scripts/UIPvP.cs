using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPvP : MonoBehaviour {

    public GameObject m_MainCanvas;
    public GameObject m_Board;
    public bool m_IsSpinning = false;
    float m_MoveTime;
    float m_MoveDuration = 6;
    Vector3 m_StartRotation;
    Vector3 m_EndRotation;
    RectTransform m_Rect;

    public Text m_RoundText;
    public Text m_MyAvatarName;
    public Text m_OpponentAvatarName;
    public TrophyGroup m_TrophyGroup1;
    public TrophyGroup m_TrophyGroup2;

    public Sprite[] m_PivotSprite;
    public Image m_PivotImage;
    private int m_Progress;

    private Category[] m_CategoryMap;
	// Use this for initialization
	void Start () {
        m_Rect = m_Board.GetComponent<RectTransform>();
        m_CategoryMap = new Category[7];
        m_CategoryMap[0] = Category.CAT_GEOGRAPHY;
        m_CategoryMap[1] = Category.CAT_CROWN;
        m_CategoryMap[2] = Category.CAT_ENTERTAINMENT;
        m_CategoryMap[3] = Category.CAT_ART;
        m_CategoryMap[4] = Category.CAT_SPORT;
        m_CategoryMap[5] = Category.CAT_HISTORY;
        m_CategoryMap[6] = Category.CAT_SCIENCE;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_IsSpinning)
        {
            m_MoveTime += Time.deltaTime;
            if (m_MoveTime < m_MoveDuration)
            {
                float d = Mathf.SmoothStep(0, 1, m_MoveTime * (1 / m_MoveDuration));
                m_Rect.localEulerAngles = m_StartRotation + (m_EndRotation - m_StartRotation) * d;
            }
            else
            {
                
                m_IsSpinning = false;
                m_Rect.localEulerAngles = m_EndRotation;

                int cat = Mathf.FloorToInt(m_Rect.localEulerAngles.z / (360f / 7));

                cat = 1;

                if (m_CategoryMap[cat] == Category.CAT_CROWN)
                {
                    GameManager.Instance.FulfillProgress();
                    UpdateProgress(3);
                   
                    OnFullProgress();
                }
                else
                {
                    CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_GOTPUZZLE);
                    cv.gameObject.GetComponent<UIGotPuzzle>().SetCategory(m_CategoryMap[cat]);
                    cv.MoveInFromRight();
                }                
            }
        }
	}

    public void Spin()
    {
        m_IsSpinning = true;
        m_StartRotation = m_Rect.localEulerAngles;
        m_EndRotation = m_Rect.localEulerAngles + new Vector3(0, 0, Random.RandomRange(360 * 5, 360 * 6));
        m_MoveTime = 0;
    }

    public void OnBack()
    {
        CanvasScript cs = gameObject.GetComponent<CanvasScript>();
        cs.MoveOutToRight();

        cs = m_MainCanvas.GetComponent<CanvasScript>();
        cs.MoveInFromLeft();
    }

    public void SetGameInfo(GameInfo game)
    {
        m_RoundText.text = "Round " + game.m_Round.ToString();
        m_MyAvatarName.text = GameManager.Instance.GetPlayerProfile().m_PlayerName;
        m_OpponentAvatarName.text = game.m_PlayerB;
        m_TrophyGroup1.SetTrophyState(game.m_PieceA);
        m_TrophyGroup2.SetTrophyState(game.m_PieceB);
        UpdateProgress(game.m_SpinProgressA);
    }

    public void FullFillProgressBar()
    {
        m_PivotImage.sprite = m_PivotSprite[3];
    }

    public void UpdateProgress(int prog)
    {
        m_PivotImage.sprite = m_PivotSprite[prog];

        if (prog == 3)
        {
            OnFullProgress();
        }
    }

    public void OnFullProgress()
    {
        GameManager.Instance.OnFullProgress();
    }
}
