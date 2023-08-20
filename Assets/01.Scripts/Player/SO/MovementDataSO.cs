using DG.Tweening;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/Player/MovementDataSO")]
public class MovementDataSO : ScriptableObject
{
    public float moveClamp = 13f;
    public float acceleration = 90f;
    public float deAcceleration = 60f;
    public float apexBonus = 2f;
}
