using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;

public class Question
{
    public Question(string q)
    {
        if (q[0] == '[')
        {
            q = q.Substring(1, q.Length - 1);
        }

        if (q[q.Length - 1] == ']')
        {
            q = q.Substring(0, q.Length - 1);
        }

        var ret = JSONNode.Parse(q);
        Debug.Log(ret["category"]);
        m_Category = Utils.CategoryStringToIndex(ret["category"]);
        m_Question = ret["question"];        
        m_Answer0 = ret["answers"][0];
        m_Answer1 = ret["answers"][1];
        m_Answer2 = ret["answers"][2];
        m_Answer3 = ret["answers"][3];
        m_QID = ret["qid"].AsInt;
        m_CorrectAnswer = ret["df"].AsInt - 1;        
    }
    public int m_Category;
    public String m_Question;
    public String m_Answer0;
    public String m_Answer1;
    public String m_Answer2;
    public String m_Answer3;
    public int m_QID;
    public int m_CorrectAnswer;
}
