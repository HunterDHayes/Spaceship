////////////////////////////////////////////////////////////////////////////////////////////////////////
// File Name:   SoundManager.cs
// Author:      Hunter Hayes
// Purpose:     To manage all audio assets
////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region DataMembers
    // Prefixes for Audio
    public string m_GamePrefix, m_MusicPrefix, m_SfxPrefix;

    // Audio Volume
    public bool m_UseDefaultValuesAlways;
    public float m_DefaultMusicVolume, m_DefaultSfxVolume;

    // Audio
    //public AudioClip[] m_MusicAudioClips;
    //public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_MusicAudioSources;
    private AudioSource[] m_SfxAudioSources;

    // Needed for Pause and Resume
    private float[] m_MusicPauseTimes;
    private float[] m_SfxPauseTimes;

    // Needed for Times Looped
    private float[] m_MusicPlayTimes;
    private float[] m_SfxPlayTimes;
    private int[] m_MusicTimesLooped;
    private int[] m_SfxTimesLooped;

    // Music On/Off and SFX On/Off Buttons
    private GameObject m_MusicOnButton, m_MusicOffButton, m_SFXOnButton, m_SFXOffButton;

    // Bool for not starting twice
    private bool hasStarted = false;
    #endregion

    #region Unity Functions

    /// <summary>
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Used to initialize the Sound Manager before Start is called
    /// </summary>
    void Awake()
    {
        // First Half of if statement //
        // If first time playing, sets the default volume values of music and sound effects 
        // Second Half of if statement //
        // If true, sets the default volume of music and sound effects all the time 
        if (PlayerPrefs.GetInt("FirstTimePlaying") == 0 || m_UseDefaultValuesAlways)
        {
            PlayerPrefs.SetFloat("MusicVolume", m_DefaultMusicVolume);
            PlayerPrefs.SetFloat("SfxVolume", m_DefaultSfxVolume);
            PlayerPrefs.SetInt("FirstTimePlaying", 1);
        }

        // Create all the audio sources for the game to play audio
        #region Create Audio Assets
        float fMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float fSfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        Object[] music = Resources.LoadAll("Audio/Music", typeof(AudioClip));
        Object[] sfx = Resources.LoadAll("Audio/SFX", typeof(AudioClip));

        m_MusicAudioSources = new AudioSource[music.Length];

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;

            m_MusicAudioSources[i] = child.AddComponent<AudioSource>() as AudioSource;
            m_MusicAudioSources[i].clip = (AudioClip)music[i];
            m_MusicAudioSources[i].volume = fMusicVolume / 100.0f;
            m_MusicAudioSources[i].loop = true;
            m_MusicAudioSources[i].name = music[i].name;
        }

        m_SfxAudioSources = new AudioSource[sfx.Length];

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;

            m_SfxAudioSources[i] = child.AddComponent<AudioSource>() as AudioSource;
            m_SfxAudioSources[i].clip = (AudioClip)sfx[i];
            m_SfxAudioSources[i].volume = fSfxVolume / 100.0f;
            m_SfxAudioSources[i].loop = false;
            m_SfxAudioSources[i].name = sfx[i].name;
        }
        #endregion

        // Init pause times to -1 for apuse and resume
        m_MusicPauseTimes = new float[m_MusicAudioSources.Length];
        m_MusicPlayTimes = new float[m_MusicAudioSources.Length];
        m_MusicTimesLooped = new int[m_MusicAudioSources.Length];
        m_SfxPauseTimes = new float[m_SfxAudioSources.Length];
        m_SfxPlayTimes = new float[m_SfxAudioSources.Length];
        m_SfxTimesLooped = new int[m_SfxAudioSources.Length];

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            m_MusicPauseTimes[i] = -1;
            m_MusicPlayTimes[i] = -1;
            m_MusicTimesLooped[i] = -1;
        }

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            m_SfxPauseTimes[i] = -1;
            m_SfxPlayTimes[i] = -1;
            m_SfxTimesLooped[i] = -1;
        }

        SetUpMusicAndSfxButtons();
    }
    /// <summary>
    /// Updates the Sound Manager
    /// </summary>
    void Update()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicPlayTimes[i] > m_MusicAudioSources[i].time)
                m_MusicTimesLooped[i]++;

            m_MusicPlayTimes[i] = m_MusicAudioSources[i].time;
        }
    }

    public void TurnOnOffMusic(bool _value)
    {
        PlayerPrefs.SetFloat("MusicVolume", m_DefaultMusicVolume);

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = m_DefaultMusicVolume / 100.0f;

        if (_value)
        {
            PlayerPrefs.SetFloat("MusicVolume", 0);

            for (int i = 0; i < m_MusicAudioSources.Length; i++)
                m_MusicAudioSources[i].volume = 0 / 100.0f;
        }
    }

    public void TurnOnOffSfx(bool _value)
    {
        PlayerPrefs.SetFloat("SfxVolume", m_DefaultSfxVolume);

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = m_DefaultSfxVolume / 100.0f;

        if (_value)
        {
            PlayerPrefs.SetFloat("SfxVolume", 0);

            for (int i = 0; i < m_MusicAudioSources.Length; i++)
                m_SfxAudioSources[i].volume = 0 / 100.0f;
        }
    }
    #endregion

    #region Functions
    public void SetUpMusicAndSfxButtons()
    {
        float fMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float fSfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        // Find the objects with Music On/Off Button and SFX On/Off Button
        m_MusicOnButton = GameObject.FindGameObjectWithTag("MusicOnButton");
        m_MusicOffButton = GameObject.FindGameObjectWithTag("MusicOffButton");
        m_SFXOnButton = GameObject.FindGameObjectWithTag("SfxOnButton");
        m_SFXOffButton = GameObject.FindGameObjectWithTag("SfxOffButton");

        // Set Music Button to On or Off based on previous selection
        // If buttons not found, skip
        if (m_MusicOnButton && m_MusicOffButton)
        {
            if (fMusicVolume > 0)
            {
                m_MusicOnButton.gameObject.SetActive(true);
                m_MusicOffButton.gameObject.SetActive(false);
                TurnOnOffMusic(false);
            }
            else
            {
                m_MusicOnButton.gameObject.SetActive(false);
                m_MusicOffButton.gameObject.SetActive(true);
                TurnOnOffMusic(true);
            }
        }

        // Set SFX Button to On or Off based on previous selection
        // If buttons not found, skip
        if (m_SFXOnButton && m_SFXOffButton)
        {
            if (fSfxVolume > 0)
            {
                m_SFXOnButton.gameObject.SetActive(true);
                m_SFXOffButton.gameObject.SetActive(false);
                TurnOnOffSfx(false);
            }
            else
            {
                m_SFXOnButton.gameObject.SetActive(false);
                m_SFXOffButton.gameObject.SetActive(true);
                TurnOnOffSfx(true);
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Play Functions
    /// <summary>
    /// Play specified music.
    /// </summary>
    /// <param name="_MusicName">Specified Music</param>
    public void PlayMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].Play();
                m_MusicPauseTimes[i] = -1;
                break;
            }
        }
    }
    /// <summary>
    /// Play specified sound effect
    /// </summary>
    /// <param name="_SfxName"></param>
    public void PlaySfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].Play();
                m_SfxPauseTimes[i] = -1;
                break;
            }
        }
    }
    /// <summary>
    /// Play all music
    /// </summary>
    public void PlayAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            m_MusicAudioSources[i].Play();
            m_MusicPauseTimes[i] = -1;
        }
    }
    /// <summary>
    /// Play all sound effects
    /// </summary>
    public void PlayAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            m_SfxAudioSources[i].Play();
            m_SfxPauseTimes[i] = -1;
        }
    }
    /// <summary>
    /// Play random music
    /// </summary>
    public void PlayRandomMusic()
    {
        if (m_MusicAudioSources.Length > 0)
        {
            int rand = Random.Range(0, m_MusicAudioSources.Length);
            m_MusicAudioSources[rand].Play();
            m_MusicPauseTimes[rand] = -1;
        }
    }
    /// <summary>
    /// Play random sound effect
    /// </summary>
    public void PlayRandomSfx()
    {
        if (m_SfxAudioSources.Length > 0)
        {
            int rand = Random.Range(0, m_SfxAudioSources.Length);
            m_SfxAudioSources[rand].Play();
            m_SfxPauseTimes[rand] = -1;
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Stop Functions
    /// <summary>
    /// Stop specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    public void StopMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].Stop();
                m_MusicPauseTimes[i] = -1;
                break;
            }
        }
    }
    /// <summary>
    /// Stop specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect Name</param>
    public void StopSfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].Stop();
                m_SfxPauseTimes[i] = -1;
                break;
            }
        }
    }
    /// <summary>
    /// Stop all music
    /// </summary>
    public void StopAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            m_MusicAudioSources[i].Stop();
            m_MusicPauseTimes[i] = -1;
        }
    }
    /// <summary>
    /// Stop all sound effects
    /// </summary>
    public void StopAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            m_SfxAudioSources[i].Stop();
            m_SfxPauseTimes[i] = -1;
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Is Sound Playing Functions
    /// <summary>
    /// Is the specified music playing
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    /// <returns>
    /// Returns true if the specified music is playing
    /// Returns false if the specified music is not playing or not found
    /// </returns>
    public bool IsMusicPlaying(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].isPlaying && m_MusicAudioSources[i].name == _MusicName)
                return true;

        return false;
    }
    /// <summary>
    /// Is specified sound efffect playing
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect Name</param>
    /// <returns>
    /// Returns true is the specified sound effect is playing
    /// Returns false if the specified sound effect is not playing or not found
    /// </returns>
    public bool IsSfxPlaying(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].isPlaying && m_SfxAudioSources[i].name == _SfxName)
                return true;

        return false;
    }
    /// <summary>
    /// Is any music playing
    /// </summary>
    /// <returns>
    /// Returns true if any music is playing
    /// Returns false is any music is not playing
    /// </returns>
    public bool IsAnyMusicPlaying()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].isPlaying)
                return true;

        return false;
    }
    /// <summary>
    /// Is any sound effect playing
    /// </summary>
    /// <returns>
    /// Returns true if any sound effect is playing
    /// Returns false if any sound effect is not playing
    /// </returns>
    public bool IsAnySfxPlaying()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].isPlaying)
                return true;

        return false;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Audio Length Functions
    /// <summary>
    /// Gets the length of the specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name.</param>
    /// <returns>
    /// Returns the length of the specified music.
    /// Returns -1 if the specified music is not found
    /// </returns>
    public float GetMusicLength(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].name == _MusicName)
                return m_MusicAudioSources[i].clip.length;

        return -1;
    }
    // Return specified sound effect length
    /// <summary>
    /// Gets the length of the specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect Name</param>
    /// <returns>
    /// Returns the length of the specified sound effect
    /// Returns -1 if the specified sound effect doesn't exist
    /// </returns>
    public float GetSfxLength(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].name == _SfxName)
                return m_SfxAudioSources[i].clip.length;

        return -1;
    }
    /// <summary>
    /// Get the minimum length between all music
    /// </summary>
    /// <returns>
    /// Returns the minimum length of all music
    /// Returns -1 is no music exists
    /// </returns>
    public float GetMinMusicLength()
    {
        if (m_MusicAudioSources.Length == 0)
            return -1;

        float minTime = m_MusicAudioSources[0].clip.length;
        int minMusic = 0;

        for (int i = 1; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].clip.length < m_MusicAudioSources[minMusic].clip.length)
            {
                minTime = m_MusicAudioSources[i].clip.length;
                minMusic = i;
            }
        }

        return minTime;
    }
    /// <summary>
    /// Gets the maximum length of all music
    /// </summary>
    /// <returns>
    /// Returns the maximum length of all music
    /// Returns -1 if no music exists
    /// </returns>
    public float GetMaxMusicLength()
    {
        if (m_MusicAudioSources.Length == 0)
            return -1;

        float maxTime = m_MusicAudioSources[0].clip.length;
        int maxMusic = 0;

        for (int i = 1; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].clip.length > m_MusicAudioSources[maxMusic].clip.length)
            {
                maxTime = m_MusicAudioSources[i].clip.length;
                maxMusic = i;
            }
        }

        return maxTime;
    }
    /// <summary>
    /// Gets the minimum length for all sound effects
    /// </summary>
    /// <returns>
    /// Returns the minimum length for all sound effects
    /// Returns -1 if no sound effects exist
    /// </returns>
    public float GetMinSfxLength()
    {
        if (m_SfxAudioSources.Length == 0)
            return -1;

        float minTime = m_SfxAudioSources[0].clip.length;
        int minSfx = 0;

        for (int i = 1; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].clip.length < m_SfxAudioSources[minSfx].clip.length)
            {
                minTime = m_SfxAudioSources[i].clip.length;
                minSfx = i;
            }
        }

        return minTime;
    }
    /// <summary>
    /// Gets the maximum length for all sound effects
    /// </summary>
    /// <returns>
    /// Returns the maximum length for all sound effects
    /// Returns -1 if no sound effects exist
    /// </returns>
    public float GetMaxSfxLength()
    {
        if (m_SfxAudioSources.Length == 0)
            return -1;

        float maxTime = m_SfxAudioSources[0].clip.length;
        int maxSfx = 0;

        for (int i = 1; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].clip.length < m_SfxAudioSources[maxSfx].clip.length)
            {
                maxTime = m_SfxAudioSources[i].clip.length;
                maxSfx = i;
            }
        }

        return maxTime;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Elapsed Time Functions
    /// <summary>
    /// Gets how long specified music has been playing
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    /// <returns>
    /// Returns how long specified music has been playing
    /// Returns 0 if the specified music is not playing
    /// Returns -1 if the specified music is not playing or doesn't exist
    /// </returns>
    public float GetMusicPlaybackTime(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                if (m_MusicAudioSources[i].isPlaying)
                    return m_MusicAudioSources[i].time;

                return 0;
            }
        }

        return -1;
    }
    /// <summary>
    /// Gets how long specified sound effect has been playing
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect name</param>
    /// <returns>
    /// Returns how long specified sound effect has been playing
    /// Returns 0 if the specified sound effect is not playing
    /// Returns -1 if the specified sound effect doesn't exist
    /// </returns>
    public float GetSfxPlaybackTime(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                if (m_SfxAudioSources[i].isPlaying)
                    return m_SfxAudioSources[i].time;

                return 0;
            }
        }

        return -1;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Looping Functions
    /// <summary>
    /// Set looping of specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    /// <param name="_Looping">Value to set looping of specified music</param>
    public void SetMusicLoop(string _MusicName, bool _Looping)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].loop = _Looping;
                return;
            }
        }
    }
    /// <summary>
    /// Set looping is specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect Name</param>
    /// <param name="_Looping">Value to set looping of specified sound effect</param>
    public void SetSfxLoop(string _SfxName, bool _Looping)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].loop = _Looping;
                return;
            }
        }
    }
    /// <summary>
    /// Set looping of all music
    /// </summary>
    /// <param name="_Looping">Value to set looping of all music</param>
    public void SetAllMusicLoop(bool _Looping)
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].loop = _Looping;
    }
    /// <summary>
    /// Set looping of all sound effect
    /// </summary>
    /// <param name="_Looping">Value to set looping of all sound effect</param>
    public void SetAllSfxLoop(bool _Looping)
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].loop = _Looping;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Set Volume Functions
    /// <summary>
    /// Set the volume of specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    /// <param name="_Volume">Value to set volume of specified music</param>
    public void SetMusicVolume(string _MusicName, float _Volume)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].volume = _Volume;
                return;
            }
        }
    }
    /// <summary>
    /// Set the volume of specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect</param>
    /// <param name="_Volume">Value to set volume of specified sound effect</param>
    public void SetSfxVolume(string _SfxName, float _Volume)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].volume = _Volume;
                return;
            }
        }
    }
    /// <summary>
    /// Set volume of all music
    /// </summary>
    /// <param name="_Volume">Value to set volume of all music</param>
    public void SetAllMusicVolume(float _Volume)
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = _Volume;
    }
    /// <summary>
    /// Set volume of all souund effects
    /// </summary>
    /// <param name="_Volume">Value to set volume of all sound effects</param>
    public void SetAllSfxVolume(float _Volume)
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = _Volume;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Get Volume Functions
    /// <summary>
    /// Get volume of specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    /// <returns>
    /// Returns the volume of the specified music
    /// Returns -1 if the specified music doesn't exist
    /// </returns>
    public float GetMusicVolume(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].name == _MusicName)
                return m_MusicAudioSources[i].volume;

        return -1;
    }
    /// <summary>
    /// Get volume of specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect Name</param>
    /// <returns>
    /// Returns the volume of the specified sound effect
    /// Returns -1 if the specified sound effect doesn't exist
    /// </returns>
    public float GetSfxVolume(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].name == _SfxName)
                return m_SfxAudioSources[i].volume;

        return -1;
    }
    /// <summary>
    /// Get minimum volume of all music
    /// </summary>
    /// <returns>
    /// Returns minimum volume of all music
    /// Returns -1 if no music exists
    /// </returns>
    public float GetMinMusicVolume()
    {
        if (m_MusicAudioSources.Length == 0)
            return -1;

        float minVolume = m_MusicAudioSources[0].volume;
        int minMusic = 0;

        for (int i = 1; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].volume < m_MusicAudioSources[minMusic].volume)
            {
                minVolume = m_MusicAudioSources[i].volume;
                minMusic = i;
            }
        }

        return minVolume;
    }
    /// <summary>
    /// Get maximum volume of all music
    /// </summary>
    /// <returns>
    /// Returns maximum volume of all music
    /// Returns -1 if no music exists
    /// </returns>
    public float GetMaxMusicVolume()
    {
        if (m_MusicAudioSources.Length == 0)
            return -1;

        float maxVolume = m_MusicAudioSources[0].volume;
        int maxMusic = 0;

        for (int i = 1; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].volume > m_MusicAudioSources[maxMusic].volume)
            {
                maxVolume = m_MusicAudioSources[i].volume;
                maxMusic = i;
            }
        }

        return maxVolume;
    }
    /// <summary>
    /// Get minimum volume of all sound effects
    /// </summary>
    /// <returns>
    /// Returns minimum volume of all sound effects
    /// Returns -1 if no sound effects exists
    /// </returns>
    public float GetMinSfxVolume()
    {
        if (m_SfxAudioSources.Length == 0)
            return -1;

        float minVolume = m_SfxAudioSources[0].volume;
        int minSfx = 0;

        for (int i = 1; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].volume < m_SfxAudioSources[minSfx].volume)
            {
                minVolume = m_SfxAudioSources[i].volume;
                minSfx = i;
            }
        }

        return minVolume;
    }
    /// <summary>
    /// Get maximum volume of all sound effects
    /// </summary>
    /// <returns>
    /// Returns maximum volume of all sound effects
    /// Returns -1 if no sound effects exists
    /// </returns>
    public float GetMaxSfxVolume()
    {
        if (m_SfxAudioSources.Length == 0)
            return -1;

        float maxVolume = m_SfxAudioSources[0].volume;
        int maxSfx = 0;

        for (int i = 1; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].volume > m_SfxAudioSources[maxSfx].volume)
            {
                maxVolume = m_SfxAudioSources[i].volume;
                maxSfx = i;
            }
        }

        return maxVolume;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Mute Functions
    /// <summary>
    /// Gets mute of specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    /// <returns>
    /// Returns the mute value of the specified music
    /// Returns false if specified music doesn't exist
    /// </returns>
    public bool GetMuteMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].name == _MusicName)
                return m_MusicAudioSources[i].mute;

        return false;
    }
    /// <summary>
    /// Sets mute of specified Music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    /// <param name="_Mute">Value to set mute of specified music</param>
    public void SetMuteMusic(string _MusicName, bool _Mute)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].mute = _Mute;
                return;
            }
        }
    }
    /// <summary>
    /// Mutes specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    public void MuteMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].mute = true;
                return;
            }
        }
    }
    /// <summary>
    /// Unmutes specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    public void UnmuteMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].mute = false;
                return;
            }
        }
    }
    /// <summary>
    /// Toggle specified Music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    public void ToggleMuteMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].mute = !m_MusicAudioSources[i].mute;
                return;
            }
        }
    }
    /// <summary>
    /// Mute all music
    /// </summary>
    public void MuteAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].mute = true;
    }
    /// <summary>
    /// Unmute all music
    /// </summary>
    public void UnmuteAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].mute = false;
    }
    /// <summary>
    /// Toggle mute all music
    /// </summary>
    public void ToggleMuteAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].mute = !m_MusicAudioSources[i].mute;
    }
    /// <summary>
    /// Gets mute of specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified sound effect Name</param>
    /// <returns>
    /// Returns the mute value of the specified sound effect
    /// Returns false if specified sound effect doesn't exist
    /// </returns>
    public bool GetMuteSfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].name == _SfxName)
                return m_SfxAudioSources[i].mute;

        return false;
    }
    /// <summary>
    /// Sets mute of specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified sound effect Name</param>
    /// <param name="_Mute">Value to set mute of specified sound effect</param>
    public void SetMuteSfx(string _SfxName, bool _Mute)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].mute = _Mute;
                return;
            }
        }
    }
    /// <summary>
    /// Mutes specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified sound effect Name</param>
    public void MuteSfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].mute = true;
                return;
            }
        }
    }
    /// <summary>
    /// Unmutes specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified sound effect Name</param>
    public void UnmuteSfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].mute = false;
                return;
            }
        }
    }
    /// <summary>
    /// Toggle specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified sound effect Name</param>
    public void ToggleMuteSfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].mute = !m_SfxAudioSources[i].mute;
                return;
            }
        }
    }
    /// <summary>
    /// Mute all sound effects
    /// </summary>
    public void MuteAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].mute = true;
    }
    /// <summary>
    /// Unmute all sound effects
    /// </summary>
    public void UnmuteAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].mute = false;
    }
    /// <summary>
    /// Toggle mute all sound effects
    /// </summary>
    public void ToggleMuteAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].mute = !m_SfxAudioSources[i].mute;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Play at Start Time Functions
    /// <summary>
    /// Play specified music.
    /// </summary>
    /// <param name="_MusicName">Specified Music</param>
    /// <param name="_StartTime">Specified Start Time</param>
    public void PlayMusicAtStartTime(string _MusicName, float _StartTime)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                PlayMusic(_MusicName);
                m_MusicAudioSources[i].time = _StartTime;
                return;
            }
        }
    }
    /// <summary>
    /// Play specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect Name</param>
    /// <param name="_StartTime">Specified Start Time</param>
    public void PlaySfxAtStartTime(string _SfxName, float _StartTime)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                PlaySfx(_SfxName);
                m_SfxAudioSources[i].time = _StartTime;
                return;
            }
        }
    }
    /// <summary>
    /// Play all music
    /// </summary>
    /// <param name="_StartTime">Specified Start Time</param>
    public void PlayAllMusicAtStartTime(float _StartTime)
    {
        PlayAllMusic();

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].time = _StartTime;
    }
    /// <summary>
    /// Play all sound effects
    /// </summary>
    /// <param name="_StartTime">Specified Start Time</param>
    public void PlayAllSfxAtStartTime(float _StartTime)
    {
        PlayAllSfx();

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].time = _StartTime;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Pause
    /// <summary>
    /// Pause specified music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    public void PauseMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                if (m_MusicAudioSources[i].isPlaying)
                {
                    m_MusicPauseTimes[i] = m_MusicAudioSources[i].time;
                    m_MusicAudioSources[i].Pause();
                }

                return;
            }
        }
    }
    /// <summary>
    /// Pause specified sound effect
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect Name</param>
    public void PauseSfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                if (m_SfxAudioSources[i].isPlaying)
                {
                    m_SfxPauseTimes[i] = m_SfxAudioSources[i].time;
                    m_SfxAudioSources[i].Pause();
                }

                return;
            }
        }
    }
    /// <summary>
    /// Pause all music
    /// </summary>
    public void PauseAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].isPlaying)
            {
                m_MusicPauseTimes[i] = m_MusicAudioSources[i].time;
                m_MusicAudioSources[i].Pause();
            }
        }
    }
    /// <summary>
    /// Pause all sound effects
    /// </summary>
    public void PauseAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].isPlaying)
            {
                m_SfxPauseTimes[i] = m_SfxAudioSources[i].time;
                m_SfxAudioSources[i].Pause();
            }
        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Resume
    /// <summary>
    /// Resume specified paused music
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    public void ResumeMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                if (m_MusicPauseTimes[i] > 0)
                    PlayMusicAtStartTime(_MusicName, m_MusicPauseTimes[i]);

                return;
            }
        }
    }
    /// <summary>
    /// Resume specified paused sound effect
    /// </summary>
    /// <param name="_SfxName">Specified Sound Effect Name</param>
    public void ResumeSfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                if (m_SfxPauseTimes[i] > 0)
                    PlaySfxAtStartTime(_SfxName, m_SfxPauseTimes[i]);

                return;
            }
        }
    }
    /// <summary>
    /// Resume all paused music
    /// </summary>
    public void ResumeAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicPauseTimes[i] > 0)
            {
                string _MusicName = m_MusicAudioSources[i].name;
                _MusicName = _MusicName.Substring(m_GamePrefix.Length + m_MusicPrefix.Length + 2);
                PlayMusicAtStartTime(_MusicName, m_MusicPauseTimes[i]);
            }
        }
    }
    /// <summary>
    /// Resume all paused sound effects
    /// </summary>
    public void ResumeAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxPauseTimes[i] > 0)
            {
                string _SfxName = m_SfxAudioSources[i].name;
                _SfxName = _SfxName.Substring(m_GamePrefix.Length + m_SfxPrefix.Length + 2);
                PlaySfxAtStartTime(_SfxName, m_SfxPauseTimes[i]);
            }

        }
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    // Times Looped
    #region Times Looped
    /// <summary>
    /// Returns the number of times the specified music has looped
    /// </summary>
    /// <param name="_MusicName">Specified Music Name</param>
    /// <returns>
    /// Returns the number of times the specified music has looped
    /// Return -1 if the specified music isn't playing or doesn't exist
    /// </returns>
    public int GetMusicTimesLooped(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {

            }
        }

        return -1;
    }
    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    // Pitch

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    // Tempo

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    // Delayed Play

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    // Slowly Increasing/Decreasing Temp

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    #endregion
}