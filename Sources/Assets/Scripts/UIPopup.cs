using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPopup : MonoBehaviour {

    public Text m_MessageText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnAccept() {
        
    }

    public void SetText(string text)
    {
        m_MessageText.text = text;

        
        
    }
}
