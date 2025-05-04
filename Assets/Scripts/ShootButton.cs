using UnityEngine;

public class ShootButton : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        Weapon.OnWeaponPicked += UpdateButtonIcon;
    }

    private void UpdateButtonIcon(Weapon i_Weapon)
    {
        m_SpriteRenderer.sprite = i_Weapon.icon;
    }
}
