using UnityEngine;

public class SmallBall : BouncingBall
{
    [SerializeField] private float m_BounceMultiplier = 0.5f;

    protected override float BounceMultiplier => m_BounceMultiplier;
}
