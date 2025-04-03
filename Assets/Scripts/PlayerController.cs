using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_PlayerSpeed = 1.5f;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    private static readonly int HorizontalInputHash = Animator.StringToHash("HorizontalInput");
    private static readonly int IsShootingHash = Animator.StringToHash("IsShooting");

    private bool m_IsShooting = false;


    private void Start()
    {
        if (m_Rigidbody == null) m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Animator == null) m_Animator = GetComponent<Animator>();
        if (m_SpriteRenderer == null) m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (!m_IsShooting) m_IsShooting = Input.GetKeyDown(KeyCode.S);

        if (m_IsShooting) OnShoot();
        else Move(horizontalInput);
    }

    private void Move(float i_InputSpeed)
    {
        // Update player speed
        m_Rigidbody.linearVelocityX = i_InputSpeed * m_PlayerSpeed;

        // Update animator controller variable
        m_Animator.SetFloat(HorizontalInputHash, i_InputSpeed);

        // Make sprite face the right direction
        if (i_InputSpeed < -0.01f) m_SpriteRenderer.flipX = true;
        else if (i_InputSpeed > 0.01f) m_SpriteRenderer.flipX = false;
    }

    private void OnShoot()
    {
        // Stop moving animation
        m_Animator.SetFloat(HorizontalInputHash, 0f);
        m_Rigidbody.linearVelocityX = 0f;

        m_Animator.SetBool(IsShootingHash, m_IsShooting);
    }

    private void OnShootEnd()
    {
        m_IsShooting = false;
        m_Animator.SetBool(IsShootingHash, m_IsShooting);
    }
}
