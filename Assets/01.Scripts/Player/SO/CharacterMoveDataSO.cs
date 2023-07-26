using UnityEngine;
[CreateAssetMenu(menuName = "SO/CharacterMoveDataSO")]
public class CharacterMoveDataSO : ScriptableObject
{
    [Header("�̵� ����")]
    public float moveClamp = 13f;
    public float acceleration = 90f;
    public float deAcceleration = 60f;
    public float apexBonus = 2f;
    [Header("�߷� ����")]
    public float fallClamp = -40f;
    public float minFallSpeed = 80f;
    public float maxFallSpeed = 120f;
    [Header("���� ����")]
    public float jumpPower = 30f;
    public float jumpApexThreshold = 10f;
    public float jumpEndEarlyGravityModifier = 3f;
    public float coyoteTime = 0.05f;
    [Header("�� ����")]
    public float abc = 0f;
}
