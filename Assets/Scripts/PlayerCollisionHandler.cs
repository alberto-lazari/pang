using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private static readonly int HitTriggerHash = Animator.StringToHash("Hit");
    private static readonly int LandedTriggerHash = Animator.StringToHash("Landed");

    [SerializeField] private Vector2 m_HitVelocity = new Vector2(1.5f, 4f);
    private bool m_IsAlive = true;

    private PlayerController m_Controller;
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private PhysicsMaterial2D m_Material;

    private int m_BubbleLayer;
    private int m_StageLayer;

    private void Awake()
    {
        m_Controller = GetComponent<PlayerController>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();

        // Store physics material for death animation
        m_Material = m_Rigidbody.sharedMaterial;
        m_Rigidbody.sharedMaterial = null;

        m_BubbleLayer = LayerMask.NameToLayer("Bubble");
        m_StageLayer = LayerMask.NameToLayer("Stage");
    }

    private void OnTriggerEnter2D(Collider2D i_Collider)
    {
        if (!m_IsAlive) return;
        if (i_Collider.gameObject.layer == m_BubbleLayer)
            OnBubbleHit(transform.position - i_Collider.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.layer != m_StageLayer) return;

        // Switch to dead sprite (if hit)
        m_Animator.SetTrigger(LandedTriggerHash);
    }

    private void OnBubbleHit(Vector2 i_HitDirection)
    {
        m_Animator.SetTrigger(HitTriggerHash);
        m_Animator.ResetTrigger(LandedTriggerHash);

        m_IsAlive = false;
        m_Controller.enabled = false;

        m_Rigidbody.linearVelocity = m_HitVelocity * new Vector2(
            // Push player along the hit direction
            i_HitDirection.x > 0 ? 1f : -1f,
            1f
        );
        m_Rigidbody.sharedMaterial = m_Material;
    }
}
