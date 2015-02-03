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

    private int m_RandomMusic;
    private float m_fTimer;
    private int m_iDifficulty; //0 = none 1 = Easy 2 = Normal 3 = Hard
    // Use this for initialization
    void Start()
    {        
        m_fTimer = 0.0f;
        m_iDifficulty = 0;
        float m_fMusic = PlayerPrefs.GetFloat("MusicVolume");
        float m_fSFX = PlayerPrefs.GetFloat("SFXVolume");
        
        if (m_fMusic < 50)
            m_bMusic.setClick(false);
        else 
            m_bMusic.setClick(true);
        
        if (m_fSFX < 50)
            m_bSFX.setClick(false);
        else 
            m_bSFX.setClick(true);

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

        m_SfxAudioSources[2].Play();
        m_RandomMusic = Random.Range(0, m_MusicAudioSources.Length);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (m_bSFX.getClick())
            PlayerPrefs.SetFloat("SFXVolume", 100);
        else
            PlayerPrefs.SetFloat("SFXVolume", 0);

        if (m_bMusic.getClick())
            PlayerPrefs.SetFloat("MusicVolume", 100);
        else
            PlayerPrefs.SetFloat("MusicVolume", 0);

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (!m_SfxAudioSources[i].isPlaying)
                m_SfxAudioSources[i].volume = PlayerPrefs.GetFloat("SFXVolume") / 100.0f;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = PlayerPrefs.GetFloat("MusicVolume") / 100.0f;

        if (m_iDifficulty != 0)
        {
            m_fTimer += Time.deltaTime;
            if (m_fTimer > .4)
                Application.LoadLevel("GamePlay");
        }

        if (!m_SfxAudioSources[2].isPlaying && !m_MusicAudioSources[m_RandomMusic].isPlaying)
            m_MusicAudioSources[m_RandomMusic].Play();

        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
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

    /// <summary>
    /// Start the game in the easiest mode
    /// </summary>
    public void startEasyGame() {
        PlayerPrefs.SetString("Difficulty", "Easy");
        m_iDifficulty = 1;
    }
    /// <summary>
    /// Start the game in the normal mode
    /// </summary>
    public void startMediumGame()
    {
        PlayerPrefs.SetString("Difficulty", "Medium");
        m_iDifficulty = 2;
    }
    /// <summary>
    /// Starts the game in the most difficult mode
    /// </summary>
    public void startHardGame()
    {
        PlayerPrefs.SetString("Difficulty", "Hard");
        m_iDifficulty = 3;
    }

    public void NormalButtonSound() {
        m_SfxAudioSources[0].Play();
    }

    public void BackButtonSound() {
        m_SfxAudioSources[1].Play();
    }

}
