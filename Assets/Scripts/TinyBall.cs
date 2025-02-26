using UnityEngine;

public class TinyBall : BouncingBall
{
    [SerializeField]
    private float m_BounceVelocity = 1.35f;

    protected override float BounceVelocity => m_BounceVelocity;
}
