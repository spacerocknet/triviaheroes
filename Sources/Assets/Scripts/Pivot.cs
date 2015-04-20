using UnityEngine;
using System.Collections;

public class Pivot : MonoBehaviour {

    Vector3 m_TargetRotation;
    RectTransform m_Rect;

    UIGameMain m_UIGameMain;
    public GameObject m_UIMainGameCanvas;

	// Use this for initialization
    void Awake()
    {
        GameObject go = GameObject.Find("CanvasPvP"); 
        m_UIGameMain = go.GetComponent<UIGameMain>();
        Debug.Log(m_UIGameMain);
    }

	void Start () {
        m_Rect = transform.parent.GetComponent<RectTransform>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Rect.transform.localEulerAngles.z != m_TargetRotation.z)
        {
            float z = 0;
            if (m_TargetRotation.z != 0) {
                z = Mathf.MoveTowardsAngle(m_Rect.transform.localEulerAngles.z, m_TargetRotation.z, Time.deltaTime * 200);
            } else {
                z = Mathf.MoveTowardsAngle(m_Rect.transform.localEulerAngles.z, m_TargetRotation.z, Time.deltaTime * 50);
            }
            m_Rect.transform.localEulerAngles = new Vector3(m_Rect.transform.localEulerAngles.x, m_Rect.transform.localEulerAngles.y, z);
            if (Mathf.DeltaAngle(z, m_TargetRotation.z) == 0)
            {
                m_TargetRotation = new Vector3(0, 0, 0);
            }
            //Debug.Log(z + " " + m_TargetRotation.z);
        }
	}

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (m_UIGameMain.m_IsSpinning)
        {
            m_TargetRotation = m_Rect.transform.localEulerAngles + new Vector3(0, 0, -15);
            if (m_TargetRotation.z < -30 || (m_TargetRotation.z > 0 && m_TargetRotation.z < 330))
            {
                m_TargetRotation = new Vector3(m_TargetRotation.x, m_TargetRotation.y, -30);
            }
        }
    }
}
