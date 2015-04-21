using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIRegister : MonoBehaviour {

    public Image m_LadyTickImage;
    public Image m_GentlemenTickImage;

	// Use this for initialization
	void Start () {
	
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
    }

    public void OnGentlemenSelect()
    {
        m_LadyTickImage.enabled = false;
        m_GentlemenTickImage.enabled = true;
    }

    public void OnDone()
    {
        StartCoroutine("LoadNextScene");
    }

    public void OnConnectFB()
    {
        StartCoroutine("LoadNextScene");
    }


    IEnumerator LoadNextScene()
    {
        AsyncOperation async = Application.LoadLevelAsync("MainScene");        
        yield return async;
    }
}
