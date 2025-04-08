using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] private float m_Speed = 2.5f;
    [SerializeField] private float m_HitAnimationDuration = 0.2f;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private Animator m_Animator;

    private int m_StageLayer;

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
        m_StageLayer = LayerMask.NameToLayer("Stage");
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        int layer = i_Collision.gameObject.layer;
        if (layer == m_StageLayer) OnStageHit(i_Collision.GetContact(0).point.y);
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnStageHit(float i_ContactY)
    {
        // Place beam on the contact point
        transform.position = new Vector3(
            transform.position.x,
            i_ContactY,
            transform.position.z
        );
        m_Rigidbody.linearVelocityY = 0f;
        m_Animator.SetBool(HitHash, true);
        Destroy(gameObject, m_HitAnimationDuration);
    }
}
