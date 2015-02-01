﻿using UnityEngine;
using System.Collections;

public class Pufferfish : MoveableObject
{
    #region DataMembers
    // Audio
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_SfxAudioSources;

    // Rotation Properties
    public float m_RotationScale;

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
        float fSFXVolume = PlayerPrefs.GetFloat("SFXVolume");

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSFXVolume / 100.0f;
     
        if (PlayerPrefs.GetInt("Paused") == 1)
            return;

        base.Update();

        transform.Rotate(Vector3.forward, Mathf.Cos(m_TimeAlive) * m_RotationScale);

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

    override protected void OnBecameInvisible()
    {
        base.OnBecameInvisible();
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
            ParticleSystem ps = (ParticleSystem)Instantiate(m_DeathParticleSystemPrefab, transform.position, Quaternion.identity);
            ps.startColor = m_ParticleColor;
            ps.renderer.sortingOrder = 1;
            ps.Play();
            DestroyObject(ps, 1.0f);

            GameObject manager = GameObject.FindGameObjectWithTag("MainCamera");
            if (manager)
                manager.SendMessage("LoseLive");
            Destroy(gameObject);
        }
    }
}
