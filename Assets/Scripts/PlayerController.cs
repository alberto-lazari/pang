using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int HorizontalInputHash = Animator.StringToHash("HorizontalInput");
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int ShootingHash = Animator.StringToHash("Shooting");
    private static readonly int IsClimbingHash = Animator.StringToHash("IsClimbing");
    private static readonly int ClimbingHash = Animator.StringToHash("Climbing");

    [SerializeField] private float m_Speed = 1.5f;
    [SerializeField] private float m_LadderTriggerDistance = 0.1f;

    [SerializeField] private Weapon m_Weapon;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;
    private Actions m_InputActions;
    private int m_StageLayer;

    private float? m_LadderX = null;
    [SerializeField] private bool m_IsAtLadderTop = false;
    [SerializeField] private bool m_IsAtLadderBottom = false;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_StageLayer = LayerMask.NameToLayer("Stage");

        m_InputActions = new();
        m_InputActions.Player.Move.canceled += ctx => Halt();
        m_InputActions.Player.Attack.performed += ctx => OnShoot();
    }

    private void OnEnable()
    {
        m_InputActions.Enable();
    }

    private void OnDisable()
    {
        m_InputActions.Disable();

        // Stop moving when disabling the controller component
        Halt();
    }

    private void Update()
    {
        Vector2 inputMovement = m_InputActions.Player.Move.ReadValue<Vector2>();
        if (inputMovement != Vector2.zero) OnMove(inputMovement);
    }

    private void OnTriggerStay2D(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.tag != "Ladder") return;

        float ladderX = i_Collider.transform.position.x;
        m_LadderX = Mathf.Abs(ladderX - transform.position.x) < m_LadderTriggerDistance
            ? ladderX
            : null;
    }

    private void OnTriggerExit2D(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.tag != "Ladder") return;

        m_LadderX = null;
        m_Animator.SetBool(IsClimbingHash, false);
        m_Rigidbody.linearVelocityY = 0f;
        m_Rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.layer != m_StageLayer) return;

        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            if (contact.normal.y <= 0f) break;
            
            m_IsAtLadderTop = i_Collision.gameObject.tag == "Ladder";
            m_IsAtLadderBottom = !m_IsAtLadderTop;

            m_Animator.SetBool(IsClimbingHash, false);

            // Stop climbing
            m_Rigidbody.linearVelocityY = 0f;
        }
    }


    private void OnMove(Vector2 i_Movement)
    {
        // Do not move while shooting
        if (IsState(ShootingHash))
        {
            Halt();
            return;
        }

        Run(i_Movement.x);
        Climb(i_Movement.y);

    }

    private void OnShoot()
    {
        if (m_Weapon == null) return;

        // Wait shoot animation to end before shooting again
        if (IsState(ShootingHash)) return;

        Halt();
        m_Animator.SetTrigger(ShootHash);
        m_Weapon.Shoot();
    }

    private void Run(float i_InputSpeed)
    {
        // Can't move horizontally while climbing
        if (IsState(ClimbingHash)) return;

        // Update player speed
        m_Rigidbody.linearVelocityX = i_InputSpeed * m_Speed;

        // Update animator controller variable
        m_Animator.SetFloat(HorizontalInputHash, i_InputSpeed);

        // Make sprite face the right direction
        if (i_InputSpeed < 0f) m_SpriteRenderer.flipX = true;
        else if (i_InputSpeed > 0f) m_SpriteRenderer.flipX = false;
    }

    private void Climb(float i_InputSpeed)
    {
        // Climb only if on ladder
        if (m_LadderX is not float ladderX) return;

        bool bIsGoingUnder = m_IsAtLadderBottom && i_InputSpeed < 0f;
        bool bIsGoingOver = m_IsAtLadderTop && i_InputSpeed > 0f;
        if (bIsGoingUnder || bIsGoingOver) return;

        m_Rigidbody.linearVelocityY = i_InputSpeed;

        if (i_InputSpeed == 0f) return;

        if (!IsState(ClimbingHash))
        {
            m_Animator.SetBool(IsClimbingHash, true);
            transform.position = new Vector3(
                ladderX,
                transform.position.y,
                transform.position.z
            );
            m_Rigidbody.linearVelocityX = 0f;
            m_Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        // Since it's climbing it's no longer at ladder top/bottom
        m_IsAtLadderTop = false;
        m_IsAtLadderBottom = false;
    }

    private void Halt()
    {
        // Stop moving animation
        m_Animator.SetFloat(HorizontalInputHash, 0f);

        m_Rigidbody.linearVelocityX = 0f;
        if (m_LadderX is float) m_Rigidbody.linearVelocityY = 0f;
    }

    private bool IsState(int i_StateNameHash)
    {
        return m_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == i_StateNameHash;
    }
}
