using UnityEngine;

public class SmallBall : BouncingBall
{
    [SerializeField]
    private float m_BounceVelocity = 1.8f;

    protected override float BounceVelocity => m_BounceVelocity;
}
