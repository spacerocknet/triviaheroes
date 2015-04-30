using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FriendPrefab : MonoBehaviour {

    public Text m_AvatarText;
    public Image m_CheckImage;

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
        ShowSelected(true);
        SceneManager.Instance.GetCanvasByID(CanvasID.CANVAS_NEWGAME).gameObject.GetComponent<UINewGame>().OnFriendSelected(gameObject);
    }

    public void ShowSelected(bool visible)
    {
        m_CheckImage.enabled = visible;
    }
}
