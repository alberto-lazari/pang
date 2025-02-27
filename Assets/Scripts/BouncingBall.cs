using UnityEngine;

public abstract class BouncingBall : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D m_Rigidbody;
    [SerializeField] protected float m_HorizontalVelocity = 0.5f;

    protected abstract float TargetHeight { get; }


    // Vertical velocity required to reach the specific target height point
    protected virtual float BounceVelocity()
    {
        // Gravity might be scaled
        float gravity = Mathf.Abs(Physics2D.gravity.y * m_Rigidbody.gravityScale);
        // Y coordinates might be scaled as well
        float height = TargetHeight / transform.localScale.y;
        return Mathf.Sqrt(2 * gravity * height);
    }

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
        m_Rigidbody.linearVelocity = new Vector2(m_HorizontalVelocity, BounceVelocity());
    }

    protected virtual void OnCeilingCollision()
    {
        m_Rigidbody.linearVelocity = new Vector2(m_HorizontalVelocity, -m_Rigidbody.linearVelocityY);
    }
}
