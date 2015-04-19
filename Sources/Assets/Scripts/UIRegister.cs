using UnityEngine;
using System.Collections;

public class UIRegister : MonoBehaviour {

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
    }

    public void OnGentlemenSelect()
    {
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
