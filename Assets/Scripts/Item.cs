using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private float m_FallingSpeed = 1f;

    protected Rigidbody2D m_Rigidbody;

    protected abstract void OnGrab(GameObject i_Player);

    protected virtual void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.linearVelocityY = -m_FallingSpeed;
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        GameObject obj = i_Collision.gameObject;
        if (obj.CompareTag("Player")) OnGrab(obj);
    }
}
