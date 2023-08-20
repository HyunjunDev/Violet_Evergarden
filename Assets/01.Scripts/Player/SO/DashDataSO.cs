using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/DashDataSO")]
public class DashDataSO : ScriptableObject
{
    public Color trailColor = Color.white;
    public float trailCycle = 0.08f;
    public float duration = 0.2f;

    [Space(20f)]
    public float dashPower = 10f;
    public float dashTime = 0.3f;
    public Ease dashEase = Ease.Linear;
    public float dashRechargeTime = 0.5f;

    [Header("Camera")]
    public float frequency = 10f;
    public float amplitude = 3f;
    public float shakeTime = 0.2f;
    public Ease easeType = Ease.Linear;
}
