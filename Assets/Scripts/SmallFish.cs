using UnityEngine;
using System.Collections;

public class SmallFish : MoveableObject
{
    #region DataMembers
    // Audio
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_SfxAudioSources;

    // Score
    public int m_ScoreValue;

    // Particle
    public ParticleSystem m_ParticleSystemPrefab, m_ParticleSystem;
    #endregion

    // Use this for initialization
    override protected void Start()
    {
        #region Create Audio Assets
        m_SfxAudioSources = new AudioSource[m_SfxAudioClips.Length];

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;

            m_SfxAudioSources[i] = child.AddComponent("AudioSource") as AudioSource;
            m_SfxAudioSources[i].clip = m_SfxAudioClips[i];
            m_SfxAudioSources[i].loop = false;
        }

        float fSFXVolume = PlayerPrefs.GetFloat("SFXVolume");

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSFXVolume / 100.0f;
        #endregion

        base.Start();

        m_ParticleSystem = (ParticleSystem)Instantiate(m_ParticleSystemPrefab, transform.position, Quaternion.identity);
        m_ParticleSystem.Play();
        m_ParticleSystem.renderer.sortingOrder = this.renderer.sortingOrder;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        m_ParticleSystem.transform.position = transform.position;
    }

    override protected void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        m_ParticleSystem.Stop();
        Destroy(m_ParticleSystem, 2.5f);
    }

    protected void OnMouseOver()
    {
        GetInput();
    }

    protected bool GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + m_ScoreValue);
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
