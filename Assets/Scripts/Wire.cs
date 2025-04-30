using UnityEngine;

public class Wire : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    private EdgeCollider2D m_Collider;
    private Harpoon m_Harpoon;

    public void SetHarpoon(Harpoon i_Harpoon)
    {
        m_Harpoon = i_Harpoon;
    }

    public void Shoot()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<EdgeCollider2D>();
    }

    private void FixedUpdate()
    {
        Vector2 tail = m_Collider.points[0];
        Vector2 head = m_Collider.points[1];
        // Use height of the sprite as wire head
        float height = m_SpriteRenderer.bounds.size.y / transform.lossyScale.y;
        head.y = height - 0.015f;
        m_Collider.points = new Vector2[] { tail, head };
    }

    private void OnTriggerEnter2D(Collider2D i_Collider)
    {
        if (!i_Collider.gameObject.CompareTag("Ladder"))
            Destroy();
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
        m_Harpoon?.RewindWire(this);
    }
}
