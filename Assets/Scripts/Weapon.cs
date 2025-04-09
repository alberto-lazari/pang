using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponHolder m_WeaponHolder;

    protected virtual void Awake()
    {
        if (m_WeaponHolder == null) m_WeaponHolder = transform.parent
            .GetComponent<WeaponHolder>();
    }

    public virtual void Shoot()
    {
        m_WeaponHolder.OnShoot();
    }
}
