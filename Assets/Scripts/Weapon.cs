using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject m_Player;
    [SerializeField] private ShootFlash m_ShootFlash;

    private float? m_PlayerHeight;

    protected virtual void Awake()
    {
        if (m_Player == null) m_Player = transform.parent.gameObject;
        if (m_ShootFlash == null) Debug.LogError("Weapon holder not assigned");

        m_PlayerHeight = m_Player.GetComponent<SpriteRenderer>()
            ?.bounds.size.y;
        if (m_PlayerHeight is not float playerHeight)
        {
            Debug.LogError("Weapon needs a player with a sprite renderer");
            return;
        }
    }

    public virtual void Shoot()
    {
        if (m_ShootFlash == null || m_PlayerHeight is not float playerHeight) return;

        // Place weapon and flash above player
        Vector3 weaponPosition = m_Player.transform.position
            + Vector3.up * playerHeight;
        transform.position = weaponPosition;
        m_ShootFlash.transform.position = weaponPosition;

        // Show shoot flash
        m_ShootFlash.OnShoot();
    }
}
