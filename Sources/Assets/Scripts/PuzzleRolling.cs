using UnityEngine;
using System.Collections;

using Image = UnityEngine.UI.Image;

public class PuzzleRolling : MonoBehaviour {

    public Sprite[] m_SpriteList;
    Image m_PuzzleImage;
    float m_Timer = 0;
    int m_CurrentSpriteIndex = 0;
	// Use this for initialization
	void Start () {
        m_PuzzleImage = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        m_Timer += Time.deltaTime;
        if (m_Timer > 0.2)
        {
            m_Timer = 0;
            m_CurrentSpriteIndex++;
            if (m_CurrentSpriteIndex >= m_SpriteList.Length) 
            {
                m_CurrentSpriteIndex = 0;
            }
            m_PuzzleImage.sprite = m_SpriteList[m_CurrentSpriteIndex];
        }
	}
}
