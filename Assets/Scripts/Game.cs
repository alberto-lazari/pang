using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    // Access as lazy singleton
    public static Game State { get; private set; }

    [SerializeField] private int score = 0;
    [SerializeField] private float m_LootChance = 0.25f;
    [SerializeField] private List<Item> m_LootItems = new();

    private void Awake()
    {
        // Singleton implementation
        if (State != null && State != this)
        {
            Destroy(gameObject);
            return;
        }
        State = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int i_Points)
    {
        score += i_Points;
    }

    public void TryLootDrop(Vector3 i_InitialPosition)
    {
        if (m_LootItems.Count == 0 || Random.value > m_LootChance) return;

        int index = Random.Range(0, m_LootItems.Count);
        GameObject loot = m_LootItems[index].gameObject;
        Instantiate(loot, i_InitialPosition, loot.transform.rotation, transform);
    }
}
