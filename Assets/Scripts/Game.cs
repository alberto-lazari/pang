using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    // Access as singleton
    public static Game State { get; private set; }

    [SerializeField] private int m_StageNumber;
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
        m_StageNumber = SceneManager.GetActiveScene().buildIndex + 1;

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
        GameUI.Instance.UpdateStage(m_StageNumber);
        GameUI.Instance.UpdateScore(m_GameScore.points);
    }

    public void AddScore(int i_Points)
    {
        m_GameScore.points += i_Points;
        GameUI.Instance.UpdateScore(m_GameScore.points);
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


    private void NextStage() => StartCoroutine(LoadNextStage());
    private IEnumerator<WaitForSeconds> LoadNextStage()
    {
        yield return new WaitForSeconds(1.5f);

        // Load the next scene by index or name
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if the next scene exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else Debug.Log("You won!");
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
