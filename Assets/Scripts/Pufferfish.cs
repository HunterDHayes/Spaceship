using UnityEngine;
using System.Collections;

public class Pufferfish : MoveableObject
{
    #region DataMembers
    // Audio
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_SfxAudioSources;

    // Rotation Properties
    public float m_RotationScale;
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
        base.Update();

        transform.Rotate(Vector3.forward, Mathf.Cos(m_TimeAlive) * m_RotationScale);
    }

    override protected void OnBecameInvisible()
    {
        base.OnBecameInvisible();
    }

    protected void OnMouseOver()
    {
        GetInput();
    }

    protected bool GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject manager = GameObject.FindGameObjectWithTag("MainCamera");
            manager.SendMessage("LoseLive");
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
