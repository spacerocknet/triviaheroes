using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAchievement : MonoBehaviour {

    public Text m_Title;
    public Text m_Description;
    public Image m_Progress;
    public Text m_Counter;
    public Image m_Stars;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInfo(Achievement a)
    {
        m_Title.text = a.m_Title;
        m_Description.text = a.m_Description;
        m_Counter.text = a.m_Counter + "/" + a.m_Requirement[2];
        m_Progress.GetComponent<RectTransform>().localScale = new Vector3((float)a.m_Counter / a.m_Requirement[2], 1, 1);
    }
}
