using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUI : GameUI
{
    [SerializeField] private Button m_PlayButton;
    [SerializeField] private Button m_QuitButton;

    private void OnEnable()
    {
        if (m_PlayButton == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_PlayButton.gameObject);

#if UNITY_WEBGL
        HideQuitButton();
#endif
    }

    private void HideQuitButton()
    {
        if (m_QuitButton != null)
        {
            m_QuitButton.gameObject.SetActive(false);
        }

        RectTransform playTransform = m_PlayButton.GetComponent<RectTransform>();
        if (playTransform == null) return;

        playTransform.pivot = new Vector2(0.5f, 0.5f);
        playTransform.anchorMin = new Vector2(0, 0.5f);
        playTransform.anchorMax = new Vector2(1, 0.5f);
        playTransform.anchoredPosition = Vector2.zero;
    }
}
