using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] private float m_Speed = 2.5f;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private Animator m_Animator;

    private static readonly int HitHash = Animator.StringToHash("Hit");

    public void Shoot()
    {
        gameObject.SetActive(true);
        m_Rigidbody.linearVelocityY = m_Speed;
    }


    private void Start()
    {
        if (m_Rigidbody == null) m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Animator == null) m_Animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        float contactY = i_Collision.GetContact(0).point.y;
        // Place beam on the contact point
        transform.position = new Vector3(
            transform.position.x,
            contactY,
            transform.position.z
        );
        m_Rigidbody.linearVelocityY = 0f;
        m_Animator.SetBool(HitHash, true);
        Destroy(gameObject, .2f);
    }
}
