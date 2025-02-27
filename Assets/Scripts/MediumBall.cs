using UnityEngine;

public class MediumBall : BouncingBall
{
    [SerializeField] private float m_BounceMultiplier = 0.75f;

    protected override float BounceMultiplier => m_BounceMultiplier;
}
