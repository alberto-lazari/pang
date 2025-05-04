using UnityEngine;
using UnityEngine.UI;

public class ClimbButton : MonoBehaviour
{
    private Button m_Button;
    private Image m_Image;
    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Image = m_Button.GetComponent<Image>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerController.OnNearLadder += SetButtonActive;
    }

    private void SetButtonActive(bool i_IsActive)
    {
        if (m_SpriteRenderer == null) return;

        m_Button.interactable = i_IsActive;
        m_Image.raycastTarget = i_IsActive;
        m_SpriteRenderer.enabled = i_IsActive;
    }
}
