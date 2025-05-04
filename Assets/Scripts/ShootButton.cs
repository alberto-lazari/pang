using UnityEngine;
using UnityEngine.UI;

public class ShootButton : MonoBehaviour
{
    private Button m_Button;
    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        Weapon.OnWeaponPicked += UpdateButtonIcon;
    }

    private void UpdateButtonIcon(Weapon i_Weapon)
    {
        if (m_SpriteRenderer == null) return;

        m_Button.interactable = true;
        m_SpriteRenderer.sprite = i_Weapon.icon;
    }
}
