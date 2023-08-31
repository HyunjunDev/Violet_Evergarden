using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/ThrowDaggerData")]
public class ThrowDaggerDataSO : ScriptableObject
{
    [Header("잔상 색")]
    public Color trailColor = Color.white;
    public float trailCycle = 0.08f;
    public float duration = 0.2f;

    [Header("페이드 이미지")]
    public float startAlpha = 0.5f;
    public float animationTime = 0.5f;

    [Header("던질 때 카메라")]
    public ShakeCameraDataSO th_shakeCameraData;

    [Header("착지할 때 카메라")]
    public ShakeCameraDataSO la_shakeCameraData;
}
