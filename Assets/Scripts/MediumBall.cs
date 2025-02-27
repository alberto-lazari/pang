using UnityEngine;

public class MediumBall : BouncingBall
{
    [SerializeField]
    private float m_BounceVelocity = 2.5f;

    protected override float BounceVelocity => m_BounceVelocity;
}
