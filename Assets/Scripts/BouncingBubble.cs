using UnityEngine;

public class BouncingBubble : MonoBehaviour
{
    private static readonly int ExplodingTriggerHash = Animator.StringToHash("Exploding");

    [SerializeField] private BouncingBubble m_SmallerBubble;
    [SerializeField] private int m_ExplodePoints;

    [SerializeField] private float m_BounceMultiplier;
    [SerializeField] private float m_MaxBounce = 2f;

    [SerializeField] private float m_HorizontalVelocity = 0.6f;
    [SerializeField] private float m_PopVerticalVelocity = 1f;

    [SerializeField] private CapsuleCollider2D m_Collider;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private SpriteRenderer m_Renderer;

    private int m_ProjectileLayer;

    private void Awake()
    {
        if (m_Collider == null) m_Collider = GetComponent<CapsuleCollider2D>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();

        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_Rigidbody.linearVelocityX = m_HorizontalVelocity;

        // Assign fixed sorting order
        m_Renderer.sortingOrder = Mathf.FloorToInt(transform.position.x * 100);

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
        FixVelocities(i_Collision);
    }

    private void OnCollisionStay2D(Collision2D i_Collision)
    {
        FixVelocities(i_Collision);
    }

    private void OnTriggerEnter2D(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.layer == m_ProjectileLayer) Explode();
    }

    private void Explode()
    {
        m_Rigidbody.simulated = false;

        // Move to front to show the explosion
        m_Renderer.sortingOrder = int.MinValue;

        // Trigger explosion animation
        m_Animator.SetTrigger(ExplodingTriggerHash);

        bool bIsTiny = m_SmallerBubble == null;
        Game state = Game.State;

        // Spawn children only if not the smallest bubble type
        if (!bIsTiny)
        {
            // Split the current bubble in two smaller ones
            state.RegisterBubble(SpawnSmallerClone(m_HorizontalVelocity));
            state.RegisterBubble(SpawnSmallerClone(-m_HorizontalVelocity));

            // Try spawning loot
            state.TryLootDrop(transform.position);
        }

        // Update global state
        state.DeregisterBubble(this);
        state.AddScore(m_ExplodePoints);
    }

    private void OnExploded()
    {
        // Destroy the current bubble on animation end
        Destroy(gameObject);
    }

    private BouncingBubble SpawnSmallerClone(float i_HorizontalVelocity)
    {
        GameObject bubble = Instantiate(m_SmallerBubble.gameObject, transform.parent);
        bubble.transform.position = transform.position;
        bubble.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(
            i_HorizontalVelocity,
            m_PopVerticalVelocity
        );
        return bubble.GetComponent<BouncingBubble>();
    }

    private void FixVelocities(Collision2D i_Collision)
    {
        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            // Check for floor collision
            if (contact.normal.y > 0f && contact.relativeVelocity.y >= 0f)
                OnFloorCollision(contact.point.y);
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
            : currentHorizontalVelocity
        );
        m_Rigidbody.linearVelocityX = direction * m_HorizontalVelocity;
    }
}
