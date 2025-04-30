using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private ShootFlash m_ShootFlash;

    public abstract bool CanShoot();
    protected abstract void OnShoot();

    public virtual void OnGrab(GameObject i_Player)
    {
        if (transform.parent != i_Player.transform)
        {
            // Attach as weapon to the player
            i_Player.GetComponent<PlayerController>()
                .SetWeapon(this);
            transform.parent = i_Player.transform;
        }
        // Place weapon above player
        float playerHeight = i_Player.GetComponent<SpriteRenderer>()
            .bounds.size.y;
        transform.position = i_Player.transform.position + Vector3.up * playerHeight;
    }

    public virtual void Destroy() {
        Destroy(gameObject);
    }

    public void Shoot()
    {
        if (!CanShoot()) return;

        OnShoot();
        if (m_ShootFlash != null)
        {
            // Show shoot flash
            Instantiate(
                m_ShootFlash,
                transform.position,
                m_ShootFlash.transform.rotation,
                transform.parent.parent
            ).OnShoot();
        }
    }

    protected virtual void Awake()
    {
        if (m_ShootFlash == null) Debug.LogError("Shoot flash is not assigned");
    }

    private void Start()
    {
        // Check if weapon is already attached to player
        Transform parent = transform.parent;
        if (parent.CompareTag("Player"))
        {
            GameObject player = parent.gameObject;
            WeaponItem item = GetComponent<WeaponItem>();
            // Call item's OnGrab if the weapon has the component
            if (item != null) item.OnGrab(player);
            else OnGrab(player);
        }
    }
}
