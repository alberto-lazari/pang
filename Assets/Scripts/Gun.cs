using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private Beam m_Beam;

    protected override void OnShoot()
    {
        if (m_Beam == null) return;

        // The gun is a child of player
        Transform gameArea = transform.parent.parent;

        // Create a new beam in the game area
        Instantiate(
            m_Beam,
            transform.position,
            m_Beam.transform.rotation,
            gameArea
        ).Shoot();
    }

    protected override void Awake()
    {
        base.Awake();
        if (m_Beam == null) Debug.LogError("Gun beam not set");
    }
}
