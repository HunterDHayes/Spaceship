using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public AudioClip[] m_MusicAudioClips;
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_MusicAudioSources;
    private AudioSource[] m_SfxAudioSources;
    public ButtonController m_bMusic;
    public ButtonController m_bSFX;
    public Canvas m_cCredits;
    public Canvas m_cMainMenu;
    public Canvas m_cDifficultyMenu;


    // Use this for initialization
    void Start()
    {
        m_cMainMenu.gameObject.SetActive(true);
        m_cCredits.gameObject.SetActive(false);
        m_cDifficultyMenu.gameObject.SetActive(false);
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
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bSFX.getClick())
            PlayerPrefs.SetFloat("SFXVolume", 100);
        else
            PlayerPrefs.SetFloat("SFXVolume", 0);

        if (m_bMusic.getClick())
            PlayerPrefs.GetFloat("MusicVolume", 100);
        else
            PlayerPrefs.GetFloat("MusicVolume", 0);

    }

    /// <summary>
    ///Exit the game
    /// </summary>
    public void exitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Goes to Select Difficulty level
    /// </summary>
    public void startGame()
    {
        m_cMainMenu.gameObject.SetActive(false);
        m_cCredits.gameObject.SetActive(false);
        m_cDifficultyMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Open the credits menu
    /// </summary>
    public void creditsMenu()
    {
        m_cMainMenu.gameObject.SetActive(false);
        m_cCredits.gameObject.SetActive(true);
        m_cDifficultyMenu.gameObject.SetActive(false);
    }
    /// <summary>
    /// Goes back to the main menu
    /// </summary>
    public void mainMenu()
    {
        m_cMainMenu.gameObject.SetActive(true);
        m_cCredits.gameObject.SetActive(false);
        m_cDifficultyMenu.gameObject.SetActive(false);
    }

    public void startEasyGame() {
        PlayerPrefs.SetString("Difficulty", "Easy");
        Application.LoadLevel("GamePlay");
    }

    public void startMediumGame()
    {
        PlayerPrefs.SetString("Difficulty", "Medium");
        Application.LoadLevel("GamePlay");
    }

    public void startHardGame()
    {
        PlayerPrefs.SetString("Difficulty", "Hard");
        Application.LoadLevel("GamePlay");
    }
}
