using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TrophyGroup : MonoBehaviour {

    List<Image> m_ImageList = new List<Image>();

	// Use this for initialization
	void Start () {
        for (int i = 1; i <= 6; i++)
        {
            m_ImageList.Add(transform.FindChild("Puzzle" + i.ToString()).gameObject.GetComponent<Image>());
        }
	}

    // Update is called once per frame
    void Update()
    {
	
	}

    public void SetTrophyState(int[] state)
    {
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] == 0)
            {
                m_ImageList[i].enabled = false;
            }
            else
            {
                m_ImageList[i].enabled = true;
            }
        }
    }
}
