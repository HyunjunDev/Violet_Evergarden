using DG.Tweening;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/CharacterMoveDataSO")]
public class CharacterMoveDataSO : ScriptableObject
{
    [Header("이동 관련")]
    public float moveClamp = 13f;
    public float acceleration = 90f;
    public float deAcceleration = 60f;
    public float apexBonus = 2f;
    [Header("중력 관련")]
    public float fallClamp = -40f;
    public float minFallSpeed = 80f;
    public float maxFallSpeed = 120f;
    [Header("점프 관련")]
    public float jumpPower = 30f;
    public float jumpApexThreshold = 10f;
    public float jumpEndEarlyGravityModifier = 3f;
    public float coyoteTime = 0.05f;
    [Header("대시 관련")]
    public float dashPower = 10f;
    public float dashTime = 0.3f;
    public Ease dashEase = Ease.Linear;
    [Header("벽 관련")]
    public float abc = 0f;
}
