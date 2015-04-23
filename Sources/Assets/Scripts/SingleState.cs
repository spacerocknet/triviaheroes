using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SingleState : MonoBehaviour {

    int m_Index;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SetIndex(int index)
    {
        if (index == 0)
        {
            transform.FindChild("ImageCircle").gameObject.GetComponent<Image>().enabled = false;
        }
    }

    public void SetCompleted()
    {
    }
}
