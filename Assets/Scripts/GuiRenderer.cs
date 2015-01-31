using UnityEngine;
using System.Collections;

public class GuiRenderer : MonoBehaviour
{
    #region DataMembers
    public Vector2 m_PreferredScreenSize;
    private Vector2 m_ScreenScale;
    #endregion

    #region GUITextures
    public Texture[] m_LivesTextures;
    #endregion

    #region GUISkins
    public GUISkin m_HUD;
    #endregion

    #region Unity Functions
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_ScreenScale.x = Screen.width / m_PreferredScreenSize.x;
        m_ScreenScale.y = Screen.height / m_PreferredScreenSize.y;
    }

    // Render GUI Elements
    void OnGUI()
    {
        RenderGUIButton(0, 0, m_HUD.GetStyle("PauseButton"));
        RenderGUIButton(200, 200, 100, 100, m_HUD.GetStyle("PauseButton"));
    }
    #endregion

    #region Functions
    void RenderGUITexture(float _X, float _Y, float _Width, float _Height, Texture2D _Texture)
    {
        GUI.DrawTexture(new Rect(_X * m_ScreenScale.x, _Y * m_ScreenScale.y, _Width * m_ScreenScale.x, _Height * m_ScreenScale.y), _Texture);
    }
    void RenderGUITexture(float _X, float _Y, Texture2D _Texture)
    {
        GUI.DrawTexture(new Rect(_X * m_ScreenScale.x, _Y * m_ScreenScale.y, _Texture.width * m_ScreenScale.x, _Texture.height * m_ScreenScale.y), _Texture);
    }

    bool RenderGUIButton(float _X, float _Y, float _Width, float _Height, string _String, GUIStyle _Button)
    {
        return GUI.Button(new Rect(_X * m_ScreenScale.x, _Y * m_ScreenScale.y, _Width * m_ScreenScale.x, _Height * m_ScreenScale.y), _String, _Button);
    }

    bool RenderGUIButton(float _X, float _Y, float _Width, float _Height, GUIStyle _Button)
    {
        return GUI.Button(new Rect(_X * m_ScreenScale.x, _Y * m_ScreenScale.y, _Width * m_ScreenScale.x, _Height * m_ScreenScale.y), "", _Button);
    }

    bool RenderGUIButton(float _X, float _Y, string _String, GUIStyle _Button)
    {
        return GUI.Button(new Rect(_X * m_ScreenScale.x, _Y * m_ScreenScale.y, _Button.normal.background.width * m_ScreenScale.x, _Button.normal.background.height * m_ScreenScale.y), _String, _Button);
    }

    bool RenderGUIButton(float _X, float _Y, GUIStyle _Button)
    {
        return GUI.Button(new Rect(_X * m_ScreenScale.x, _Y * m_ScreenScale.y, _Button.normal.background.width * m_ScreenScale.x, _Button.normal.background.height * m_ScreenScale.y), "", _Button);
    }

    void RenderGUILabel(float _X, float _Y, float _Width, float _Height, string _String, GUIStyle _Label)
    {
        GUI.Label(new Rect(_X * m_ScreenScale.x, _Y * m_ScreenScale.y, _Width * m_ScreenScale.x, _Height * m_ScreenScale.y), _String, _Label);
    }
    #endregion
}
