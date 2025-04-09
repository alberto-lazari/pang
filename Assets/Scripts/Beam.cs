using UnityEngine;

public class Beam : MonoBehaviour
{
    private static readonly int HitHash = Animator.StringToHash("Hit");

    [SerializeField] private float m_Speed = 2.5f;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private Animator m_Animator;

    private int m_StageLayer;

    public void Shoot()
    {
        gameObject.SetActive(true);
        m_Rigidbody.linearVelocityY = m_Speed;
    }


    private void Start()
    {
        if (m_Rigidbody == null) m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Animator == null) m_Animator = GetComponent<Animator>();
        m_StageLayer = LayerMask.NameToLayer("Stage");
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
    }

    private void OnTriggerEnter2D(Collider2D i_Collider)
    {
        gameObject.SetActive(false);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
