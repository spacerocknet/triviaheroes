using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class PageViewItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private ScrollRect m_ParentScrollRect;
    private ScrollRect m_ScrollRect;
	// Use this for initialization
	void Start () {
        m_ParentScrollRect = transform.parent.parent.GetComponent<ScrollRect>();
        m_ScrollRect = GetComponent<ScrollRect>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_ParentScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        m_ParentScrollRect.OnDrag(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        //scrollRect.SetDraggedPosition(data);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_ParentScrollRect.OnEndDrag(eventData);
        Debug.Log(m_ScrollRect.horizontalNormalizedPosition);
    }
}
