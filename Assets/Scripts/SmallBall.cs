using UnityEngine;

public class SmallBall : BouncingBall
{
    [SerializeField] private float m_TargetHeight = 0.92f;

    protected override float TargetHeight => m_TargetHeight;
}
