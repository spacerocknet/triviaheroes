using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextManager  {

    static private TextManager m_sInstance;

    Dictionary<string, string> m_TextList = new Dictionary<string, string>();

    private TextManager()
    {
        m_sInstance = this;
        m_TextList["CantChallenge"] = "Opponent does not have any Trophy which you don't have";
    }

    public static TextManager Instance
    {
        get
        {
            if (m_sInstance == null)
            {
                m_sInstance = new TextManager();
            }
            return m_sInstance;
        }
    }

    public string GetTextByKey(string key)
    {
        return m_TextList[key];
    }

    public string GetAbilityName(Ability ability)
    {
        string ret = "NULL Ability";
        switch (ability)
        {
            case Ability.ABILITY_CLAIM:
                {
                    ret = "Free Claim";
                    break;
                }
            case Ability.ABILITY_CHALLENGE:
                {
                    ret = "Free Challenge";
                    break;
                }
            case Ability.ABILITY_COPY:
                {
                    ret = "Copy";
                    break;
                }
            case Ability.ABILITY_REMOVE:
                {
                    ret = "Remove";
                    break;
                }
            case Ability.ABILITY_UNDO:
                {
                    ret = "Undo";
                    break;
                }
            case Ability.ABILITY_SWITCH:
                {
                    ret = "Switch";
                    break;
                }
            default:
                break;
        }
        return ret;
    }
}
