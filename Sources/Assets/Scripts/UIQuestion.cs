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

    public Image[] m_CheckImage;
    public Image[] m_AnswerImage;
    public Sprite[] m_AnswerSprite;

    float m_Timer;
    float m_ShowAnswerTimer = 0;
    bool m_IsShowAnswer = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_IsShowAnswer)
        {
            m_Timer -= Time.deltaTime;
            m_TimerText.text = Mathf.FloorToInt(m_Timer).ToString();
        }
        if (m_ShowAnswerTimer > 0)
        {
            m_ShowAnswerTimer -= Time.deltaTime;
            if (m_ShowAnswerTimer <= 0)
            {
                GameManager.Instance.OnShowAnswerEnded();
            }
        }
	}

    public void OnAnswer1()
    {
        //Correct answer
        GameManager.Instance.OnAnswerSelect(0);
    }

    public void OnAnswer2()
    {
        //Wrong Answer;
        GameManager.Instance.OnAnswerSelect(1);
    }

    public void OnAnswer3()
    {
        //Wrong Answer;
        GameManager.Instance.OnAnswerSelect(2);
    }

    public void OnAnswer4()
    {
        //Wrong Answer;
        GameManager.Instance.OnAnswerSelect(3);        
    }

    public void SetQuestion(Question question)
    {
        m_QuestionText.text = question.m_Question;
        m_Answer0.text = question.m_Answer0;
        m_Answer1.text = question.m_Answer1;
        m_Answer2.text = question.m_Answer2;
        m_Answer3.text = question.m_Answer3;
        m_Timer = 60;

        for (int i = 0; i < 4; i++)
        {
            m_CheckImage[i].enabled = false;
            m_AnswerImage[i].sprite = m_AnswerSprite[i];
        }

    }

    public void ShowAnswer(int select, int answer)
    {
        m_IsShowAnswer = true;
        m_ShowAnswerTimer = 2;
        m_CheckImage[answer].enabled = true;
        if (select != answer)
        {
            m_AnswerImage[select].sprite = m_AnswerSprite[4];            
        }
    }
}
