using UnityEngine;

public class BigBall : BouncingBall
{
    [SerializeField] private float m_TargetHeight = 1.9f;

    protected override float TargetHeight => m_TargetHeight;
}
