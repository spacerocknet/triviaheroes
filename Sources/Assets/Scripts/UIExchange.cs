using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIExchange : MonoBehaviour {

    public Text m_MinDiamond;
    public Text m_MaxDiamond;
    public Text m_Diamond;
    public Text m_Coin;
    public Slider m_Slider;

    int m_DiamondAmount;
    int m_CurrentPick;

    float m_Rate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInfo(int diamond, float rate)
    {
        m_MinDiamond.text = "0";
        m_MaxDiamond.text = diamond.ToString();
        m_DiamondAmount = diamond;
        m_Rate = rate;
        m_Slider.value = 0;
    }

    public void OnValueChanged()
    {
        float value = m_Slider.value;
        m_CurrentPick = Mathf.RoundToInt(value * m_DiamondAmount);

        m_Diamond.text = m_CurrentPick.ToString(); ;
        m_Coin.text = Mathf.RoundToInt(m_CurrentPick * m_Rate).ToString();
    }

    public void OnBack()
    {
        GetComponent<CanvasScript>().Hide();
    }

    public void OnAccept()
    {
        GetComponent<CanvasScript>().Hide();
        GameManager.Instance.ExchangeDiamond(m_CurrentPick);
    }
}
