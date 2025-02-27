using UnityEngine;

public class TinyBall : BouncingBall
{
    [SerializeField] private float m_TargetHeight = 0.55f;

    protected override float TargetHeight => m_TargetHeight;
}
