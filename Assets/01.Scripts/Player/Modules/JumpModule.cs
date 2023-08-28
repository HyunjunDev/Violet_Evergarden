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
    public bool jumpDown => _jumpDown;

    private bool _jumpUp = false;
    public bool jumpUp { get => _jumpUp; set => _jumpUp = value; }

    private bool _jumpEndEarly = false;
    public bool jumpEndEarly => _jumpEndEarly;

    // coyoteTime은 땅을 벗어났을 때 점프할 수 있는 것이 유지되는 시간
    private Coroutine _coyoteCoroutine = null;

    private bool _jumpable = false;
    public bool jumpable { get => _jumpable; set => _jumpable = value; }

    private Coroutine _jumpDownCoroutine = null;

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

    public void JumpKeyDown()
    {
        /*if(!_jumpable)
        {
            return;
        }*/
        if(_jumpDownCoroutine != null)
        {
            StopCoroutine(_jumpDownCoroutine);
        }
        _jumpDownCoroutine = StartCoroutine(JumpDownCoroutine());
    }

    private IEnumerator JumpDownCoroutine()
    {
        _jumpDown = true;
        yield return new WaitForSeconds(0.1f);
        _jumpDown = false;
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
            _jumpable = false;
            return;
        }

        if (_coyoteCoroutine != null)
        {
            StopCoroutine(_coyoteCoroutine);
        }

        _coyoteCoroutine = StartCoroutine(CoyoteCoroutine());
    }

    private IEnumerator CoyoteCoroutine()
    {
        yield return new WaitForSeconds(_player.JumpDataSO.coyoteTime);
        if (_player.CheckExcutingModules(EPlayerModuleType.Dash, EPlayerModuleType.WallGrab) || _player.playerCollider.GetCollision(EBoundType.Down))
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

        if (!_player.playerCollider.GetCollision(EBoundType.Down))
        {
            // 한계값부터 0까지 
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

        // 머리 꽁
        if (_player.playerCollider.GetCollision(EBoundType.Up) && _player.movingController.currentVerticalSpeed > 0f)
        {
            _player.movingController.currentVerticalSpeed = 0f;
        }

        // 빨리 떨어지기
        if (!_player.playerCollider.GetCollision(EBoundType.Down) && !_jumpEndEarly && _jumpUp && _player.rigid.velocity.y > 0f)
        {
            _excuting = false;
            _jumpEndEarly = true;
        }
    }

    private void JumpStart()
    {
        if(_player.CheckExcutingModules(EPlayerModuleType.WallGrab))
        {
            //벽쩜
            _player.ExitModules(EPlayerModuleType.WallGrab);
        }

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
