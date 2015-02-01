using UnityEngine;
using System.Collections;

public class DestroyParticleSystem : MonoBehaviour
{
    #region DataMembers
    // Destroy Timer
    public float m_DestroyTime, m_DestroyTimer;
    #endregion

    #region Unity Functions
    // Use this for initialization
    void Start()
    {
        m_DestroyTimer = m_DestroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        m_DestroyTimer -= Time.deltaTime;

        if (m_DestroyTimer <= 0.0f)
            Destroy(gameObject);
    }
    #endregion
}
