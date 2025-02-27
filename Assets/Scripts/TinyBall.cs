using UnityEngine;

public class TinyBall : BouncingBall
{
    [SerializeField] private float m_BounceMultiplier = 0.27f;

    protected override float BounceMultiplier => m_BounceMultiplier;
}
