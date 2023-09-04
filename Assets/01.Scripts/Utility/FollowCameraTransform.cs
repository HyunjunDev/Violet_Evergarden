using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraTransform : MonoBehaviour
{
    Transform _targetTrm = null;

    private void Awake()
    {
        _targetTrm = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Vector2 position = _targetTrm.position;
        transform.position = position;
    }
}
