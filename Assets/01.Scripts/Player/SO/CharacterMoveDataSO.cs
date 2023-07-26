using UnityEngine;
[CreateAssetMenu(menuName = "SO/CharacterMoveDataSO")]
public class CharacterMoveDataSO : ScriptableObject
{
    [Header("�̵� ����")]
    public float moveClamp = 13f;
    public float acceleration = 90f;
    public float deAcceleration = 60f;
    public float apexBonus = 2f;
    [Header("���� ����")]
    [Header("�� ����")]
    public float abc = 0f;
}
