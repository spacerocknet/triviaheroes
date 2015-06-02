using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIProfile : MonoBehaviour {

    public GameObject m_AchievementPanel;
    public GameObject m_AchievementPrefab;

    public CustomizePage m_CustomizePage;
    public AvatarScript m_Avatar;

    public List<UIAchievement> m_AchievementList = new List<UIAchievement>();

	// Use this for initialization
	void Start () {
        int num = AchievementList.Instance.GetAchievementCount();
        for (int i = 0; i < num; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_AchievementPrefab);
            go.transform.SetParent(m_AchievementPanel.transform);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -90 - i * 180, 0);
            rt.localScale = new Vector3(1, 1, 1);
            if (i % 2 == 0)
            {
                go.transform.FindChild("Background").GetComponent<Image>().color = Color.white;
            }

            Achievement a = AchievementList.Instance.GetAchievementBy(i);
            go.GetComponent<UIAchievement>().SetInfo(a);

            m_AchievementList.Add(go.GetComponent<UIAchievement>());
        }
        m_AchievementPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, num * 180);
	}

    // Update is called once per frame
    void Update()
    {
	
	}

    public void OnBack()
    {
        CanvasScript cv = SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PROFILE);
        cv.MoveOutToRight();
    }

    public void Refresh()
    {
        m_CustomizePage.Refresh();
        RefreshAvatar();
        RefreshAchievement();
    }

    public void RefreshAchievement()
    {
        for (int i = 0; i < m_AchievementList.Count; i++)
        {
            Achievement a = AchievementList.Instance.GetAchievementBy(i);
            m_AchievementList[i].SetInfo(a);
        }
    }

    public void RefreshAvatar()
    {
        PlayerProfile pl = GameManager.Instance.GetPlayerProfile();        
        m_Avatar.SetInfo(GameManager.Instance.GetMyActiveAvatar());
    }
}
