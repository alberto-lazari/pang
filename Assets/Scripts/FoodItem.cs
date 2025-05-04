using UnityEngine;

public class FoodItem : Item
{
    [SerializeField] private int m_Points;

    public override void OnPick(GameObject i_Player)
    {
        Game.State.AddScore(m_Points);
        Destroy(gameObject);
    }
}
