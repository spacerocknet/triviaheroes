using UnityEngine;
using System.Collections;



public class CanvasScript : MonoBehaviour {

    bool m_bIsMoving = false;
    
    public RectTransform m_RootPos;
    public RectTransform m_RightPos;
    public RectTransform m_LeftPos;
    public RectTransform m_LeftFar;
    private float m_MoveDuration = 0.7f;

    RectTransform rt;
    float m_MoveTime;

    Vector3 startPos;
    Vector3 endPos;

    private bool m_IsActive = false;
    private int m_NextID;

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
                    ////Debug.Log("FAK");
                    rt.position = rt.position - new Vector3(1000, 0, 0);

                }

                if (endPos.Equals(m_RootPos.position))
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    
                    gameObject.SetActive(false);
                    if (m_NextID != -1)
                    {
                        SceneManager.Instance.GetCanvasByID((CanvasID)m_NextID).SetActive(true);
                    }
                }
                SendMessage("OnShowUp");
            }
        }
        
	}

    public void MoveInFromRight()
    {
        gameObject.SetActive(true);
        SetActive(true);
        Move(m_RightPos.position, m_RootPos.position, m_MoveDuration);
    }

    public void MoveOutToRight(int cid = -1)
    {
        Move(m_RootPos.position, m_RightPos.position, m_MoveDuration);
        m_NextID = cid;
    }

    public void MoveInFromLeft()
    {
        SetActive(true);
        gameObject.SetActive(true);
        Move(m_LeftPos.position, m_RootPos.position, m_MoveDuration);
    }


    public void MoveInFromLeftFar()
    {
        SetActive(true);
        gameObject.SetActive(true);
        Move(m_LeftFar.position, m_RootPos.position, m_MoveDuration);
    }


    public void MoveOutToLeft(int cid = -1)
    {
        Move(m_RootPos.position, m_LeftPos.position, m_MoveDuration);
        m_NextID = cid;
    }

    public void MoveOutToLeftFar(int cid = -1)
    {
        Move(m_RootPos.position, m_LeftFar.position, m_MoveDuration);
        m_NextID = cid;
    }

    public void Move(Vector3 _startPos, Vector3 _endPos, float time)
    {
        m_bIsMoving = true;        

        startPos = _startPos;
        endPos = _endPos;

        rt.position = startPos;

        m_MoveTime = 0;
    }

    public void Show(int cid = -1)
    {
        Debug.Log("Show: " + gameObject.name);
        //SetActive(true);
        m_NextID = cid;
        rt.position = m_RootPos.position;
        gameObject.SetActive(true);
        SendMessage("OnShowUp");
    }

    public void Hide()
    {
        Debug.Log("Hide: " + gameObject.name);
        //SetActive(false);
        rt.position = m_LeftFar.position;
        gameObject.SetActive(false);
        if (m_NextID != -1)
        {
            SceneManager.Instance.GetCanvasByID((CanvasID)m_NextID).SetActive(true);
        }
    }

    public void SetActive(bool active)
    {
        m_IsActive = active;
        if (active)
        {
            SceneManager.Instance.SetActiveScene(this);
        }
    }

    public bool IsActive()
    {
        return m_IsActive;
    }
}
