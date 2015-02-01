using UnityEngine;
using System.Collections;

public class MoveableObject : MonoBehaviour
{
    #region DataMembers
    // Movement
    public float m_MinMovementSpeed, m_MaxMovementSpeed, m_MovementSpeed, m_YPosition, m_TimeAlive, m_MinWaveMovementScale, m_MaxWaveMovementScale, m_WaveMovementScale;
    public bool m_IsMovingRight, m_IsMovingSin;
    #endregion

    #region Unity Functions
    // Use this for initialization
    virtual protected void Start()
    {
        m_MovementSpeed = Random.Range(m_MinMovementSpeed, m_MaxMovementSpeed);
        m_WaveMovementScale = Random.Range(m_MinWaveMovementScale, m_MaxWaveMovementScale);

        m_IsMovingRight = (transform.position.x < 0) ? true : false;
        m_IsMovingSin = (Random.Range(0, 2) == 0) ? true : false;

        m_YPosition = transform.position.y;

        //if(m_IsMovingRight)
        //    transform.Rotate(Vector3.forward, 180.0f * -1);

        //transform.localScale.x *= -1;
        if(m_IsMovingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //if (!Visible())
        //    DestroyObject(this);
        
        float deltaTime = Time.deltaTime;
        m_TimeAlive += deltaTime;

        Vector3 position = transform.position;
        position.Set(position.x + ((m_IsMovingRight) ? m_MovementSpeed * deltaTime : m_MovementSpeed * -deltaTime), m_YPosition + ((m_IsMovingSin) ? Mathf.Sin(m_TimeAlive) : Mathf.Cos(m_TimeAlive)) * m_WaveMovementScale, 0);
        transform.position = position;


    }

    virtual protected void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Functions
    #endregion
}
