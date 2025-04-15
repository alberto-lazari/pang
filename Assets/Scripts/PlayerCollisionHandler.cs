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
        // Perform on bubble hit
        if (!m_IsAlive || i_Collider.gameObject.layer != m_BubbleLayer) return;

        m_IsAlive = false;
        m_Controller.enabled = false;

        m_Animator.SetTrigger(HitTriggerHash);
        m_Animator.ResetTrigger(LandedTriggerHash);

        bool bLeftHit = i_Collider.transform.position.x - transform.position.x < 0f;
        m_Rigidbody.linearVelocity = m_HitVelocity
            // Push player in the right direction
            * new Vector2(bLeftHit ? 1f : -1f, 1f);
        m_Rigidbody.sharedMaterial = m_Material;
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.layer != m_StageLayer) return;

        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            // Switch to dead sprite (if hit)
            m_Animator.SetTrigger(LandedTriggerHash);
        }
    }
}
