using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabModule : PlayerModule
{
    private float _keepTimer = 0f;
    private EFlipState _enterWallFlipState = EFlipState.None;
    public EFlipState EnterWallFlipState => _enterWallFlipState;

    private Coroutine _wallExitCoroutine = null;

    public override void Exit()
    {
        _excuting = false;
        _keepTimer = 0f;
        _player.playerCollider.onGroundExited?.Invoke();
        if (_wallExitCoroutine != null)
        {
            StopCoroutine(_wallExitCoroutine);
        }
        _player.GetModule<GravityModule>(EPlayerModuleType.Gravity).gravityModifier = 1f;
    }

    protected override void InitModule()
    {
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        if (!_excuting)
        {
            return;
        }

        _keepTimer += Time.deltaTime;
        if (_keepTimer <= Time.fixedDeltaTime * 3f)
        {
            return;
        }

        if (!_player.playerCollider.GetCollision(EBoundType.Left) && !_player.playerCollider.GetCollision(EBoundType.Right))
        {
            WallExit();
        }
    }

    public void TryWallGrab()
    {
        _player.playerCollider.CheckCollision();
        bool isHorizontal = _player.playerCollider.GetCollision(EBoundType.Left) || _player.playerCollider.GetCollision(EBoundType.Right);
        if (_player.playerCollider.GetCollision(EBoundType.Up))
        {
            _player.GetModule<JumpModule>(EPlayerModuleType.Jump).jumpable = false;
        }
        else if (isHorizontal)
        {
            _player.playerAnimation.WallGrabAnimation();
            _player.movingController.ResetMovingManager();
            _excuting = true;
            _keepTimer = 0f;
            _player.GetModule<JumpModule>(EPlayerModuleType.Jump).JumpRecharge();
            _player.GetModule<GravityModule>(EPlayerModuleType.Gravity).gravityModifier = 0.1f;
            _enterWallFlipState = _player.playerRenderer.currentFlipState;
        }
    }

    private void WallExit()
    {
        if (_wallExitCoroutine != null)
        {
            StopCoroutine(_wallExitCoroutine);
        }
        _wallExitCoroutine = StartCoroutine(WallExitCoroutine());
    }

    private IEnumerator WallExitCoroutine()
    {
        _player.GetModule<GravityModule>(EPlayerModuleType.Gravity).gravityModifier = 1f;
        yield return new WaitForSeconds(_player.JumpDataSO.wallCoyoteTime);
        _excuting = false;
        _keepTimer = 0f;
        _player.playerCollider.onGroundExited?.Invoke();
    }
}
