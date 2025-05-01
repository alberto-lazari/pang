using UnityEngine;

public class Platform : MonoBehaviour
{
    private static readonly int BreakTriggerHash = Animator.StringToHash("Break");

    [SerializeField] private int m_BreakPoints = 50;
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.CompareTag("HarpoonWire"))
        {
            m_Animator.SetTrigger(BreakTriggerHash);
            Game.State.AddScore(m_BreakPoints);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
