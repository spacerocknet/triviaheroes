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
    Question m_Question;

    public Button[] m_HelpButtons;
    public Button m_EndGameButton;

    public Text m_TimeOutText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_IsShowAnswer)
        {
            m_Timer -= Time.deltaTime;
          
            if (m_Timer < 0)
            {
                m_Timer = 0;
                GameManager.Instance.OnAnswerSelect(-1);
                m_TimeOutText.gameObject.SetActive(true);
            }
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
        if (!m_IsShowAnswer)
        {
            GameManager.Instance.OnAnswerSelect(0);
        }
    }

    public void OnAnswer2()
    {
        //Wrong Answer;
        if (!m_IsShowAnswer)
        {
            GameManager.Instance.OnAnswerSelect(1);
        }
    }

    public void OnAnswer3()
    {
        //Wrong Answer;
        if (!m_IsShowAnswer)
        {
            GameManager.Instance.OnAnswerSelect(2);
        }
    }

    public void OnAnswer4()
    {
        //Wrong Answer;
        if (!m_IsShowAnswer)
        {
            GameManager.Instance.OnAnswerSelect(3);
        }
    }

    public void OnEndGame()
    {
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_ENDGAMECONFIRM).Show();
    }

    public void SetQuestion(Question question)
    {
        m_QuestionText.text = question.m_Question;
        m_Answer0.text = question.m_Answer0;
        m_Answer1.text = question.m_Answer1;
        m_Answer2.text = question.m_Answer2;
        m_Answer3.text = question.m_Answer3;
        m_Timer = 15;

        for (int i = 0; i < 4; i++)
        {
            m_CheckImage[i].enabled = false;
            m_AnswerImage[i].sprite = m_AnswerSprite[0];
            m_AnswerImage[i].GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < 4; i++)
        {
            m_HelpButtons[i].interactable = true;
        }

        m_Question = question;

        m_IsShowAnswer = false;

        m_EndGameButton.gameObject.SetActive(false);
        m_TimeOutText.gameObject.SetActive(false);

        if (GameManager.Instance.GetPlayerProfile().m_FirstTimeExperience[1] == false)
        {
            GameManager.Instance.ShowHelpBoost();
        }
    }

    public void SetPVEQuestion(Question question, int number)
    {
        m_QuestionText.text = question.m_Question;
        m_Answer0.text = question.m_Answer0;
        m_Answer1.text = question.m_Answer1;
        m_Answer2.text = question.m_Answer2;
        m_Answer3.text = question.m_Answer3;
        m_Timer = 15;

        for (int i = 0; i < 4; i++)
        {
            m_CheckImage[i].enabled = false;
            m_AnswerImage[i].sprite = m_AnswerSprite[0];
            m_AnswerImage[i].GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < 4; i++)
        {
            m_HelpButtons[i].interactable = true;
        }

        m_Question = question;

        m_QuestionTitle.text = "Question " + number.ToString() + ";";

        m_IsShowAnswer = false;

        m_EndGameButton.gameObject.SetActive(true);
        m_TimeOutText.gameObject.SetActive(false);

        if (number == 1)
        {
            m_EndGameButton.gameObject.SetActive(false);
        }
        else
        {
            m_EndGameButton.gameObject.SetActive(true);
        }

        if (GameManager.Instance.GetPlayerProfile().m_FirstTimeExperience[1] == false)
        {
            GameManager.Instance.ShowHelpBoost();
        }

        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Instance.GetPlayerProfile().m_PVEBoostUsed[i] == false)
            {
                m_HelpButtons[i].interactable = true;
            }
            else
            {
                m_HelpButtons[i].interactable = false;
            }
        }
    }

    public void ShowAnswer(int select, int answer)
    {
        m_IsShowAnswer = true;
        m_ShowAnswerTimer = 2;
        //m_CheckImage[answer].enabled = true;

        if (select != answer)
        {
            if (select != -1)
            {
                m_AnswerImage[select].sprite = m_AnswerSprite[1];
            }
        }
        else
        {
            if (select != -1)
            {
                m_AnswerImage[select].sprite = m_AnswerSprite[2];
            }
        }
    }


    //1: Remove tow
    //2: Add 15 secs
    //3: Show % breakdown
    //4: Skip, get new question
    public void OnUseHelp(int type)
    {
        if (GameManager.Instance.m_IsPVE)
        {
            GameManager.Instance.GetPlayerProfile().m_PVEBoostUsed[type - 1] = true;
        }

        switch (type)
        {
            case 2:
                m_HelpButtons[1].interactable = false;
                int c = 2;
                int prev = -1;
                while (c > 0)
                {                    
                    int idx = Random.Range(0, 4);
                    if (idx != m_Question.m_CorrectAnswer && idx != prev)
                    {
                        prev = idx;
                        c--;
                        //m_AnswerImage[idx].sprite = m_AnswerSprite[4];
                        m_AnswerImage[idx].GetComponent<Button>().interactable = false;
                    }                    
                }
                AchievementList.Instance.OnAction(Achievement_Action.USE_HINT);
                break;
            case 1:
                m_Timer += 15f;
                m_HelpButtons[0].interactable = false;
                AchievementList.Instance.OnAction(Achievement_Action.USE_EXTRATIME);
                break;
            case 3:
                m_HelpButtons[2].interactable = false;
                int[] arr = new int[4];
                int sum = 100;
                for (int i = 0; i < 3; i++)
                {
                    arr[i] = Random.Range(0, sum);
                    sum -= arr[i];
                }
                arr[3] = sum;
                m_Answer0.text = m_Answer0.text + " (" + arr[0].ToString() + ")";
                m_Answer1.text = m_Answer1.text + " (" + arr[1].ToString() + ")";
                m_Answer2.text = m_Answer2.text + " (" + arr[2].ToString() + ")";
                m_Answer3.text = m_Answer3.text + " (" + arr[3].ToString() + ")";
                AchievementList.Instance.OnAction(Achievement_Action.USE_SURVEY);
                break;
            case 4:
                m_HelpButtons[3].interactable = false;
                GameManager.Instance.SkipQuestion();
                AchievementList.Instance.OnAction(Achievement_Action.USE_SKIP);
                break;
            default:
                
                break;
        }
    }
}
