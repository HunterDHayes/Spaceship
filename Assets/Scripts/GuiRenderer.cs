using UnityEngine;
using System.Collections;

public class GuiRenderer : MonoBehaviour
{
    #region DataMembers
    // Audio Files
    public AudioClip[] m_MusicAudioClips;
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_MusicAudioSources;
    private AudioSource[] m_SfxAudioSources;

    // Screen Scale
    public Vector2 m_PreferredScreenSize;
    private Vector2 m_ScreenScale;

    // Score
    public Vector2 m_ScoreBoxRenderPosition, m_ScoreRenderPosition;
    public int m_ScoreFontSize;

    // Lives
    public bool[] m_Lives;
    public Vector2 m_LivesRenderPosition;
    public float m_LivesRenderSpacing;
    public float m_DamageOverlayTime, m_DamageOverlayTimer;

    // Pause Menu
    public bool m_IsPaused;
    public Vector2 m_PauseButtonPosition, m_PauseMenuPosition, m_ResumeButtonPosition, m_RestartButtonPosition, m_ExitButtonPosition, m_MusicButtonPosition, m_SFXButtonPosition;
    public int m_PauseMenuTextSize;
    #endregion

    #region GUITextures
    public Texture m_DamageOverlay;
    public Texture m_ScoreBox;
    public Texture[] m_LivesTextures;
    public Texture m_PauseMenuBackground;
    #endregion

    #region GUISkins
    public GUISkin m_HUD;
    #endregion

    #region Unity Functions
    // Use this for initialization
    void Start()
    {
        #region Create Audio Assets
        m_MusicAudioSources = new AudioSource[m_MusicAudioClips.Length];

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;

            m_MusicAudioSources[i] = child.AddComponent("AudioSource") as AudioSource;
            m_MusicAudioSources[i].clip = m_MusicAudioClips[i];
            m_MusicAudioSources[i].loop = true;
        }

        m_SfxAudioSources = new AudioSource[m_SfxAudioClips.Length];

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;

