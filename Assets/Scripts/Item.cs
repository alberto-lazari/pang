using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected PlayerCollisionHandler m_Player;
    [SerializeField] private float m_FallingSpeed = 1f;

    protected Rigidbody2D m_Rigidbody;
    private int m_PlayerLayer;


    protected abstract void OnGrab();

    protected virtual void Awake()
    {
        if (m_Player == null) Debug.LogError("Player is not set");
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_PlayerLayer = LayerMask.NameToLayer("Player");

        m_Rigidbody.linearVelocityY = -m_FallingSpeed;
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.layer == m_PlayerLayer) OnGrab();
    }
}
