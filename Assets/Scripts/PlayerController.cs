using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int HorizontalInputHash = Animator.StringToHash("HorizontalInput");
    private static readonly int VerticalInputHash = Animator.StringToHash("VerticalInput");
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int ShootingHash = Animator.StringToHash("Shooting");
    private static readonly int IsClimbingHash = Animator.StringToHash("IsClimbing");
    private static readonly int ClimbingHash = Animator.StringToHash("Climbing");
    private static readonly int TopOutHash = Animator.StringToHash("TopOut");

    [SerializeField] private float m_RunningSpeed = 1.5f;
    [SerializeField] private float m_ClimbingSpeed = 0.9f;
    [SerializeField] private float m_LadderTriggerDistance = 0.05f;

    [SerializeField] private Weapon m_Weapon;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;
    private Actions m_InputActions;
    private int m_StageLayer;

    public enum LadderStatus
    {
        Away,
        Bottom,
        Middle,
        Top,
    }
    private float? m_LadderX = null;
    private float? m_LadderTopY = null;
    private LadderStatus m_LadderStatus = LadderStatus.Away;

    private float m_TouchingWallDirection = 0f;


    public void SetWeapon(Weapon i_Weapon)
    {
        if (m_Weapon != null && m_Weapon != i_Weapon) m_Weapon.Destroy();
        m_Weapon = i_Weapon;
    }

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
        OnMove(inputMovement);
    }

    private void OnTriggerEnter2D(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.tag != "Ladder") return;

        if (m_LadderStatus != LadderStatus.Top) m_LadderStatus = LadderStatus.Bottom;

        m_LadderTopY = i_Collider.transform.position.y
            + i_Collider.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void OnTriggerStay2D(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.tag != "Ladder") return;

        if (m_LadderX == null)
        {
            float ladderX = i_Collider.transform.position.x;
            m_LadderX = Mathf.Abs(ladderX - transform.position.x) < m_LadderTriggerDistance
                ? ladderX
                : null;
        }
    }

    private void OnTriggerExit2D(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.tag != "Ladder") return;

        m_LadderX = null;
        m_LadderTopY = null;
        m_LadderStatus = LadderStatus.Away;

        m_Animator.SetBool(IsClimbingHash, false);
        m_Rigidbody.linearVelocityY = 0f;
        m_Rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.layer != m_StageLayer) return;

        float? contactY = null;
        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            // Handle floor collision
            if (contact.normal.y > 0f)
            {
                if (contactY == null) contactY = contact.point.y;
                if (i_Collision.gameObject.CompareTag("Ladder")) m_LadderStatus = LadderStatus.Top;

                // Stop climbing
                m_Rigidbody.linearVelocityY = 0f;
                m_Animator.SetBool(IsClimbingHash, false);
            }
        }

        // If not at the top of the ladder it has to be at the bottom
        if (m_LadderX != null && m_LadderStatus != LadderStatus.Top)
            m_LadderStatus = LadderStatus.Bottom;

        if (contactY is float y)
        {
            m_Rigidbody.bodyType = RigidbodyType2D.Dynamic;

            // Place on the contact point
            transform.position = new Vector3(
                transform.position.x,
                // Penetrate slightly to let collisions adjust the position
                y - 0.01f,
                transform.position.z
            );
        }
    }

    private void OnCollisionStay2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.layer != m_StageLayer) return;

        m_TouchingWallDirection = 0f;
        foreach (ContactPoint2D contact in i_Collision.contacts)
        {
            float normal = contact.normal.x;
            // Register collision with wall
            //  1 -> right wall
            // -1 -> left wall
            //  0 -> not colliding with any wall
            if (Mathf.Abs(normal) > 0.5f) m_TouchingWallDirection = -Mathf.Sign(normal);
        }
    }


    private void OnShoot()
    {
        if (m_Weapon == null || !m_Weapon.CanShoot()) return;

        // Wait shoot animation to end before shooting again
        if (IsState(ShootingHash)) return;

        Halt();
        m_Animator.SetTrigger(ShootHash);
        m_Weapon.Shoot();
    }

    private void OnMove(Vector2 i_Movement)
    {
        // Stop while topping out of a ladder
        if (IsState(TopOutHash))
        {
            m_Rigidbody.linearVelocityY = 0f;
            return;
        }

        // Do not move while shooting
        if (IsState(ShootingHash))
        {
            Halt();
            return;
        }

        Run(i_Movement.x);
        Climb(i_Movement.y);

    }

    private void Run(float i_InputSpeed)
    {
        // Can't move horizontally while climbing
        if (IsState(ClimbingHash)) return;

        // Avoid bumping into walls
        if (i_InputSpeed * m_TouchingWallDirection > 0f)
        {
            m_Animator.SetFloat(HorizontalInputHash, 0f);
            return;
        }

        // Make sprite face the right direction
        if (i_InputSpeed < 0f) m_SpriteRenderer.flipX = true;
        else if (i_InputSpeed > 0f) m_SpriteRenderer.flipX = false;

        // Update player speed
        m_Rigidbody.linearVelocityX = i_InputSpeed * m_RunningSpeed;

        // Update animator controller variable
        m_Animator.SetFloat(HorizontalInputHash, i_InputSpeed);
    }

    private void Climb(float i_InputSpeed)
    {
        // Climb only if on ladder
        if (m_LadderX is not float ladderX) return;

        bool bIsGoingUnder = m_LadderStatus == LadderStatus.Bottom && i_InputSpeed < 0f;
        bool bIsGoingOver = m_LadderStatus == LadderStatus.Top && i_InputSpeed > 0f;
        if (bIsGoingUnder || bIsGoingOver) return;

        if (IsState(ClimbingHash))
        {
            m_Rigidbody.linearVelocityY = i_InputSpeed * m_ClimbingSpeed;
            m_Animator.SetFloat(VerticalInputHash, i_InputSpeed);
        }

        if (i_InputSpeed == 0f) return;

        if (!IsState(ClimbingHash))
        {
            // Start climbing
            m_Animator.SetBool(IsClimbingHash, true);
            m_Animator.SetFloat(HorizontalInputHash, 0f);
            transform.position = new Vector3(
                ladderX,
                transform.position.y,
                transform.position.z
            );
            m_Rigidbody.linearVelocityX = 0f;
            m_Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        // Since it's climbing it's no longer at ladder top/bottom
        m_LadderStatus = LadderStatus.Middle;

        // Manage top out
        if (m_LadderTopY is not float ladderTopY) return;
        float playerHeight = m_SpriteRenderer.bounds.size.y;
        float topOutY = ladderTopY - playerHeight / 2;
        if (transform.position.y > topOutY)
        {
            // Stop climbing
            if (i_InputSpeed > 0f)
            {
                m_Animator.SetBool(IsClimbingHash, false);
                m_Rigidbody.linearVelocityY = 0f;
                m_Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
            // Top out
            m_Animator.SetTrigger(TopOutHash);
            transform.position = new Vector3(
                ladderX,
                // Perform top out when a specific Y is reached
                i_InputSpeed > 0f
                    ? ladderTopY + 0.01f
                    : topOutY - 0.01f,
                transform.position.z
            );
        }
    }

    private void Halt()
    {
        // Stop moving animations
        m_Animator.SetFloat(HorizontalInputHash, 0f);
        if (!IsState(TopOutHash)) m_Animator.SetFloat(VerticalInputHash, 0f);

        m_Rigidbody.linearVelocityX = 0f;
        if (m_LadderX != null) m_Rigidbody.linearVelocityY = 0f;
    }

    private bool IsState(int i_StateNameHash)
    {
        return m_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == i_StateNameHash;
    }
}
