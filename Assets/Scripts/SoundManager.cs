using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    #region DataMembers
    // Prefixes for Audio
    public string m_GamePrefix, m_MusicPrefix, m_SfxPrefix;

    // Audio
    public AudioClip[] m_MusicAudioClips;
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_MusicAudioSources;
    private AudioSource[] m_SfxAudioSources;
    #endregion

    #region Unity Functions
    // Use this for initialization
    void Start()
    {
        #region Create Audio Assets
        float fMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float fSfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        m_MusicAudioSources = new AudioSource[m_MusicAudioClips.Length];

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;

            m_MusicAudioSources[i] = child.AddComponent("AudioSource") as AudioSource;
            m_MusicAudioSources[i].clip = m_MusicAudioClips[i];
            m_MusicAudioSources[i].volume = fMusicVolume / 100.0f;
            m_MusicAudioSources[i].loop = true;
            m_MusicAudioSources[i].name = m_MusicAudioClips[i].name;
        }

        m_SfxAudioSources = new AudioSource[m_SfxAudioClips.Length];

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;

            m_SfxAudioSources[i] = child.AddComponent("AudioSource") as AudioSource;
            m_SfxAudioSources[i].clip = m_SfxAudioClips[i];
            m_SfxAudioSources[i].volume = fSfxVolume / 100.0f;
            m_SfxAudioSources[i].loop = false;
            m_SfxAudioSources[i].name = m_SfxAudioClips[i].name;
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        float fMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float fSfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = fMusicVolume / 100.0f;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSfxVolume / 100.0f;
    }
    #endregion

    #region Functions
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Play Sounds Functions
    /// <summary>Play specified music.</summary>
    /// <param name="_MusicName">Specified Music</param>
    public void PlayMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].Play();
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
                break;
            }
        }
    }
    // Play all music
    public void PlayAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].Play();
    }
    // Play all sound effects
    public void PlayAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].Play();
    }
    // Play random music
    public void PlayRandomMusic()
    {
        if (m_MusicAudioSources.Length > 0)
            m_MusicAudioSources[Random.Range(0, m_MusicAudioSources.Length)].Play();
    }
    // Play random sound effect
    public void PlayRandomSfx()
    {
        if (m_SfxAudioSources.Length > 0)
            m_SfxAudioSources[Random.Range(0, m_SfxAudioSources.Length)].Play();
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Stop Sounds Functions
    // Stop specified music
    public void StopMusic(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
            {
                m_MusicAudioSources[i].Stop();
                break;
            }
        }
    }
    // Stop specified sound effect
    public void StopSfx(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
            {
                m_SfxAudioSources[i].Stop();
                break;
            }
        }
    }
    // Stop all music
    public void StopAllMusic()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].Stop();
    }
    // Stop all sound effects
    public void StopAllSfx()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].Stop();
    }
    #endregion
    #region Is Sound Playing Functions
    // Is specified music playing
    public bool IsMusicPlaying(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].isPlaying && m_MusicAudioSources[i].name == _MusicName)
                return true;

        return false;
    }
    // Is specified sound effect playing
    public bool IsSfxPlaying(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].isPlaying && m_SfxAudioSources[i].name == _SfxName)
                return true;

        return false;
    }
    // Is any music playing
    public bool IsAnyMusicPlaying()
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].isPlaying)
                return true;

        return false;
    }
    // Is any sound effect playing
    public bool IsAnySfxPlaying()
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].isPlaying)
                return true;

        return false;
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    // Mute
    // Pause/Resume
    // Audio Length
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Audio Length Functions
    /// <summary>
    ///  Gets the length of the music with the specified name.
    /// </summary>
    /// <param name="_MusicName">Specified Music Name.</param>
    /// <returns>Returns the length of the specified music.</returns>
    public float GetMusicLength(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].name == _MusicName)
                return m_MusicAudioSources[i].clip.length;

        return -1;
    }
    // Return specified sound effect length
    public float GetSfxLength(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].name == _SfxName)
                return m_SfxAudioSources[i].clip.length;

        return -1;
    }
    // Return min music length
    public float GetMinMusicLength()
    {
        if (m_MusicAudioSources.Length == 0)
            return 0;

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
    // Return max music length
    public float GetMaxMusicLength()
    {
        if (m_MusicAudioSources.Length == 0)
            return 0;

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
    // Return min sound effect length
    public float GetMinSfxLength()
    {
        if (m_SfxAudioSources.Length == 0)
            return 0;

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
    // Return max sound effect length
    public float GetMaxSfxLength()
    {
        if (m_SfxAudioSources.Length == 0)
            return 0;

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
    // Return how long specified music has been playing
    public float GetMusicPlaybackTime(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
        {
            if (m_MusicAudioSources[i].name == _MusicName)
                return m_MusicAudioSources[i].time;
        }

        return -1;
    }
    // Return how long specified sound effect has been playing
    public float GetSfxPlaybackTime(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            if (m_SfxAudioSources[i].name == _SfxName)
                return m_SfxAudioSources[i].time;
        }

        return -1;
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Looping Functions
    // Set Loop of specified music
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
    // Set Loop of specified sound effect
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
    // Set Loop of all music
    public void SetAllMusicLoop(bool _Looping)
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].loop = _Looping;
    }
    // Set Loop of all sound effect
    public void SetAllSfxLoop(bool _Looping)
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].loop = _Looping;
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // Times Looped
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Set Volume Functions
    // Set volume of specified music
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
    // Set volume of specified sound effect
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
    // Set volume of all music
    public void SetAllMusicVolume(float _Volume)
    {
        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = _Volume;
    }
    // Set volume of all sound effects
    public void SetAllSfxVolume(float _Volume)
    {
        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = _Volume;
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Get Volume Functions
    // Get volume of specified music
    public float GetMusicVolume(string _MusicName)
    {
        _MusicName = m_GamePrefix + "_" + m_MusicPrefix + "_" + _MusicName;

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            if (m_MusicAudioSources[i].name == _MusicName)
                return m_MusicAudioSources[i].volume;

        return -1;
    }
    // Get volume of specified sound effect
    public float GetSfxVolume(string _SfxName)
    {
        _SfxName = m_GamePrefix + "_" + m_SfxPrefix + "_" + _SfxName;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            if (m_SfxAudioSources[i].name == _SfxName)
                return m_SfxAudioSources[i].volume;

        return -1;
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // Pitch
    // Tempo
    // Chose Start Time
    // Delayed Play
    // Slowly Increasing/Decreasing Temp (Extra)
    #endregion
}