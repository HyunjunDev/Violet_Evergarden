using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/MultiplierData")]
public class MultiplierDataSO : ScriptableObject
{
    public float gravityMultiplier = 1f;
    public float dashMultiplier = 1f;
    public float jumpMultiplier = 1f;
    public float throwSpeedMultiplier = 1f;
    public float speedMultiplier = 1f;
}
