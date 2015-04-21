using UnityEngine;
using System.Collections;

public class UINewGame : MonoBehaviour {

    public GameObject m_MainCanvas;
    public GameObject m_NewGameCanvas;
    public GameObject m_GameMainCanvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBack()
    {
        CanvasScript cs = m_NewGameCanvas.GetComponent<CanvasScript>();
        cs.MoveOutToRight();

        cs = m_MainCanvas.GetComponent<CanvasScript>();
        cs.MoveInFromLeft();
    }

    public void StartGame()
    {
        CanvasScript cs = m_NewGameCanvas.GetComponent<CanvasScript>();
        cs.MoveOutToLeft();

        cs = m_GameMainCanvas.GetComponent<CanvasScript>();
        cs.MoveInFromRight();
    }
    
}
