using UnityEngine;
using System.Collections;

public class Shark : MoveableObject
{
    #region DataMembers
    // Audio
    public AudioClip[] m_MusicAudioClips;
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_MusicAudioSources;
    private AudioSource[] m_SfxAudioSources;
    #endregion

    // Use this for initialization
    override protected void Start()
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

        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }

    override protected void OnBecameInvisible()
    {
        base.OnBecameInvisible();
    }

    protected void OnMouseOver()
    {
        Debug.Log("MouseOver Shark");
        GetInput();
    }

    protected bool GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("GetInput Shark");
            GameObject manager = GameObject.FindGameObjectWithTag("MainCamera");
            manager.SendMessage("LoseLive");
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
