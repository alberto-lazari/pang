using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private ShootFlash m_ShootFlash;

    protected abstract void OnShoot();

    public void Shoot()
    {
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
}
