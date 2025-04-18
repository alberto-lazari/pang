using UnityEngine;

public class ShootFlash : MonoBehaviour
{
    private static readonly int ShootTriggerHash = Animator.StringToHash("Shoot");

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void OnShoot()
    {
        // Trigger shoot flash animation
        m_Animator.SetTrigger(ShootTriggerHash);
    }
}
