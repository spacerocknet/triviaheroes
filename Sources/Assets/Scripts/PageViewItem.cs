using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class PageViewItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private ScrollRect scrollRect;
	// Use this for initialization
	void Start () {
        scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        scrollRect.OnDrag(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        //scrollRect.SetDraggedPosition(data);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.OnEndDrag(eventData);
    }
}
