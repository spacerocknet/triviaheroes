using UnityEngine;
using System.Collections;

public class UIEndgameConfirm : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnYes()
    {
        GetComponent<CanvasScript>().Hide();
        GameManager.Instance.OnEndPvEGame();
    }

    public void OnNo()
    {
        GetComponent<CanvasScript>().Hide();
    }

    public void OnNext()
    {
        GameManager.Instance.OnEndPvEGameConfirm();
    }
}
