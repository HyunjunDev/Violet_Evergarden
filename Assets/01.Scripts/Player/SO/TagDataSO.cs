using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/TagData")]
public class TagDataSO : ScriptableObject
{
    public RuntimeAnimatorController hanaAnimatorController;
    public RuntimeAnimatorController genAnimatorController;

    public ShakeCameraDataSO shakeCameraData;
}
