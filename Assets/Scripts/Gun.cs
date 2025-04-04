using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Beam m_Beam;

    private float? m_PlayerHeight;


    private void Start()
    {
        if (m_Beam == null) Debug.LogError("Gun beam not set");

        m_PlayerHeight = GetComponent<SpriteRenderer>()
            ?.bounds
            .size
            .y;
        if (m_PlayerHeight == null)
        {
            Debug.LogError("Gun needs to be attached to a player with a sprite renderer");
        }
    }

    public override void Shoot()
    {
        if (m_Beam == null || m_PlayerHeight == null) return;
        GameObject beam = Instantiate<GameObject>(
            m_Beam.gameObject,
            transform.position + Vector3.up * (float)m_PlayerHeight,
            m_Beam.transform.rotation
        );
        beam.GetComponent<Beam>().Shoot();
    }
}
