using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class GameModeTab : MonoBehaviour {

    public Sprite[] m_SpriteList;
    GameManager m_GameManager;

    // Use this for initialization
    void Start()
    {
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMultiplayer()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];
        Debug.Log("On Multi");
    }

    public void OnSingle()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[1];
        Debug.Log("On Single");
    }

}
