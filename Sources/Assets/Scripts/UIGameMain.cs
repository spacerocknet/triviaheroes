using UnityEngine;
using System.Collections;

public class UIGameMain : MonoBehaviour {

    public GameObject m_MainCanvas;
    public GameObject m_Board;
    public bool m_IsSpinning = false;
    float m_MoveTime;
    float m_MoveDuration = 6;
    Vector3 m_StartRotation;
    Vector3 m_EndRotation;
    RectTransform m_Rect;


	// Use this for initialization
	void Start () {
        m_Rect = m_Board.GetComponent<RectTransform>();
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
                Debug.Log("SPINNING DONE");
                m_IsSpinning = false;
                m_Rect.localEulerAngles = m_EndRotation;
                int idx = (int)(m_Rect.localEulerAngles.z) / 60;
                CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_GOTPUZZLE);
                cv.MoveInFromRight();
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
}
