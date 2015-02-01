using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

    #region DataMembers
    // Audio
    public AudioClip[] m_MusicAudioClips;
    public AudioClip[] m_SfxAudioClips;
    private AudioSource[] m_MusicAudioSources;
    private AudioSource[] m_SfxAudioSources;

    // Enums
    enum MoveableObjectEnum { FISH, SHARK, SQUID };

    // Properties
    public MoveableObject[] m_MoveableObjects;
    public float m_SpawnRange;
    public float[] m_MinSpawnTimes, m_MaxSpawnTimes, m_SpawnTimers;
    #endregion

    // Use this for initialization
    void Start()
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

        for (int i = 0; i < m_MinSpawnTimes.Length; i++)
            m_SpawnTimers[i] = Random.Range(m_MinSpawnTimes[i], m_MaxSpawnTimes[i]);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_SpawnTimers.Length; i++)
        {
            m_SpawnTimers[i] -= Time.deltaTime;

            if (m_SpawnTimers[i] <= 0)
            {
                m_SpawnTimers[i] = Random.Range(m_MinSpawnTimes[i], m_MaxSpawnTimes[i]);

                float x = Random.Range(0, 2);

                if (x == 0)
                    x = -10;
                else
                    x = 10;

                float y = Random.Range(-m_SpawnRange, m_SpawnRange);

                Vector3 position = new Vector3(x, y, 0);
                Instantiate(m_MoveableObjects[i], position, Quaternion.identity);
            }
        }
    }
}
