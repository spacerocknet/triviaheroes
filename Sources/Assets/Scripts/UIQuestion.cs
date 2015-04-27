using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIQuestion : MonoBehaviour {

    public Text m_QuestionTitle;
    public Text m_QuestionText;
    public Text m_Answer0;
    public Text m_Answer1;
    public Text m_Answer2;
    public Text m_Answer3;
    public Text m_TimerText;

    float m_Timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnAnswer1()
    {
        
    }

    public void OnAnswer2()
    {
    }

    public void OnAnswer3()
    {
    }

    public void OnAnswer4()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_GOTTROPHY);
        cv.MoveInFromRight();
    }

    public void SetQuestion(Question question)
    {
        //m_QuestionTitle
        //m_Timer = 60;
    }
}
