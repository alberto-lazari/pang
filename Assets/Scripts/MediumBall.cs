using UnityEngine;

public class MediumBall : BouncingBall
{
    [SerializeField] private float m_TargetHeight = 1.4f;

    protected override float TargetHeight => m_TargetHeight;
}
