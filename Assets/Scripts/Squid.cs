using UnityEngine;
using System.Collections;

public class Squid : MoveableObject
{
    #region DataMembers
    // Audio
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_SfxAudioSources;

    // Score Value
    public int m_ScoreValue;

    // Ink Splot
    public InkSplot m_InkSplot;
    public float m_MinInkTime, m_MaxInkTime, m_InkTimer;
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

        m_InkTimer = Random.Range(m_MinInkTime, m_MaxInkTime);

        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        m_InkTimer -= Time.deltaTime;

        if (m_InkTimer <= 0.0f)
        {
            m_InkTimer = Random.Range(m_MinInkTime, m_MaxInkTime);
            Instantiate(m_InkSplot, transform.position, Quaternion.identity);
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
