using UnityEngine;
using System.Collections;

public class Fish : MoveableObject
{
    #region DataMembers
    // Audio
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_SfxAudioSources;

    // Score
    public int m_ScoreValue;

    // Shark Animation Images
    public int m_TextureToRender;
    public float m_AnimationTime, m_AnimationTimer;
    public Sprite[] m_AnimationTextures;
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
    }

    // Update is called once per frame
    override protected void Update()
    {
        if (PlayerPrefs.GetInt("Paused") == 1)
            return;

        base.Update();
    }

    override protected void OnBecameInvisible()
    {
        base.OnBecameInvisible();

        m_AnimationTimer -= Time.deltaTime;

        if (m_AnimationTimer <= 0.0f)
        {
            m_AnimationTimer = m_AnimationTime;
            m_TextureToRender++;

            if (m_TextureToRender >= m_AnimationTextures.Length)
                m_TextureToRender = 0;

            GetComponent<SpriteRenderer>().sprite = m_AnimationTextures[m_TextureToRender];
        }
    }

    protected void OnMouseOver()
    {
        GetInput();
    }

    protected void GetInput()
    {
        if (PlayerPrefs.GetInt("Paused") == 1)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + m_ScoreValue);
            Destroy(gameObject);
        }
    }
}
