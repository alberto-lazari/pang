using UnityEngine;

public class WeaponItem : Item
{
    private Weapon m_Weapon;
    private SpriteRenderer m_SpriteRenderer;

    public override void OnPick(GameObject i_Player)
    {
        // Disable the weapon item
        m_Rigidbody.simulated = false;
        m_SpriteRenderer.sprite = null;

        m_Weapon.OnPick(i_Player);
    }

    protected override void Awake()
    {
        base.Awake();
        m_Weapon = GetComponent<Weapon>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
