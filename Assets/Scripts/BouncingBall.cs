using UnityEngine;

public abstract class BouncingBall : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D m_Rigidbody;
    [SerializeField] protected CapsuleCollider2D m_Collider;
    [SerializeField] protected float m_HorizontalVelocity = 0.6f;
    [SerializeField] protected float m_MaxBounce = 2f;

    protected abstract float BounceMultiplier { get; }


    // Vertical velocity required to reach the specific target height point
    protected virtual float BounceVelocity(float i_BottomPoint)
    {
        float topPoint = i_BottomPoint + m_Collider.size.y;
        if (topPoint > m_MaxBounce) return 0f;

        // Gravity might be scaled
        float gravity = Mathf.Abs(Physics2D.gravity.y * m_Rigidbody.gravityScale);

        // Y coordinates might be scaled as well
        float bounceHeight = m_MaxBounce * BounceMultiplier / transform.localScale.y;
        if (topPoint + bounceHeight > m_MaxBounce)
        {
            bounceHeight = m_MaxBounce - topPoint;
        }

        return Mathf.Sqrt(2 * gravity * bounceHeight);
    }

    protected virtual void Start()
    {
        if (m_Rigidbody == null) m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Collider == null) m_Collider = GetComponent<CapsuleCollider2D>();

        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_Rigidbody.linearVelocityX = m_HorizontalVelocity;

        // Assign fixed sorting order
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(transform.position.x * 100);
    }

    protected virtual void OnCollisionEnter2D(Collision2D i_Collision)
    {
        FixVelocities(i_Collision);
    }

    protected virtual void OnCollisionStay2D(Collision2D i_Collision)
    {
        FixVelocities(i_Collision);
    }

    protected virtual void FixVelocities(Collision2D i_Collision)
    {
        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            // Check for floor collision
            if (contact.normal.y > 0f)
            {
                OnFloorCollision(contact.point.y);
            }
            FixHorizontalVelocity(contact.normal.x);
        }
    }

    protected virtual void OnFloorCollision(float i_ContactPoint)
    {
        // Reset vertical velocity with static one
        m_Rigidbody.linearVelocityY = BounceVelocity(i_ContactPoint);
    }

    protected virtual void FixHorizontalVelocity(float i_NormalX)
    {
        float currentHorizontalVelocity = m_Rigidbody.linearVelocityX;
        if (Mathf.Abs(currentHorizontalVelocity) == m_HorizontalVelocity) return;

        float direction = Mathf.Sign(i_NormalX != 0f
            ? i_NormalX
            : currentHorizontalVelocity);

        m_Rigidbody.linearVelocityX = direction * m_HorizontalVelocity;
    }
}
