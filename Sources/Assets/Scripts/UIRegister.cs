using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRegister : MonoBehaviour {

    public Image m_LadyTickImage;
    public Image m_GentlemenTickImage;
    public Text m_InputText;
    int m_Sex;

	// Use this for initialization
	void Start () {
        m_Sex = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnNickname()
    {
    }

    public void OnLadySelect()
    {
        m_LadyTickImage.enabled = true;
        m_GentlemenTickImage.enabled = false;
        m_Sex = 1;
    }

    public void OnGentlemenSelect()
    {
        m_LadyTickImage.enabled = false;
        m_GentlemenTickImage.enabled = true;
        m_Sex = 0;
    }

    public void OnDone()
    {
        GameManager.Instance.SetRegisterInfo(m_InputText.text, m_Sex);        
        NetworkManager.Instance.DoRegister(m_InputText.text, m_Sex);
    }

    public void OnConnectFB()
    {
        NetworkManager.Instance.DoRegister("FBPlayer", 0);
        //StartCoroutine("LoadNextScene");
    }


    IEnumerator LoadNextScene()
    {
        AsyncOperation async = Application.LoadLevelAsync("MainScene");        
        yield return async;
    }
}