            m_SfxAudioSources[i] = child.AddComponent("AudioSource") as AudioSource;
            m_SfxAudioSources[i].clip = m_SfxAudioClips[i];
            m_SfxAudioSources[i].loop = false;
        }

        float fMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float fSFXVolume = PlayerPrefs.GetFloat("SFXVolume");

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = fMusicVolume / 100.0f;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSFXVolume / 100.0f;
        #endregion

        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Paused", -1);

        int random = Random.Range(0, m_MusicAudioSources.Length);
        m_MusicAudioSources[random].Play();
    }

    // Update is called once per frame
    void Update()
    {
        m_ScreenScale.x = Screen.width / m_PreferredScreenSize.x;
        m_ScreenScale.y = Screen.height / m_PreferredScreenSize.y;

        // Change Font Sizes
        m_HUD.label.fontSize = (int)(m_ScoreFontSize * m_ScreenScale.y);
        m_HUD.GetStyle("PauseMenuButton").fontSize = (int)(m_PauseMenuTextSize * m_ScreenScale.y);

        float fMusicVolume = PlayerPrefs.GetFloat("MusicVolume");

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = fMusicVolume / 100.0f;

        float fSFXVolume = PlayerPrefs.GetFloat("SFXVolume");

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSFXVolume / 100.0f;

        if (m_DamageOverlayTimer > 0.0f && PlayerPrefs.GetInt("Paused") == -1)
            m_DamageOverlayTimer -= Time.deltaTime;

        if (!m_Lives[2])
            Application.LoadLevel("End Menu");
    }

    // Render GUI Elements
    void OnGUI()
    {
        if (RenderGUIButton(m_PauseButtonPosition.x, m_PauseButtonPosition.y, m_HUD.GetStyle("PauseButton").normal.background.width * 2, m_HUD.GetStyle("PauseButton").normal.background.height * 2, m_HUD.GetStyle("PauseButton")))
            PlayerPrefs.SetInt("Paused", 1);

        // Lives
        for (int i = 0; i < m_Lives.Length; i++)
            RenderGUITexture(m_LivesRenderPosition.x + (m_LivesTextures[0].width * 2 + m_LivesRenderSpacing) * i, m_LivesRenderPosition.y, m_LivesTextures[0].width * 2, m_LivesTextures[0].height * 2, (m_Lives[i]) ? m_LivesTextures[0] : m_LivesTextures[1]);

        // Score Box
        RenderGUITexture(m_ScoreBoxRenderPosition.x, m_ScoreBoxRenderPosition.y, m_ScoreBox.width * 2, m_ScoreBox.height * 2, m_ScoreBox);

        // Score
        string score = "Score   " + PlayerPrefs.GetInt("Score");
        RenderGUILabel(m_ScoreBoxRenderPosition.x + m_ScoreRenderPosition.x, m_ScoreBoxRenderPosition.y + m_ScoreRenderPosition.y, m_ScoreBox.width * 2.5f, m_ScoreBox.height * 2, score, m_HUD.label);
        
        if (m_DamageOverlayTimer > 0.0f && PlayerPrefs.GetInt("Paused") == -1)
            RenderGUITexture(0, 0, m_PreferredScreenSize.x, m_PreferredScreenSize.y, m_DamageOverlay);

        // Pause Menu
        if (PlayerPrefs.GetInt("Paused") == 1)
        {
            RenderGUITexture(m_PauseMenuPosition.x, m_PauseMenuPosition.y, m_PauseMenuBackground.width * 2, m_PauseMenuBackground.height * 2, m_PauseMenuBackground);

            // Resumes the game
            if (RenderGUIButton(m_PauseMenuPosition.x + m_ResumeButtonPosition.x, m_PauseMenuPosition.y + m_ResumeButtonPosition.y,
                m_HUD.GetStyle("PauseMenuButton").normal.background.width * 2, m_HUD.GetStyle("PauseMenuButton").normal.background.height * 2, "Resume", m_HUD.GetStyle("PauseMenuButton")))
                PlayerPrefs.SetInt("Paused", -1);

            // Restart Game
            if (RenderGUIButton(m_PauseMenuPosition.x + m_RestartButtonPosition.x, m_PauseMenuPosition.y + m_RestartButtonPosition.y,
                m_HUD.GetStyle("PauseMenuButton").normal.background.width * 2, m_HUD.GetStyle("PauseMenuButton").normal.background.height * 2, "Restart", m_HUD.GetStyle("PauseMenuButton")))
                Application.LoadLevel("Gameplay");

            // Exit to Main menu
            if (RenderGUIButton(m_PauseMenuPosition.x + m_ExitButtonPosition.x, m_PauseMenuPosition.y + m_ExitButtonPosition.y,
                m_HUD.GetStyle("PauseMenuExitButton").normal.background.width * 2, m_HUD.GetStyle("PauseMenuExitButton").normal.background.height * 2, "Exit", m_HUD.GetStyle("PauseMenuExitButton")))
                Application.LoadLevel("Main Menu");

            // SFX Button
            if (PlayerPrefs.GetFloat("SFXVolume") == 100.0f)
            {
                if (RenderGUIButton(m_PauseMenuPosition.x + m_SFXButtonPosition.x, m_PauseMenuPosition.y + m_SFXButtonPosition.y,
                m_HUD.GetStyle("PauseMenuSFXOnButton").normal.background.width * 2, m_HUD.GetStyle("PauseMenuSFXOnButton").normal.background.height * 2, "", m_HUD.GetStyle("PauseMenuSFXOnButton")))
                    PlayerPrefs.SetFloat("SFXVolume", 0.0f);
            }
            else
            {
                if (RenderGUIButton(m_PauseMenuPosition.x + m_SFXButtonPosition.x, m_PauseMenuPosition.y + m_SFXButtonPosition.y,
                m_HUD.GetStyle("PauseMenuSFXOffButton").normal.background.width * 2, m_HUD.GetStyle("PauseMenuSFXOffButton").normal.background.height * 2, "", m_HUD.GetStyle("PauseMenuSFXOffButton")))
                    PlayerPrefs.SetFloat("SFXVolume", 100.0f);
            }

            // Music Button
            if (PlayerPrefs.GetFloat("MusicVolume") == 100.0f)
            {
                if (RenderGUIButton(m_PauseMenuPosition.x + m_MusicButtonPosition.x, m_PauseMenuPosition.y + m_MusicButtonPosition.y,
                m_HUD.GetStyle("PauseMenuMusicOnButton").normal.background.width * 2, m_HUD.GetStyle("PauseMenuMusicOnButton").normal.background.height * 2, "", m_HUD.GetStyle("PauseMenuMusicOnButton")))
                    PlayerPrefs.SetFloat("MusicVolume", 0.0f);
            }
            else
            {
                if (RenderGUIButton(m_PauseMenuPosition.x + m_MusicButtonPosition.x, m_PauseMenuPosition.y + m_MusicButtonPosition.y,
                m_HUD.GetStyle("PauseMenuMusicOffButton").normal.background.width * 2, m_HUD.GetStyle("PauseMenuMusicOffButton").normal.background.height * 2, "", m_HUD.GetStyle("PauseMenuMusicOffButton")))
                    PlayerPrefs.SetFloat("MusicVolume", 100.0f);
            }
        }
    }
    #endregion

    #region Functions
    void GainLive()
    {
        for (int i = m_Lives.Length - 1; i >= 0; i--)
        {
            if (!m_Lives[i])
            {
                m_Lives[i] = true;
                break;
            }
        }
    }

    void LoseLive()
    {
        for (int i = 0; i < m_Lives.Length; i++)
        {
            if (m_Lives[i])
            {
                m_Lives[i] = false;
                m_DamageOverlayTimer = m_DamageOverlayTime;
                break;
            }
        }
    }
    #endregion

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
