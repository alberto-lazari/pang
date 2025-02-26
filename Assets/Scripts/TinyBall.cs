using UnityEngine;

public class TinyBall : BouncingBall
{
    [SerializeField]
    private float m_BounceVelocity = 1.6f;

    protected override float BounceVelocity => m_BounceVelocity;
}
