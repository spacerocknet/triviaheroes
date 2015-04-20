using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

    public RectTransform m_Parent;
    public RectTransform m_Transform;
    private Vector3 m_Start;
    private Vector3 m_ParentStart;

    void Awake()
    {
        m_Transform = gameObject.GetComponent<RectTransform>();
        m_Start = m_Transform.localPosition;
        m_ParentStart = m_Parent.localPosition;
        Debug.Log(m_Start);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        //Debug.Log(m_Parent.anchoredPosition3D.y);
        if (m_Parent.anchoredPosition3D.y > 610)
        {
           // Debug.Log(m_Start);
            m_Transform.localPosition = m_Start - new Vector3(0, m_Parent.anchoredPosition3D.y - 610, 0) - (m_ParentStart - m_Parent.localPosition);
          //  Debug.Log(m_Transform.anchoredPosition3D);
        }
        else
        {
            m_Transform.localPosition = m_Start - (m_ParentStart - m_Parent.localPosition);
        }
	}
}
