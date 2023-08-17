using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpModule : CharacterModule
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
        _myCharacter.characterCollider.onGrounded += OnGrounded;
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        CalculateJumpApex();
        CalculateJump();
    }

    private void OnGrounded()
    {
        DashModule dashModule = _myCharacter.GetModule<DashModule>(ECharacterModuleType.Dash);
        if (dashModule != null)
        {
            if(dashModule.Dashing)
            {
                return;
            }
        }
        Exit();
        _jumpable = true;
    }

    private void CalculateJumpApex()
    {
        if (_locked)
        {
            return;
        }

        if (!_myCharacter.characterCollider.GetCollision(EBoundType.Down, false))
        {
            // 한계값부터 0까지 
            _apexPoint = Mathf.InverseLerp(_myCharacter.characterMovingManager.characterMoveDataSO.jumpApexThreshold
                , 0f, Mathf.Abs(_myCharacter.rigid.velocity.y));

            _fallSpeed = Mathf.Lerp(_myCharacter.characterMovingManager.characterMoveDataSO.minFallSpeed,
                _myCharacter.characterMovingManager.characterMoveDataSO.maxFallSpeed, _apexPoint);
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
            _myCharacter.characterAnimation.JumpAnimation();
            Exit();
            _jumpable = false;
            _myCharacter.characterMovingManager.currentVerticalSpeed = _myCharacter.characterMovingManager.characterMoveDataSO.jumpPower;
        }

        // 머리 꽁
        if (_myCharacter.characterCollider.GetCollision(EBoundType.Up, false) && _myCharacter.characterMovingManager.currentVerticalSpeed > 0f)
        {
            _myCharacter.characterMovingManager.currentVerticalSpeed = 0f;
        }

        // 빨리 떨어지기
        if (!_myCharacter.characterCollider.GetCollision(EBoundType.Down, false) && !_jumpEndEarly && _jumpUp && _myCharacter.rigid.velocity.y > 0f)
        {
            _jumpEndEarly = true;
        }
    }
}
