using UnityEngine;
using System.Collections;

public class StoreCustomizePage : MonoBehaviour {

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

        
        for (int i = 0; i < 8; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_ItemSelectPagePrefab);
            go.transform.SetParent(panel.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100 - i * 260, 0);
            go.transform.FindChild("Image").gameObject.SetActive(false);

            for (int j = 0; j < 2; j++)
            {
                GameObject panel1 = go.transform.FindChild("Panel").gameObject;
                GameObject go1 = (GameObject)GameObject.Instantiate(m_ItemPrefab);
                go1.transform.SetParent(panel1.transform);
                go1.transform.localScale = new Vector3(1, 1, 1);
                go1.GetComponent<RectTransform>().anchoredPosition = new Vector3(200 + 0 + j * 200, 0, 0);
                go1.GetComponent<ItemScript>().SetInfo(i, j + 1, true, true);
            }

            go.GetComponent<HorizontalScrollSnap>().Screens = 2;
        }
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, 8 * 260 + 100);
    }

    
}
