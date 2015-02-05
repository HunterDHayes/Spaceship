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

    // Properties
    public MoveableObject[] m_MoveableObjects;
    public float m_SpawnRange, m_SpawnPosition;
    public float[] m_EasyMinSpawnTimes, m_EasyMaxSpawnTimes, m_NormalMinSpawnTimes, m_NormalMaxSpawnTimes, m_HardMinSpawnTimes, m_HardMaxSpawnTimes;
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
        float fSfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = fMusicVolume / 100.0f;

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSfxVolume / 100.0f;
        #endregion

        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Hard":
                {
                    m_MinSpawnTimes = m_HardMinSpawnTimes;
                    m_MaxSpawnTimes = m_HardMaxSpawnTimes;
                    break;
                }
            case "Medium":
                {
                    m_MinSpawnTimes = m_NormalMinSpawnTimes;
                    m_MaxSpawnTimes = m_NormalMaxSpawnTimes;
                    break;
                }
            case "Easy":
            default:
                {
                    m_MinSpawnTimes = m_EasyMinSpawnTimes;
                    m_MaxSpawnTimes = m_EasyMaxSpawnTimes;
                    break;
                }
        }

        m_SpawnTimers = new float[m_MinSpawnTimes.Length];

        for (int i = 0; i < m_MinSpawnTimes.Length; i++)
            m_SpawnTimers[i] = Random.Range(m_MinSpawnTimes[i], m_MaxSpawnTimes[i]);
    }

    // Update is called once per frame
    void Update()
    {
        float fMusicVolume = PlayerPrefs.GetFloat("MusicVolume");

        for (int i = 0; i < m_MusicAudioSources.Length; i++)
            m_MusicAudioSources[i].volume = fMusicVolume / 100.0f;

        float fSfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        for (int i = 0; i < m_SfxAudioSources.Length; i++)
            m_SfxAudioSources[i].volume = fSfxVolume / 100.0f;
     
        if (PlayerPrefs.GetInt("Paused") == 1)
            return;

        for (int i = 0; i < m_SpawnTimers.Length; i++)
        {
            m_SpawnTimers[i] -= Time.deltaTime;

            if (m_SpawnTimers[i] <= 0)
            {
                m_SpawnTimers[i] = Random.Range(m_MinSpawnTimes[i], m_MaxSpawnTimes[i]);

                float x = Random.Range(0, 2);

                if (x == 0)
                    x = -m_SpawnPosition;
                else
                    x = m_SpawnPosition;

                float y = Random.Range(transform.position.y + -m_SpawnRange, transform.position.y + m_SpawnRange);

                Vector3 position = new Vector3(x, y, 0);
                Instantiate(m_MoveableObjects[i], position, Quaternion.identity);
            }
        }
    }
}
