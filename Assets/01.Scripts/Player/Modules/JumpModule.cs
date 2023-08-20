using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class JumpModule : PlayerModule
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
            if (!_jumpable)
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
    private Coroutine _coyoteCoroutine = null;

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
        _player.playerCollider.onGroundExited += OnGroundExited;
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
        _excuting = false;
    }

    private void OnGroundExited()
    {
        if(_player.CheckExcutingModules(EPlayerModuleType.Dash))
        {
            return;
        }

        if(_coyoteCoroutine != null)
        {
            StopCoroutine(_coyoteCoroutine);
        }

        _coyoteCoroutine = StartCoroutine(CoyoteCoroutine());
    }

    private IEnumerator CoyoteCoroutine()
    {
        yield return new WaitForSeconds(_player.JumpDataSO.coyoteTime);
        if (_player.CheckExcutingModules(EPlayerModuleType.Dash))
        {
            yield break;
        }
        _jumpable = false;
    }

    private void OnGrounded()
    {
        _excuting = false;
        JumpRecharge();
        Vector2 spawnPoint = _player.transform.position;
        spawnPoint.y -= 0.3f;
        GameObject landingParticle = PoolManager.Instance.Pop(EPoolType.LandingParticle).gameObject;
        landingParticle.transform.position = spawnPoint;
    }

    private void CalculateJumpApex()
    {
        if (_locked)
        {
            return;
        }

        if (!_player.playerCollider.GetCollision(EBoundType.Down, false))
        {
            // �Ѱ谪���� 0���� 
            _apexPoint = Mathf.InverseLerp(_player.JumpDataSO.jumpApexThreshold
                , 0f, Mathf.Abs(_player.rigid.velocity.y));

            _fallSpeed = Mathf.Lerp(_player.GravityDataSO.minFallSpeed,
                _player.GravityDataSO.maxFallSpeed, _apexPoint);
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

        // �Ӹ� ��
        if (_player.playerCollider.GetCollision(EBoundType.Up, false) && _player.movingController.currentVerticalSpeed > 0f)
        {
            _player.movingController.currentVerticalSpeed = 0f;
        }

        // ���� ��������
        if (!_player.playerCollider.GetCollision(EBoundType.Down, false) && !_jumpEndEarly && _jumpUp && _player.rigid.velocity.y > 0f)
        {
            _excuting = false;
            _jumpEndEarly = true;
        }
    }

    private void JumpStart()
    {
        Vector2 spawnPoint = _player.transform.position;
        spawnPoint.y -= 0.3f;
        GameObject jumpParticle = PoolManager.Instance.Pop(EPoolType.JumpParticle).gameObject;
        jumpParticle.transform.position = spawnPoint;

        _player.playerAnimation.JumpAnimation();
        Exit();
        _excuting = true;
        _jumpDown = false;
        _jumpable = false;
        _player.movingController.currentVerticalSpeed = _player.JumpDataSO.jumpPower;
    }
}
