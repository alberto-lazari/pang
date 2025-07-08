using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI m_StageText;
    [SerializeField] private TextMeshProUGUI m_ScoreText;

    private void Awake()
    {
        Instance = this;

        if (m_StageText == null) Debug.LogError("Stage text is not assigned");
        if (m_ScoreText == null) Debug.LogError("Score text is not assigned");
    }

    public void SetStageText(string i_Message) => m_StageText.text = i_Message;
    public void UpdateStage(int i_Stage) => m_StageText.text = $"Stage {i_Stage}";
    public void UpdateScore(int i_Score) => m_ScoreText.text = $"Score {i_Score}";
}
