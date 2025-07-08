using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    // Access as singleton
    public static Game State { get; private set; }

    [SerializeField] private int m_StageNumber;
    [SerializeField] private int m_FirstStageIndex = 1;
    [SerializeField] private string m_MainMenuSceneName = "MainMenu";
    [SerializeField] private HashSet<BouncingBubble> m_ActiveBubbles;

    [SerializeField] private float m_LootChance = 0.5f;
    [SerializeField] private List<Item> m_LootItems = new();

    private GameScore m_GameScore;

    private void Awake()
    {
        // Singleton implementation
        if (State != null && State != this)
        {
            Destroy(gameObject);
            return;
        }
        State = this;

        // Get current stage number
        m_StageNumber = SceneManager.GetActiveScene().buildIndex - m_FirstStageIndex + 1;

        // Initialize bubbles cache
        m_ActiveBubbles = new HashSet<BouncingBubble>(
            FindObjectsByType<BouncingBubble>(FindObjectsSortMode.None)
        );

        // Keep reference to game score instance
        m_GameScore = GameScore.Instance;
        if (m_GameScore == null)
        {
            // Create the game score root object
            GameObject gameScoreObject = new GameObject("GameScore");
            m_GameScore = gameScoreObject.AddComponent<GameScore>();
        }
    }

    private void Start()
    {
        // Don't set stage number for non-stage scenes (like main menu)
        if (SceneManager.GetActiveScene().name.StartsWith("Stage"))
        {
            GameUI.Instance.UpdateStage(m_StageNumber);
        }
        GameUI.Instance.UpdateScore(m_GameScore.points);
    }


    public void StartNewGame()
    {
        // Reset previous game score
        UpdateScore(0);
        SceneManager.LoadScene(m_FirstStageIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void AddScore(int i_Points)
    {
        m_GameScore.points += i_Points;
        UpdateScore(m_GameScore.points);
    }

    public void GameOver(string i_Message)
    {
        GameUI.Instance.SetStageText(i_Message);
        int index = SceneUtility.GetBuildIndexByScenePath(
                $"Assets/Scenes/{m_MainMenuSceneName}.unity");
        StartCoroutine(LoadStage(index));
    }

    public void RegisterBubble(BouncingBubble i_Bubble) => m_ActiveBubbles.Add(i_Bubble);

    public void DeregisterBubble(BouncingBubble i_Bubble)
    {
        m_ActiveBubbles.Remove(i_Bubble);

        // Check if stage is beaten
        if (m_ActiveBubbles.Count == 0) NextStage();
    }

    public void TryLootDrop(Vector3 i_InitialPosition)
    {
        if (m_LootItems.Count == 0 || Random.value > m_LootChance) return;

        int index = Random.Range(0, m_LootItems.Count);
        GameObject loot = m_LootItems[index].gameObject;
        Instantiate(loot, i_InitialPosition, loot.transform.rotation, transform);
    }


    private void UpdateScore(int i_Points)
    {
        m_GameScore.points = i_Points;
        GameUI.Instance.UpdateScore(i_Points);
    }

    private void NextStage()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if the next scene exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            GameUI.Instance.SetStageText($"Stage {m_StageNumber} cleared!");
            StartCoroutine(LoadStage(nextSceneIndex));
        }
        // The current scene was the last one
        else GameOver("You won!");
    }
    private IEnumerator<WaitForSeconds> LoadStage(int i_Index)
    {
        // Let the player take a breath before the next stage
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(i_Index);
    }


    private class GameScore : MonoBehaviour
    {
        public static GameScore Instance { get; private set; }

        public int points = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
