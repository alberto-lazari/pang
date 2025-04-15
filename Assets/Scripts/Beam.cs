using UnityEngine;

public class Beam : MonoBehaviour
{
    private static readonly int HitHash = Animator.StringToHash("Hit");

    [SerializeField] private float m_Speed = 2.5f;

    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    private int m_BubbleLayer;


    public void Shoot()
    {
        gameObject.SetActive(true);
        m_Rigidbody.linearVelocityY = m_Speed;
    }


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        m_BubbleLayer = LayerMask.NameToLayer("Bubble");
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
        if (i_Collider.gameObject.layer != m_BubbleLayer) return;

        gameObject.SetActive(false);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
