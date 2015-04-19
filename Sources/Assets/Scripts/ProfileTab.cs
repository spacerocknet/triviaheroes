using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class ProfileTab : MonoBehaviour {

    public Sprite[] m_SpriteList;
    

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTheirTurn()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[1];

        
    }

    public void OnYourTurn()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[0];

        
    }

    public void OnPastGame()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = m_SpriteList[2];

        
    }
}
