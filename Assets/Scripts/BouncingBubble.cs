using UnityEngine;

public class BouncingBubble : MonoBehaviour
{
    private static readonly int ExplodingTriggerHash = Animator.StringToHash("Exploding");

    [SerializeField] private GameObject m_SmallerBall = null;

    [SerializeField] private float m_BounceMultiplier;
    [SerializeField] private float m_MaxBounce = 2f;

    [SerializeField] private float m_HorizontalVelocity = 0.6f;
    [SerializeField] private float m_PopVerticalVelocity = 1f;

    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private CapsuleCollider2D m_Collider;
    [SerializeField] private Animator m_Animator;

    private int m_ProjectileLayer;

    private void Awake()
    {
        if (m_Rigidbody == null) m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Collider == null) m_Collider = GetComponent<CapsuleCollider2D>();
        if (m_Animator == null) m_Animator = GetComponent<Animator>();

        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_Rigidbody.linearVelocityX = m_HorizontalVelocity;

        // Assign fixed sorting order
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(transform.position.x * 100);

        m_ProjectileLayer = LayerMask.NameToLayer("Projectile");
    }


    // Vertical velocity required to reach the specific target height point
    private float BounceVelocity(float i_BottomPoint)
    {
        float topPoint = i_BottomPoint + m_Collider.size.y;
        if (topPoint > m_MaxBounce) return 0f;

        // Gravity might be scaled
        float gravity = Mathf.Abs(Physics2D.gravity.y * m_Rigidbody.gravityScale);

        // Y coordinates might be scaled as well
        float bounceHeight = m_MaxBounce * m_BounceMultiplier / transform.localScale.y;
        if (topPoint + bounceHeight > m_MaxBounce)
        {
            bounceHeight = m_MaxBounce - topPoint;
        }

        return Mathf.Sqrt(2 * gravity * bounceHeight);
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        OnCollision(i_Collision);
    }

    private void OnCollisionStay2D(Collision2D i_Collision)
    {
        OnCollision(i_Collision);
    }


    private void OnCollision(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.layer == m_ProjectileLayer) Explode();
        else FixVelocities(i_Collision);
    }

    private void Explode()
    {
        m_Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        m_Rigidbody.linearVelocity = Vector2.zero;

        // Trigger explosion animation
        m_Animator.SetTrigger(ExplodingTriggerHash);

        // Spawn children only if not the smallest ball type
        if (m_SmallerBall != null)
        {
            // Split the current ball in two smaller ones
            SpawnSmallerClone(m_HorizontalVelocity);
            SpawnSmallerClone(-m_HorizontalVelocity);
        }
    }

    private void OnExploded()
    {
        // Destroy the current ball on animation end
        Destroy(gameObject);
    }

    private void SpawnSmallerClone(float i_HorizontalVelocity)
    {
        GameObject ball = Instantiate<GameObject>(m_SmallerBall);
        ball.transform.position = transform.position;
        ball.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(
            i_HorizontalVelocity,
            m_PopVerticalVelocity
        );
    }

    private void FixVelocities(Collision2D i_Collision)
    {
        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            // Check for floor collision
            if (contact.normal.y > 0f) OnFloorCollision(contact.point.y);
            FixHorizontalVelocity(contact.normal.x);
        }
    }

    private void OnFloorCollision(float i_ContactPoint)
    {
        // Reset vertical velocity with static one
        m_Rigidbody.linearVelocityY = BounceVelocity(i_ContactPoint);
    }

    private void FixHorizontalVelocity(float i_NormalX)
    {
        float currentHorizontalVelocity = m_Rigidbody.linearVelocityX;
        if (Mathf.Abs(currentHorizontalVelocity) == m_HorizontalVelocity) return;

        float direction = Mathf.Sign(i_NormalX != 0f
            ? i_NormalX
            : currentHorizontalVelocity);

        m_Rigidbody.linearVelocityX = direction * m_HorizontalVelocity;
    }
}
