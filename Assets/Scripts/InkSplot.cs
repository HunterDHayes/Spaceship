using UnityEngine;
using System.Collections;

public class InkSplot : MonoBehaviour
{
        #region DataMembers
    // Audio
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_SfxAudioSources;

    // Ink Splot
    public float m_MinInkTime, m_MaxInkTime, m_InkTime, m_InkTimer;
    public ParticleSystem m_ParticleSystemPrefab, m_ParticleSystem;
    #endregion

    // Use this for initialization
    void Start()
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

        float fSfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSfxVolume / 100.0f;
        #endregion

        m_SfxAudioSources[0].Play();

        m_InkTime = Random.Range(m_MinInkTime, m_MaxInkTime);
        m_InkTimer = m_InkTime;

        //m_ParticleSystem = (ParticleSystem)Instantiate(m_ParticleSystemPrefab, transform.position, Quaternion.identity);
        //m_ParticleSystem.Play();
        //m_ParticleSystem.renderer.sortingOrder = this.renderer.sortingOrder;
        //DestroyObject(m_ParticleSystem, m_InkTime + 5.0f);
    }

    // Update is called once per frame
    protected void Update()
    {
        float fSfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSfxVolume / 100.0f;
     
        if (PlayerPrefs.GetInt("Paused") == 1)
            return;

        m_InkTimer -= Time.deltaTime;

        if (m_InkTimer <= 0.0f)
        {
            //m_ParticleSystem.Stop(true);
            Destroy(gameObject);
        }

        Color color = this.GetComponent<SpriteRenderer>().color;
        color.a = m_InkTimer / m_InkTime * 2.0f;
        this.GetComponent<SpriteRenderer>().color = color;
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
            Destroy(gameObject);
    }
}
