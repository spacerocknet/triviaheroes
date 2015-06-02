using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIWaiting : MonoBehaviour {

    public Text m_Content;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetContentType(int _type = 0)
    {
        if (_type == 0)
        {
            m_Content.text = GameConfig.Instance.GetRandomTips();
        }
        else
        {
            m_Content.text = "Searching new Opponent";
        }
    }
}
