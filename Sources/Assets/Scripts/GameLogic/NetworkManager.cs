using UnityEngine;
using System.Collections;
using SimpleJSON;

public class NetworkManager : MonoBehaviour {

    private static NetworkManager m_sInstance = null;    

    public void Awake()
    {
        m_sInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    private NetworkManager()
    {
        m_sInstance = this;
    }

    public static NetworkManager Instance
    {
        get
        {
            if (m_sInstance == null)
            {
                m_sInstance = new NetworkManager();
            }
            return m_sInstance;
        }
    }

    IEnumerator ActuallyDoRegister(string name, int sex)
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
        var ret = new JSONClass();
        ret["result"].AsBool = true;
        ret["name"] = name;
        ret["sex"].AsInt = sex;
        GameManager.Instance.OnRegisterResult(ret.ToString());
    }

    public void DoRegister(string name, int sex)
    {
        StartCoroutine(ActuallyDoRegister(name, sex));
    }

    public void OnRegisterResult()
    {
    }
}
