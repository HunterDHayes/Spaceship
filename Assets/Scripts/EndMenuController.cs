using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndMenuController : MonoBehaviour {
    public AudioClip[] m_MusicAudioClips;
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_MusicAudioSources;
    private AudioSource[] m_SfxAudioSources;
    private int m_iScore;
    public Text m_tScore;
    private int m_bButtonPressed; //0 = none 1=Replay 2=Menu 3=Exit
    private float m_fTimer;
    
	// Use this for initialization
	
    void Start () {
        m_iScore = PlayerPrefs.GetInt("Score");
        m_tScore.text = m_iScore.ToString();

        m_fTimer = 0.0f;
        m_bButtonPressed = 0;
        
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

        int random = Random.Range(0, m_MusicAudioSources.Length);
        m_MusicAudioSources[random].Play();
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = PlayerPrefs.GetFloat("SFXVolume") / 100.0f;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = PlayerPrefs.GetFloat("MusicVolume") / 100.0f;

        if (m_bButtonPressed == 1) {
            m_fTimer += Time.deltaTime;
            if (m_fTimer > .4)
                Application.LoadLevel("GamePlay");
        }

        if (m_bButtonPressed == 2)
        {
            m_fTimer += Time.deltaTime;
            if (m_fTimer > .4)
                Application.LoadLevel("Main Menu");
        }

        if (m_bButtonPressed == 3)
        {
            m_fTimer += Time.deltaTime;
            if (m_fTimer > .4)
                Application.Quit();
        }
	}

    public void exitGame() {
        m_bButtonPressed = 3;
    }

    public void mainMenu() {
        m_bButtonPressed = 2;
    }

    public void replayGame() {
        m_bButtonPressed = 1;
    }

    public void NormalButtonSound()
    {
        m_SfxAudioSources[0].Play();
    }

    public void BackButtonSound()
    {
        m_SfxAudioSources[1].Play();
    }
}
