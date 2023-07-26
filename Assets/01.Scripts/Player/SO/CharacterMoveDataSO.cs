using UnityEngine;
[CreateAssetMenu(menuName = "SO/CharacterMoveDataSO")]
public class CharacterMoveDataSO : ScriptableObject
{
    [Header("이동 관련")]
    public float moveClamp = 13f;
    public float acceleration = 90f;
    public float deAcceleration = 60f;
    public float apexBonus = 2f;
    [Header("점프 관련")]
    [Header("벽 관련")]
    public float abc = 0f;
}
