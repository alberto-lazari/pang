using UnityEngine;

public class SmallBall : BouncingBall
{
    [SerializeField]
    private float m_BounceVelocity = 2.2f;

    protected override float BounceVelocity => m_BounceVelocity;
}
