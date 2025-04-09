using UnityEngine;

public class ShootFlash : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private static readonly int ShootTriggerHash = Animator.StringToHash("Shoot");

    private void Awake()
    {
        if (m_Animator == null) m_Animator = GetComponent<Animator>();
    }

    public void OnShoot()
    {
        // Trigger shoot flash animation
        m_Animator.SetTrigger(ShootTriggerHash);
    }
}
