using UnityEngine;
using System.Collections;

public enum PVPStateType { NORMAL, TROPHY, CHALLENGE };

public class PVPState {
    public PVPStateType m_Type;
    public Category m_TargetTrophy; //Trophy will be acquired in TROPHY or CHALLENGE mode, or Category in NORMAL mode
    public Category m_BetTrophy;    //Trophy will lose in CHALLENGE MODE
    public int m_CurrentQuestion;
}

