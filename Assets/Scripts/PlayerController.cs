using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_Speed = 1.5f;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    private static readonly int HorizontalInputHash = Animator.StringToHash("HorizontalInput");

    private void Start()
    {
        if (m_Rigidbody == null) m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Animator == null) m_Animator = GetComponent<Animator>();
        if (m_SpriteRenderer == null) m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Update player speed
        m_Rigidbody.linearVelocityX = horizontalInput * m_Speed;

        // Update animator controller variable
        m_Animator.SetFloat(HorizontalInputHash, horizontalInput);

        // Make sprite face the right direction
        if (horizontalInput < -0.01f) m_SpriteRenderer.flipX = true;
        else if (horizontalInput > 0.01f) m_SpriteRenderer.flipX = false;
    }
}
