using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/JumpDataSO")]
public class JumpDataSO : ScriptableObject
{
    public float jumpPower = 30f;
    public float jumpApexThreshold = 10f;
    public float jumpEndEarlyGravityModifier = 3f;
    public float coyoteTime = 0.02f;
    public float wallCoyoteTime = 0.035f;
}
