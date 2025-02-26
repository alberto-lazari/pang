using UnityEngine;

public abstract class BouncingBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private float m_HorizontalVelocity = 0.5f;

    protected abstract float BounceVelocity { get; }

    protected virtual void Start()
    {
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_Rigidbody.linearVelocityX = m_HorizontalVelocity;
        // Assign fixed sorting order
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(transform.position.x * 100);
    }

    protected virtual void OnCollisionEnter2D(Collision2D i_Collision)
    {
        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            Vector2 point = transform.InverseTransformPoint(contact.point);
            if (Mathf.Abs(point.x) > Mathf.Abs(point.y))
            {
                OnHorizontalCollision();
            }
            else if (point.y < 0f)
            {
                OnFloorCollision();
            }
            else
            {
                OnCeilingCollision();
            }
        }
    }

    protected virtual void OnHorizontalCollision()
    {
        // Change direction by inverting velocity
        m_HorizontalVelocity *= -1f;
        m_Rigidbody.linearVelocityX = m_HorizontalVelocity;
    }

    protected virtual void OnFloorCollision()
    {
        // Reset vertical velocity with static one
        m_Rigidbody.linearVelocity = new Vector2(m_HorizontalVelocity, BounceVelocity);
    }

    protected virtual void OnCeilingCollision()
    {
        m_Rigidbody.linearVelocity = new Vector2(m_HorizontalVelocity, -m_Rigidbody.linearVelocityY);
    }
}
