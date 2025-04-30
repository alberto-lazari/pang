using UnityEngine;

public class Platform : MonoBehaviour
{
    private static readonly int BreakTriggerHash = Animator.StringToHash("Break");

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.CompareTag("HarpoonWire"))
            m_Animator.SetTrigger(BreakTriggerHash);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
