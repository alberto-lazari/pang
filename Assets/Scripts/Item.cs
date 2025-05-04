using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private float m_FallingSpeed = 1f;
    private int m_StageLayer;

    protected Rigidbody2D m_Rigidbody;

    public abstract void OnPick(GameObject i_Player);

    protected virtual void Awake()
    {
        m_StageLayer = LayerMask.NameToLayer("Stage");
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.linearVelocityY = -m_FallingSpeed;
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        GameObject obj = i_Collision.gameObject;
        if (obj.CompareTag("Player") && !transform.parent.gameObject.CompareTag("Player"))
            OnPick(obj);
    }

    private void OnCollisionExit2D(Collision2D i_Collision)
    {
        // Make item fall if it has no ground
        if (i_Collision.gameObject.layer == m_StageLayer)
            m_Rigidbody.linearVelocityY = -m_FallingSpeed;
    }
}
