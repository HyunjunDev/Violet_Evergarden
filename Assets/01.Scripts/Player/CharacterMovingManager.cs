using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMovingManager : MonoBehaviour
{
    [SerializeField]
    private CharacterMoveDataSO _characterMoveDataSO = null;
    private Vector3 _velocity = Vector3.zero;
    private bool _isGrounded = false;

    private Vector3 _lastPosition = Vector3.zero;
    private float _currentHorizontalSpeed = 0f;
    private float _currentVerticalSpeed = 0f;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        CheckRunCollision();
    }

    #region Collision
    private BoxCollider2D _col = null;
    private Bounds _characterBounds => _col.bounds;
    [SerializeField]
    private LayerMask _groundLayer = 0;
    [SerializeField]
    private int _detectorCount = 3;
    [SerializeField]
    private float _detectionRayLength = 0f;
    [SerializeField, Range(0.1f, 0.3f)]
    private float _rayBuffer = 0.1f;

    private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
    private bool _colUp, _colRight, _colDown, _colLeft;

    #endregion
    private void CheckRunCollision()
    {
        CalculateRayRanged();

        var groundedCheck = RunDetection(_raysDown);
        _colUp = RunDetection(_raysUp);
        _colLeft = RunDetection(_raysLeft);
        _colRight = RunDetection(_raysRight);

        Debug.Log("GroundCheck : " + groundedCheck + "UP : " + _colUp + "Left : " + _colLeft + "Right : " + _colRight);
    }

    private bool RunDetection(RayRange range)
    {
        return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
    }

    private void CalculateRayRanged()
    {
        // This is crying out for some kind of refactor. 
        var b = new Bounds(_characterBounds.center, _characterBounds.size);

        _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
        _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
        _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
        _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
    }


    private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
    {
        for (var i = 0; i < _detectorCount; i++)
        {
            var t = (float)i / (_detectorCount - 1);
            yield return Vector2.Lerp(range.Start, range.End, t);
        }
    }
}

public struct RayRange
{
    public RayRange(float x1, float y1, float x2, float y2, Vector2 dir)
    {
        Start = new Vector2(x1, y1);
        End = new Vector2(x2, y2);
        Dir = dir;
    }

    public readonly Vector2 Start, End, Dir;
}