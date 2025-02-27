using UnityEngine;

public class SmallBall : BouncingBall
{
    [SerializeField] private float m_TargetHeight = 0.95f;

    protected override float TargetHeight => m_TargetHeight;
}
