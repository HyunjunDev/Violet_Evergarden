using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpModule : CharacterModule
{
    private float _fallSpeed = 0f;
    public float fallSpeed => _fallSpeed;

    // ���� �������� 1�� ��
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

    // coyoteTime�� ���� ����� �� ������ �� �ִ� ���� �����Ǵ� �ð�
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
            // �Ѱ谪���� 0���� 
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
            _jumpable = false;
            _jumpDown = false;
            _myCharacter.characterMovingManager.currentVerticalSpeed = _myCharacter.characterMovingManager.characterMoveDataSO.jumpPower;
        }

        // �Ӹ� ��
        if (_myCharacter.characterCollider.GetCollision(EBoundType.Up, false) && _myCharacter.rigid.velocity.y > 0f)
        {
            Debug.Log("�Ӹ� ��");
            _myCharacter.characterMovingManager.currentVerticalSpeed = 0f;
        }

        // ���� ��������
        if (!_myCharacter.characterCollider.GetCollision(EBoundType.Down, false) && !_jumpEndEarly && _jumpUp && _myCharacter.rigid.velocity.y > 0f)
        {
        }
    }
}
