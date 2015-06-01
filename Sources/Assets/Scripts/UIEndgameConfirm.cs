using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEndgameConfirm : MonoBehaviour {

    public Image m_WinImage;
    public Image m_LoseImage;
    public Text m_QuestionText;
    public Text m_RewardTitleText;
    public Text m_RewardText;
    public Text m_LifeText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnYes()
    {
        GetComponent<CanvasScript>().Hide();
        GameManager.Instance.OnEndPvEGame(true);
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
