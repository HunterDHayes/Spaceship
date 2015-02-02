using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ButtonController : MonoBehaviour
{
    public Sprite m_bOn;
    public Sprite m_bOff;
    public Image m_iButton;
    private bool m_bClicked;
    // Use this for initialization
    void Start()
    {
        //m_bClicked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bClicked)
        {
            m_iButton.sprite = m_bOn;
        }
        else
        {
            m_iButton.sprite = m_bOff;
        }
    }

    void ClickEvent()
    {
        m_bClicked = !m_bClicked;
    }

    public bool getClick()
    {
        return m_bClicked;
    }

    public void setClick(bool click) {
        m_bClicked = click;
    }
}
