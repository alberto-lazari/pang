using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private static readonly int HitTriggerHash = Animator.StringToHash("Hit");
    private static readonly int LandedTriggerHash = Animator.StringToHash("Landed");

    [SerializeField] private PlayerController m_Controller;
    [SerializeField] private Animator m_Animator;

    private bool m_IsAlive = true;
    private int m_BubbleLayer;
    private int m_StageLayer;

    private void Awake()
    {
        if (m_Controller == null) m_Controller = GetComponent<PlayerController>();
        if (m_Animator == null) m_Animator = GetComponent<Animator>();

        m_BubbleLayer = LayerMask.NameToLayer("Bubble");
        m_StageLayer = LayerMask.NameToLayer("Stage");
    }

    private void OnTriggerEnter2D(Collider2D i_Collider)
    {
        if (!m_IsAlive || i_Collider.gameObject.layer != m_BubbleLayer) return;

        m_IsAlive = false;
        m_Controller.enabled = false;
        m_Animator.SetTrigger(HitTriggerHash);
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.layer != m_StageLayer) return;

        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            // Check for floor collision
            if (contact.normal.y > 0f) m_Animator.SetTrigger(LandedTriggerHash);
        }
    }
}
