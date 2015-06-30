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
        Debug.Log("INIT PAGE");
        GameObject panel = transform.FindChild("Panel").gameObject;
        for (int i = 0; i < 8; i++)
        {
            GameObject go = (GameObject)GameObject.Instantiate(m_ItemSelectPagePrefab);
            go.transform.SetParent(panel.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -150 - i * 260, 0);

            go.GetComponent<HorizontalScrollSnap>().SetItemCallBack(OnItemSelected, i);

            go.transform.FindChild("Text").gameObject.SetActive(false);
            go.transform.FindChild("Image 1").gameObject.SetActive(false);

            int count = 0;
            //Add first dummy
            {
                GameObject panel1 = go.transform.FindChild("Panel").gameObject;
                GameObject go1 = (GameObject)GameObject.Instantiate(m_ItemPrefab);
                go1.transform.SetParent(panel1.transform);
                go1.transform.localScale = new Vector3(1, 1, 1);
                go1.GetComponent<RectTransform>().anchoredPosition = new Vector3(200 + count * 200, 0, 0);
                go1.GetComponent<ItemScript>().SetInfo(-1, 0, false, false);
                count++;
            }
            for (int j = 0; j < 3; j++)
            {
                if (GameManager.Instance.IsItemOwned(i, j))
                {
                    GameObject panel1 = go.transform.FindChild("Panel").gameObject;
                    GameObject go1 = (GameObject)GameObject.Instantiate(m_ItemPrefab);
                    go1.transform.SetParent(panel1.transform);
                    go1.transform.localScale = new Vector3(1, 1, 1);
                    go1.GetComponent<RectTransform>().anchoredPosition = new Vector3(200 + count * 200, 0, 0);
                    go1.GetComponent<ItemScript>().SetInfo(i, j, false, false);
                    count++;
                }
            }

            //Add last dummy
            {
                GameObject panel1 = go.transform.FindChild("Panel").gameObject;
                GameObject go1 = (GameObject)GameObject.Instantiate(m_ItemPrefab);
                go1.transform.SetParent(panel1.transform);
                go1.transform.localScale = new Vector3(1, 1, 1);
                go1.GetComponent<RectTransform>().anchoredPosition = new Vector3(200 + count * 200, 0, 0);
                go1.GetComponent<ItemScript>().SetInfo(-1, 0, false, false);

                count++;
            }

            //Add last dummy
            {
                GameObject panel1 = go.transform.FindChild("Panel").gameObject;
                GameObject go1 = (GameObject)GameObject.Instantiate(m_ItemPrefab);
                go1.transform.SetParent(panel1.transform);
                go1.transform.localScale = new Vector3(1, 1, 1);
                go1.GetComponent<RectTransform>().anchoredPosition = new Vector3(200 + count * 200, 0, 0);

                go1.GetComponent<ItemScript>().SetInfo(-1, 0, false, false);

                count++;
            }

            //go.transform.FindChild("Panel").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(count * 200 + 400, go.transform.FindChild("Panel").gameObject.GetComponent<RectTransform>().sizeDelta.y);
            go.GetComponent<HorizontalScrollSnap>().Screens = count - 3;
            go.GetComponent<HorizontalScrollSnap>().StartingScreen = 0;
        }
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, 8 * 260 + 150);


    }

    public void Refresh()
    {
        GameObject panel = transform.FindChild("Panel").gameObject;
        foreach (Transform childTransform in panel.transform)
        {
            Destroy(childTransform.gameObject);
        }
        InitPage();
    }

    public void OnItemSelected(int cat, int id)
    {
        Debug.Log("Item selected " + cat + " " + id);
        PlayerProfile pl = GameManager.Instance.GetPlayerProfile();
        pl.ItemSelected(cat, id);
        pl.Save();
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_PROFILE).GetComponent<UIProfile>().RefreshAvatar();
    }
}
