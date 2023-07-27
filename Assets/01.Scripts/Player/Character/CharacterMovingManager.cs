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
    public CharacterMoveDataSO characterMoveDataSO => _characterMoveDataSO;

    private float _currentHorizontalSpeed = 0f;
    public float currentHorizontalSpeed { get => _currentHorizontalSpeed; set => _currentHorizontalSpeed = value; }

    private float _currentVerticalSpeed = 0f;
    public float currentVerticalSpeed { get => _currentVerticalSpeed; set => _currentVerticalSpeed = value; }


    private void Awake()
    {
        _myCharacter = GetComponent<MyCharacter>();
    }

    private void FixedUpdate()
    {
        _myCharacter.rigid.velocity = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed);

    }

    public void ResetMovingManager()
    {
        _currentHorizontalSpeed = _currentVerticalSpeed = 0f;
        _myCharacter.rigid.velocity = Vector2.zero;
    }
}