using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InviteFriendPrefab : MonoBehaviour {

    public Text m_AvatarText;
    public Image m_CheckImage;
    bool m_IsSelected = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInfo(string info)
    {
        m_AvatarText.text = info;
    }

    public void OnSelected()
    {
        m_IsSelected = !m_IsSelected;
        ShowSelected(m_IsSelected);
        //SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME).gameObject.GetComponent<UINewGame>().OnFriendSelected(gameObject);
    }

    public void ShowSelected(bool visible)
    {
        m_CheckImage.gameObject.SetActive(visible);
    }
}
