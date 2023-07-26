using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMovingManager : MonoBehaviour
{
    private MyCharacter _myCharacter = null;

    [SerializeField]
    private CharacterMoveDataSO _characterMoveDataSO = null;
    private Vector3 _velocity = Vector3.zero;
    private bool _isGrounded = false;

    private Vector3 _lastPosition = Vector3.zero;
    private float _currentHorizontalSpeed = 0f;
    private float _currentVerticalSpeed = 0f;

    public CharacterMoveDataSO characterMoveDataSO => _characterMoveDataSO;
    public float currentHorizontalSpeed { get => _currentHorizontalSpeed; set => _currentHorizontalSpeed = value; }
    public float currentVerticalSpeed { get => _currentVerticalSpeed; set => _currentVerticalSpeed = value; }

    private void Awake()
    {
        _myCharacter = GetComponent<MyCharacter>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        _myCharacter.rigid.velocity = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed);
    }

    #region Collision
    #endregion

    #region Walk

    #endregion
}