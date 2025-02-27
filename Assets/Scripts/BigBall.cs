using UnityEngine;

public class BigBall : BouncingBall
{
    [SerializeField] private float m_BounceMultiplier = 1f;

    protected override float BounceMultiplier => m_BounceMultiplier;
}
