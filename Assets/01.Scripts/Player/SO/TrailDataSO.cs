using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TrailData")]
public class TrailDataSO : ScriptableObject
{
    public Color trailColor = Color.white;
    public float duration = 0f;
    public float fadeOutTime = 0.5f;
}
