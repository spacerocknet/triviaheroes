﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIProfile : MonoBehaviour {

    public GameObject m_AchievementPanel;
    public GameObject m_AchievementPrefab;

	// Use this for initialization
	void Start () {
        int num = 20;
        for (int i = 0; i < num; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_AchievementPrefab);
            go.transform.parent = m_AchievementPanel.transform;
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -90 - i * 180, 0);
            rt.localScale = new Vector3(1, 1, 1);
            if (i % 2 == 0)
            {
                go.transform.FindChild("Background").GetComponent<Image>().color = Color.white;
            }
        }
        m_AchievementPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, num * 180);
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
