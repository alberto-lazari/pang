using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MoveState
    {
        Idle   = 0,
        Moving = 1,
    }

    public enum Direction
    {
        Left  = 0,
        Right = 1,
    }

    [SerializeField] private float m_Speed = 1f;
    [SerializeField] private float m_CollisionDistance = 0.12f;
    [SerializeField] private Object m_Stage;
    [SerializeField] private LayerMask m_WallLayer;
    [SerializeField] private Animator m_Animator;

    private static readonly int MoveStateHash = Animator.StringToHash("MoveState");
    private static readonly int FacingDirectionHash = Animator.StringToHash("FacingDirection");

    private MoveState m_MoveState = MoveState.Idle;
    private Direction m_FacingDirection = Direction.Right;


    public Vector2 GetDirectionVector()
    {
        return m_FacingDirection == Direction.Left ? Vector2.left : Vector2.right;
    }

    private void Start()
    {
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        HandleInput();
        if (CanMove())
        {
            Move();
        }
    }

    private bool Facing(Direction i_Direction)
    {
        return m_FacingDirection == i_Direction;
    }

    private bool IsBlocked(Direction i_Direction)
    {
        // Do not cast rays from player's feet, otherwise they will catch the floor
        Vector2 rayOrigin = (Vector2)transform.position + Vector2.up * 0.1f;
        return Physics2D.Raycast(rayOrigin, GetDirectionVector(), m_CollisionDistance, m_WallLayer);
    }

    private void HandleInput()
    {
        bool bLeft = Input.GetKey(KeyCode.LeftArrow);
        bool bRight = Input.GetKey(KeyCode.RightArrow);
        bool bLeftDown = Input.GetKeyDown(KeyCode.LeftArrow);
        bool bRightDown = Input.GetKeyDown(KeyCode.RightArrow);

        switch (m_MoveState)
        {
        case MoveState.Idle:
            if (bLeft || bRight)
            {
                m_MoveState = MoveState.Moving;
                m_FacingDirection = bRight ? Direction.Right : Direction.Left;

                m_Animator.SetInteger(MoveStateHash, (int)m_MoveState);
                m_Animator.SetInteger(FacingDirectionHash, (int)m_FacingDirection);
            }
            break;
        case MoveState.Moving:
            // Change direction
            if (bLeftDown && Facing(Direction.Right) || bRightDown && Facing(Direction.Left))
            {
                m_FacingDirection = bRightDown ? Direction.Right : Direction.Left;
                m_Animator.SetInteger(FacingDirectionHash, (int)m_FacingDirection);
            }
            // Stop
            if (!bLeft && Facing(Direction.Left) || !bRight && Facing(Direction.Right))
            {
                m_MoveState = MoveState.Idle;
                m_Animator.SetInteger(MoveStateHash, (int)m_MoveState);
            }
            break;
        }
    }

    private bool CanMove()
    {
        bool bMoving = m_MoveState == MoveState.Moving;
        bool bNotBlocked = !IsBlocked(m_FacingDirection);
        return bMoving && bNotBlocked;
    }

    private void Move()
    {
        transform.position += (Vector3)GetDirectionVector() * m_Speed * Time.deltaTime;
    }
}
