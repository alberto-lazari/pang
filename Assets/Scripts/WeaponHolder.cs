using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject m_Player;
    [SerializeField] private Animator m_Animator;

    private float? m_PlayerHeight;

    private static readonly int ShootTriggerHash = Animator.StringToHash("Shoot");

    private void Awake()
    {
        if (m_Animator == null) m_Animator = GetComponent<Animator>();

        if (m_Player == null)
        {
            Debug.LogError("Player not set");
            return;
        }

        m_PlayerHeight = m_Player.GetComponent<SpriteRenderer>()
            ?.bounds
            .size
            .y;
        if (m_PlayerHeight == null)
            Debug.LogError("Weapon needs a player with a sprite renderer");

        transform.position = m_Player.transform.position
            + Vector3.up * (float)m_PlayerHeight;
    }

    public void OnShoot()
    {
        m_Animator.SetTrigger(ShootTriggerHash);
    }
}
