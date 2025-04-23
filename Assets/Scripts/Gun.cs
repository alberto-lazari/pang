using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Beam m_Beam;

    protected override void Awake()
    {
        base.Awake();

        if (m_Beam == null) Debug.LogError("Gun beam not set");
    }

    public override void Shoot()
    {
        base.Shoot();

        if (m_Beam == null) return;
        Instantiate<Beam>(
            m_Beam,
            transform.position,
            m_Beam.transform.rotation,
            transform.parent
        ).Shoot();
    }
}
