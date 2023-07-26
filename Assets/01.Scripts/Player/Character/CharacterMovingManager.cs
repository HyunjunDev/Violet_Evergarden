using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMovingManager : MonoBehaviour
{
    private CharacterCollider _characterCollider = null;

    [SerializeField]
    private CharacterMoveDataSO _characterMoveDataSO = null;
    private Vector3 _velocity = Vector3.zero;
    private bool _isGrounded = false;

    private Vector3 _lastPosition = Vector3.zero;
    private float _currentHorizontalSpeed = 0f;
    private float _currentVerticalSpeed = 0f;

    private void Awake()
    {
        _characterCollider = GetComponent<CharacterCollider>();
    }

    private void Update()
    {
    }

    #region Collision
    #endregion

    #region Walk

    #endregion
}