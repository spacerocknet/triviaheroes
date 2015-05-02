using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

public class Question
{
    public Question(string q)
    {
        var ret = JSONNode.Parse(q);
        m_Category = ret["category"].AsInt;
        m_Question = ret["question"];
        m_Answer0 = ret["answer1"];
        m_Answer1 = ret["answer2"];
        m_Answer2 = ret["answer3"];
        m_Answer3 = ret["answer4"];
        m_CorrectAnswer = ret["correct"].AsInt;
    }
    public int m_Category;
    public String m_Question;
    public String m_Answer0;
    public String m_Answer1;
    public String m_Answer2;
    public String m_Answer3; 
    public int m_CorrectAnswer;
}
