using UnityEngine;

public class Shield : Item
{
    private static readonly int IsActiveHash = Animator.StringToHash("IsActive");

    private Animator m_Animator;

    public override void OnGrab(GameObject i_Player)
    {
        i_Player.GetComponent<PlayerCollisionHandler>()
            .SetShield(this);
        transform.parent = i_Player.transform;
        transform.position = i_Player.transform.position;
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
