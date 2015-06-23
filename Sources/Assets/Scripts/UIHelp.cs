using UnityEngine;
using System.Collections;

public class UIHelp : MonoBehaviour {

    public GameObject m_Panel1;
    public GameObject m_Panel2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowNewGameHelp()
    {
    }

    public void ShowBoostHelp()
    {
    }

    public void OnNewGame()
    {
        GetComponent<CanvasScript>().Hide();
    }

    public void OnUseHelp(int id)
    {
        GetComponent<CanvasScript>().Hide();
    }

    public void Show(int id)
    {
        GetComponent<CanvasScript>().Show();
        if (id == 0)
        {
            m_Panel1.SetActive(true);
            m_Panel2.SetActive(false);
        }
        else if (id == 1)
        {
            m_Panel1.SetActive(false);
            m_Panel2.SetActive(true);
        }
    }
}
