using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Quaternion GetDashRotation(Vector2 dir, float weg)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle + weg, Vector3.forward);
        return rot;
    }
}
