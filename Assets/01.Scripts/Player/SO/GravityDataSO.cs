using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/GravityDataSO")]
public class GravityDataSO : ScriptableObject
{
    public float fallClamp = -40f;
    public float minFallSpeed = 80f;
    public float maxFallSpeed = 120f;
}
