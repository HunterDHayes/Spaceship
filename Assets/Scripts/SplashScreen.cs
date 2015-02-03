using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    // Screen Scale
    public Vector2 m_PreferredScreenSize;
    private Vector2 m_ScreenScale;

    // Data members
    public float m_SplashScreenTime, m_SplashScreenTimer;
    public int m_RenderSplashScreenIndex;
    public Texture[] m_SplashScreens;

    // Use this for initialization
    void Start()
    {
        m_SplashScreenTimer = m_SplashScreenTime;
    }

    // Update is called once per frame
    void Update()
    {
        m_ScreenScale.x = Screen.width / m_PreferredScreenSize.x;
        m_ScreenScale.y = Screen.height / m_PreferredScreenSize.y;

        m_SplashScreenTimer -= Time.deltaTime;

        if (m_SplashScreenTimer <= 0.0f)
        {
            m_RenderSplashScreenIndex++;
            m_SplashScreenTimer = m_SplashScreenTime;

            if (m_RenderSplashScreenIndex >= m_SplashScreens.Length)
                Application.LoadLevel("Main Menu");
        }

        if (m_RenderSplashScreenIndex >= m_SplashScreens.Length)
            Application.LoadLevel("Main Menu");
    }

    void OnGUI()
    {
        if (m_RenderSplashScreenIndex < m_SplashScreens.Length)
            RenderGUITexture(0, 0, m_SplashScreens[m_RenderSplashScreenIndex]);
        else
            RenderGUITexture(0, 0, m_SplashScreens[m_RenderSplashScreenIndex - 1]);
    }

    #region RenderGUIFunctions
    void RenderGUITexture(float _X, float _Y, float _Width, float _Height, Texture _Texture)
    {
        GUI.DrawTexture(new Rect(_X * m_ScreenScale.x, _Y * m_ScreenScale.y, _Width * m_ScreenScale.x, _Height * m_ScreenScale.y), _Texture);
    }
    void RenderGUITexture(float _X, float _Y, Texture _Texture)
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
