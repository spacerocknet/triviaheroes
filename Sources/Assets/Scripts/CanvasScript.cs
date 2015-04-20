using UnityEngine;
using System.Collections;



public class CanvasScript : MonoBehaviour {

    bool m_bIsMoving = false;
    
    public RectTransform m_RootPos;
    public RectTransform m_RightPos;
    public RectTransform m_LeftPos;
    public RectTransform m_LeftFar;
    public float m_MoveDuration = 1.0f;

    RectTransform rt;
    float m_MoveTime;

    Vector3 startPos;
    Vector3 endPos;

	// Use this for initialization
	void Start () {
        rt = gameObject.GetComponent<RectTransform>();
        m_RootPos = GameObject.Find("RootPos").GetComponent<RectTransform>();
        m_RightPos = GameObject.Find("RightPos").GetComponent<RectTransform>();
        m_LeftPos = GameObject.Find("LeftPos").GetComponent<RectTransform>();
        m_LeftFar = GameObject.Find("LeftFar").GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {

        if (m_bIsMoving)
        {
            m_MoveTime += Time.deltaTime;
            if (m_MoveTime < m_MoveDuration)
            {
                float d = Mathf.SmoothStep(0, 1, m_MoveTime * (1 / m_MoveDuration));
                rt.position = startPos + (endPos - startPos) * d;
            }
            else
            {
                m_bIsMoving = false;
                rt.position = endPos;
                if (endPos.Equals(m_LeftPos.position))
                {
                    Debug.Log("FAK");
                    rt.position = rt.position - new Vector3(1000, 0, 0);
                }
            }
        }
        
	}

    public void MoveInFromRight()
    {
        Move(m_RightPos.position, m_RootPos.position, m_MoveDuration);
    }

    public void MoveOutToRight()
    {
        Move(m_RootPos.position, m_RightPos.position, m_MoveDuration);
    }

    public void MoveInFromLeft()
    {
        Move(m_LeftPos.position, m_RootPos.position, m_MoveDuration);
    }


    public void MoveInFromLeftFar()
    {
        Move(m_LeftFar.position, m_RootPos.position, m_MoveDuration);
    }


    public void MoveOutToLeft()
    {
        Move(m_RootPos.position, m_LeftPos.position, m_MoveDuration);
    }

    public void MoveOutToLeftFar()
    {
        Move(m_RootPos.position, m_LeftFar.position, m_MoveDuration);
    }

    public void Move(Vector3 _startPos, Vector3 _endPos, float time)
    {
        m_bIsMoving = true;

        startPos = _startPos;
        endPos = _endPos;

        rt.position = startPos;

        m_MoveTime = 0;
    }
}
