using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingController : MonoBehaviour
{
    private Player _player = null;

    [SerializeField]
    private PlayerMoveDataSO _characterMoveDataSO = null;
    public PlayerMoveDataSO characterMoveDataSO => _characterMoveDataSO;

    private float _currentHorizontalSpeed = 0f;
    public float currentHorizontalSpeed { get => _currentHorizontalSpeed; set => _currentHorizontalSpeed = value; }

    private float _currentVerticalSpeed = 0f;
    public float currentVerticalSpeed { get => _currentVerticalSpeed; set => _currentVerticalSpeed = value; }


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        _player.rigid.velocity = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed);

    }

    public void ResetMovingManager()
    {
        _currentHorizontalSpeed = _currentVerticalSpeed = 0f;
        _player.rigid.velocity = Vector2.zero;
    }
}