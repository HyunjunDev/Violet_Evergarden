using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/ThrowDaggerData")]
public class ThrowDaggerDataSO : ScriptableObject
{
    [Header("�ܻ�")]
    public TrailDataSO trailData;

    [Header("���̵� �̹���")]
    public float startAlpha = 0.5f;
    public float animationTime = 0.5f;

    [Header("���� �� ī�޶�")]
    public ShakeCameraDataSO th_shakeCameraData;

    [Header("������ �� ī�޶�")]
    public ShakeCameraDataSO la_shakeCameraData;
}
