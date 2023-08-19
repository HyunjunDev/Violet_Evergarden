using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class JumpModule : PlayerModule
{
    private float _fallSpeed = 0f;
    public float fallSpeed => _fallSpeed;

    // 점프 정점에서 1이 됨
    private float _apexPoint = 0f;
    public float apexPoint => _apexPoint;

    private bool _jumpDown = false;
    public bool jumpDown
    {
        get => _jumpDown;
        set
        {
            if (_jumpable == false)
            {
                return;
            }
            _jumpDown = value;
        }
    }

    private bool _jumpUp = false;
    public bool jumpUp { get => _jumpUp; set => _jumpUp = value; }

    private bool _jumpEndEarly = false;
    public bool jumpEndEarly => _jumpEndEarly;

    // coyoteTime은 땅을 벗어났을 때 점프할 수 있는 것이 유지되는 시간
    private bool _coyoteUseable = false;
    public bool cotoyeUseable => _coyoteUseable;
    private float _coyoteTimer = 0f;

    private bool _jumpable = false;
    public bool jumpable { get => _jumpable; set => _jumpable = value; }

    public override void Exit()
    {
        _fallSpeed = 0f;
        _apexPoint = 0f;
        _jumpDown = false;
        _jumpUp = false;
        _jumpEndEarly = false;
    }

    protected override void InitModule()
    {
        _player.playerCollider.onGrounded += OnGrounded;
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        CalculateJumpApex();
        CalculateJump();
    }

    public void JumpRecharge()
    {
        _fallSpeed = 0f;
        _apexPoint = 0f;
        _jumpDown = false;
        _jumpable = true;
    }

    public void JumpEnd()
    {
        _jumpUp = true;
    }

    private void OnGrounded()
    {
        _fallSpeed = 0f;
        _apexPoint = 0f;
        _jumpDown = false;
        _jumpable = true;
        Vector2 spawnPoint = _player.transform.position;
        spawnPoint.y -= 0.3f;
        GameObject.Instantiate(_player.LandingParticle, spawnPoint, Quaternion.identity).Play();
    }

    private void CalculateJumpApex()
    {
        if (_locked)
        {
            return;
        }

        if (!_player.playerCollider.GetCollision(EBoundType.Down, false))
        {
            // 한계값부터 0까지 
            _apexPoint = Mathf.InverseLerp(_player.movingController.characterMoveDataSO.jumpApexThreshold
                , 0f, Mathf.Abs(_player.rigid.velocity.y));

            _fallSpeed = Mathf.Lerp(_player.movingController.characterMoveDataSO.minFallSpeed,
                _player.movingController.characterMoveDataSO.maxFallSpeed, _apexPoint);
        }
    }

    private void CalculateJump()
    {
        if (_locked)
        {
            return;
        }

        if (_jumpDown && _jumpable)
        {
            JumpStart();
        }

        // 머리 꽁
        if (_player.playerCollider.GetCollision(EBoundType.Up, false) && _player.movingController.currentVerticalSpeed > 0f)
        {
            _player.movingController.currentVerticalSpeed = 0f;
        }

        // 빨리 떨어지기
        if (!_player.playerCollider.GetCollision(EBoundType.Down, false) && !_jumpEndEarly && _jumpUp && _player.rigid.velocity.y > 0f)
        {
            _jumpEndEarly = true;
        }
    }

    private void JumpStart()
    {
        Vector2 spawnPoint = _player.transform.position;
        spawnPoint.y -= 0.3f;
        GameObject.Instantiate(_player.JumpParticle, spawnPoint, Quaternion.identity).Play();

        _player.playerAnimation.JumpAnimation();
        Exit();
        _jumpDown = false;
        _jumpable = false;
        _player.movingController.currentVerticalSpeed = _player.movingController.characterMoveDataSO.jumpPower;
    }
}
