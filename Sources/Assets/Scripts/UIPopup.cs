using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPopup : MonoBehaviour {

    public Text m_MessageText;

    public delegate void PopUpDelegate();
    PopUpDelegate m_YesDelegate = null;
    PopUpDelegate m_NoDelegate = null;

    public GameObject m_YesButton;
    public GameObject m_NoButton;
    public GameObject m_AcceptButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnAccept() {
        gameObject.GetComponent<CanvasScript>().Hide();
    }

    public void OnYes()
    {
        gameObject.GetComponent<CanvasScript>().Hide();
        m_YesDelegate();
    }

    public void OnNo()
    {
        gameObject.GetComponent<CanvasScript>().Hide();
        m_NoDelegate();
    }

    public void SetText(string text)
    {
        m_MessageText.text = text;              
    }

    public void Show(string text, int type, PopUpDelegate onYes, PopUpDelegate onNo, int nextCanvas)
    {
        SetText(text);
        if (type == 0)
        {
            m_AcceptButton.SetActive(true);
            m_YesButton.SetActive(false);
            m_NoButton.SetActive(false);
        }
        else if (type == 1)
        {
            m_AcceptButton.SetActive(false);
            m_YesButton.SetActive(true);
            m_NoButton.SetActive(true);
        }
        GetComponent<CanvasScript>().Show(nextCanvas);
        m_YesDelegate = onYes;
        m_NoDelegate = onNo;
    }

}
