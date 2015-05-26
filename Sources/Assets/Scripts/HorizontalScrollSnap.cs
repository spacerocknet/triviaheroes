using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class HorizontalScrollSnap : MonoBehaviour
{
    [Tooltip("the container the screens or pages belong to")]
    public Transform ScreensContainer;
    [Tooltip("how many screens or pages are there within the content")]
    public int Screens = 1;
    [Tooltip("which screen or page to start on")]
    public int StartingScreen = 0;

    private List<float> m_Positions;
    private ScrollRect m_ScrollRect;
    private float m_LerpTarget;
    private bool m_Lerp;
    private RectTransform m_ScrollViewRectTrans;
    public int m_Cat;
    public int m_ID;
    public float m_ChildSize = 200;
    public delegate void OnItemChanged(int cat, int id);
    OnItemChanged m_Callback;

    void Start()
    {
        m_ScrollRect = gameObject.GetComponent<ScrollRect>();
        m_ScrollViewRectTrans = gameObject.GetComponent<RectTransform>();
        m_ScrollRect.inertia = false;
        m_Lerp = false;

        m_Positions = new List<float>();

        if (Screens > 0)
        {
            Vector3 startPos = ScreensContainer.localPosition;
            Vector3 endPos = ScreensContainer.localPosition + Vector3.left * ((Screens - 1) * m_ScrollViewRectTrans.rect.width);

            float horiNormPos = 0;

            for (int i = 0; i < Screens; ++i)
            {                
                //float horiNormPos = (float)(i * 2) / (float)((Screens - 1) * 2);
                // this does not seem to have an effect [Tested on Unity 4.6.0 RC 2]
                //m_ScrollRect.horizontalNormalizedPosition = horiNormPos;
                m_Positions.Add(horiNormPos);
                horiNormPos = horiNormPos - m_ChildSize;
            }
        }

        // this does not seem to have an effect [Tested on Unity 4.6.0 RC 2]
        //m_ScrollRect.horizontalNormalizedPosition = (float)(StartingScreen - 1) / (float)(Screens - 1);
    }

    void FixedUpdate()
    {
        if (m_Lerp)
        {
            //m_ScrollRect.horizontalNormalizedPosition = Mathf.Lerp(m_ScrollRect.horizontalNormalizedPosition, m_LerpTarget, 10f * Time.deltaTime);
            //ScreensContainer.localPosition = Vector3.Lerp(ScreensContainer.localPosition, m_LerpTarget * new Vector3(200, 0, 0), 10 * Time.deltaTime);
            ScreensContainer.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(ScreensContainer.gameObject.GetComponent<RectTransform>().anchoredPosition, new Vector3(m_LerpTarget, 0, 0), 10 * Time.deltaTime);
            if (Mathf.Approximately(ScreensContainer.gameObject.GetComponent<RectTransform>().anchoredPosition.x, m_LerpTarget))
            {
                Debug.Log("OK");
                m_Lerp = false;
            }

        }
    }

    /// <summary>
    /// Bind this to UnityEditor Event trigger Pointer Up
    /// </summary>
    public void DragEnd()
    {
        if (m_ScrollRect.horizontal)
        {
            m_Lerp = true;
            m_LerpTarget = FindClosestFrom(ScreensContainer.gameObject.GetComponent<RectTransform>().anchoredPosition.x, m_Positions);            
            //Debug.Log("Target: " + m_LerpTarget + " " + ScreensContainer.gameObject.GetComponent<RectTransform>().anchoredPosition.x);            
        }
    }

    /// <summary>
    /// Bind this to UnityEditor Event trigger Drag
    /// </summary>
    public void OnDrag()
    {
        m_Lerp = false;
    }

    float FindClosestFrom(float start, List<float> positions)
    {
        float closest = 0;
        float distance = Mathf.Infinity;

        

        foreach (float position in m_Positions)
        {
            if (Mathf.Abs(start - position) < distance)
            {
                distance = Mathf.Abs(start - position);
                closest = position;               
            }
        }

        for (int i = 0; i < m_Positions.Count; i++)
        {
            if (m_Positions[i] == closest)
            {
                if (m_Callback != null)
                {
                    m_Callback(m_Cat, i);
                }
            }
        }

        return closest;
    }

    public void SetItemCallBack(OnItemChanged func, int cat)
    {
        m_Callback = func;
        m_Cat = cat;
    }
}
