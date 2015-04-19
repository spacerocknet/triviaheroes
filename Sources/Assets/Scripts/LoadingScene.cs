using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour {

    float m_Timer;
    AsyncOperation m_LoadLevelAsync;
	// Use this for initialization
	void Start () {
        StartCoroutine("LoadNextScene");
	}
	
	// Update is called once per frame
	void Update () {
        m_Timer += Time.deltaTime;        
        if (m_Timer > 1 && m_LoadLevelAsync.progress >= 0.9f)
        {
            m_LoadLevelAsync.allowSceneActivation = true;
        }
	}

    IEnumerator LoadNextScene()
    {        
        m_LoadLevelAsync = Application.LoadLevelAsync("RegisterScene");
        m_LoadLevelAsync.allowSceneActivation = false;                
        yield return m_LoadLevelAsync;        
    }
}
