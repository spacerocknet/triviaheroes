using UnityEngine;
using System.Collections;

public class CustomizePage : MonoBehaviour {

    public int m_NumberOfOption;
    public GameObject m_ItemSelectPagePrefab;
    public GameObject m_ItemPrefab;

	// Use this for initialization
	void Start () {
        InitPage();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void InitPage()
    {
        GameObject panel = transform.FindChild("Panel").gameObject;
        for (int i = 0; i < m_NumberOfOption; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_ItemSelectPagePrefab);
            go.transform.parent = panel.transform;
            go.transform.localScale = new Vector3(1, 1, 1);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -150 - i * 200, 0);

            for (int j = 0; j < 10; j++)
            {
                GameObject panel1 = go.transform.FindChild("Panel").gameObject;
                GameObject go1 = (GameObject)GameObject.Instantiate(m_ItemPrefab);
                go1.transform.parent = panel1.transform;
                go1.transform.localScale = new Vector3(1, 1, 1);
                go1.GetComponent<RectTransform>().anchoredPosition = new Vector3(100 + j * 200, 0, 0);
            }

            go.transform.FindChild("Panel").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10 * 200, 150);
        }
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, m_NumberOfOption * 200);
    }
}
