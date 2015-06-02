using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;

public enum Achievement_Action { WIN_SINGLE, WIN_MULTI, UPGRADE_DOCTER, UPGRADE_SCIENTIST, UPGRADE_AHTHLETE, UPGRADE_BUSINESS, UPGRADE_WARRIOR, UPGRADE_MUSICIAN,
COLLECT_AVATAR, SPIN_SLOTMACHINE, CORRECT_GEOGRAPHY, CORRECT_ENTERTAINMENT, CORRECT_HISTORY, CORRECT_ART, CORRECT_SCIENCE, CORRECT_SPORT, USE_HINT
, USE_EXTRATIME
, USE_SURVEY
, USE_SKIP
, RECEIVE_LIVES
, SEND_LIVES
, STREAK_WIN
};

public class Achievement
{
    public string m_Title;
    public string m_Description;
    public List<int> m_Requirement;
    Achievement_Action m_Action;
    public int m_Counter;

    public void OnAction(Achievement_Action action) {
        if (action == m_Action)
        {
            m_Counter++;
        }
    }

    public int GetCounter()
    {
        return m_Counter;
    }

    public void SetInfo(string _title, string _des, List<int> _requirement, Achievement_Action _action)
    {
        m_Title = _title;
        m_Description = _des;
        m_Requirement = new List<int>(_requirement.Count);
        for (int i = 0; i < _requirement.Count; i++) {
            m_Requirement.Add(_requirement[i]);
        }
        m_Action = _action;
    }

    public string ToString()
    {
        return m_Title + " " + m_Description + " " + m_Requirement[0] + " " + m_Requirement[1] + " " + m_Requirement[2] + " " + m_Action.ToString();
    }

    public void SetCounter(int counter)
    {
        m_Counter = counter;
    }
}
