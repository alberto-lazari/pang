using UnityEngine;

public class BigBall : BouncingBall
{
    [SerializeField]
    private float m_BounceVelocity = 2.9f;

    protected override float BounceVelocity => m_BounceVelocity;
}
