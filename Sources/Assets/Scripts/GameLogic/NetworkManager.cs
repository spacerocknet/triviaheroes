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

    IEnumerator ActuallyDoStartNewGame(string friend)
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
        var ret = new JSONClass();
        if (friend == "Random")
        {
            ret["opponent"] = "RandomOpponent";
        }
        else
        {
            ret["opponent"] = friend;
        }
        Debug.Log("HIHI");
        GameManager.Instance.OnStartNewGameResult(ret.ToString());
    }

    public void DoRegister(string name, int sex)
    {
        StartCoroutine(ActuallyDoRegister(name, sex));
    }

    public void DoStartNewGame(string friend)
    {
        StartCoroutine(ActuallyDoStartNewGame(friend));
    }

    IEnumerator ActuallyDoCategoryConfirmToPlay(Category cat)
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
        var ret = new JSONClass();
        ret["category"].AsInt = (int)cat;
        ret["question"] = "Which is odd number?";
        ret["answer1"] = "1";
        ret["answer2"] = "2";
        ret["answer3"] = "4";
        ret["answer4"] = "6";
        ret["correct"].AsInt = 0;
        GameManager.Instance.OnCategoryConfirmToPlayResult(ret.ToString());
    }

    public void DoCategoryConfirmToPlay(Category cat)
    {
        StartCoroutine(ActuallyDoCategoryConfirmToPlay(cat));
    }

    IEnumerator ActuallyDoTrophyClaimSelected(int trophy)
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
        var ret = new JSONClass();
        ret["category"].AsInt = (int)trophy;
        ret["question"] = "Which is odd number?";
        ret["answer1"] = "1";
        ret["answer2"] = "2";
        ret["answer3"] = "4";
        ret["answer4"] = "6";
        ret["correct"].AsInt = 0;
        GameManager.Instance.OnTrophyClaimSelectedResult(ret.ToString());
    }

    IEnumerator ActuallyDoTrophyChallengeSelected()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
        var ret = new JSONClass();
        ret["category"].AsInt = (int)0;
        ret["question"] = "Which is odd number?";
        ret["answer1"] = "1";
        ret["answer2"] = "2";
        ret["answer3"] = "4";
        ret["answer4"] = "6";
        ret["correct"].AsInt = 0;
        GameManager.Instance.OnTrophyClaimSelectedResult(ret.ToString());
    }

    public void DoTrophyClaimSelected(int trophy)
    {
        StartCoroutine(ActuallyDoTrophyClaimSelected(trophy));
    }

    public void DoTrophyChallengeSelected()
    {
        StartCoroutine(ActuallyDoTrophyChallengeSelected());
    }

    public void OnRegisterResult()
    {
    }


}
