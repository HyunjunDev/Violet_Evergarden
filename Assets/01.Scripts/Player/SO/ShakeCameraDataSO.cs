using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ShakeCameraData")]
public class ShakeCameraDataSO : ScriptableObject
{
    public float frequency = 10f;
    public float amplitude = 3f;
    public float shakeTime = 0.2f;
    public Ease easeType = Ease.Linear;
}
