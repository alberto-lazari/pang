using UnityEngine;

public class Shield : Item
{
    private static readonly int IsActiveHash = Animator.StringToHash("IsActive");

    private Animator m_Animator;

    protected override void OnGrab()
    {
        m_Player.SetShield(this);
        transform.parent = m_Player.transform;
        transform.position = m_Player.transform.position;
        m_Rigidbody.simulated = false;
        m_Animator.SetBool(IsActiveHash, true);
    }

    protected override void Awake()
    {
        base.Awake();
        m_Animator = GetComponent<Animator>();
    }

    public void OnHit()
    {
        Destroy(gameObject);
    }
}
