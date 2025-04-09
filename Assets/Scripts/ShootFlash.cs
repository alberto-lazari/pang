using UnityEngine;

public class ShootFlash : MonoBehaviour
{
    private static readonly int ShootTriggerHash = Animator.StringToHash("Shoot");

    [SerializeField] private Animator m_Animator;

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
