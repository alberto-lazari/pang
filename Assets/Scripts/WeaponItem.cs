using UnityEngine;

public class WeaponItem : Item
{
    private Weapon m_Weapon;
    private SpriteRenderer m_SpriteRenderer;

    protected override void OnGrab(GameObject i_Player)
    {
        // Attach as weapon to the player
        i_Player.GetComponent<PlayerController>()
            .SetWeapon(m_Weapon);
        transform.parent = i_Player.transform;

        // Disable the weapon item
        m_Rigidbody.simulated = false;
        m_SpriteRenderer.sprite = null;

        // Place weapon above player
        float playerHeight = i_Player.GetComponent<SpriteRenderer>()
            .bounds.size.y;
        transform.position = i_Player.transform.position + Vector3.up * playerHeight;
    }

    protected override void Awake()
    {
        base.Awake();
        m_Weapon = GetComponent<Weapon>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
