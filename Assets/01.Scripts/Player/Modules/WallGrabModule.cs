using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabModule : PlayerModule
{
    private float keepTimer = 0f;

    public override void Exit()
    {
        _excuting = false;
        keepTimer = 0f;
        _player.GetModule<GravityModule>(EPlayerModuleType.Gravity).gravityModifier = 1f;
        _player.playerCollider.onGroundExited?.Invoke();
    }

    protected override void InitModule()
    {
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        if(!_excuting)
        {
            return;
        }

        keepTimer += Time.deltaTime;
        if(keepTimer <= Time.fixedDeltaTime * 3f)
        {
            return;
        }

        if (!_player.playerCollider.GetCollision(EBoundType.Left) && !_player.playerCollider.GetCollision(EBoundType.Right))
        {
            Exit();
        }
    }

    public void TryWallGrab()
    {
        _player.playerCollider.CheckCollision();
        if (_player.playerCollider.GetCollision(EBoundType.Left) || _player.playerCollider.GetCollision(EBoundType.Right)
            || !_player.playerCollider.GetCollision(EBoundType.Up) || !_player.playerCollider.GetCollision(EBoundType.Down))
        {
            _player.movingController.ResetMovingManager();
            _excuting = true;
            keepTimer = 0f;
            _player.GetModule<JumpModule>(EPlayerModuleType.Jump).JumpRecharge();
            _player.GetModule<GravityModule>(EPlayerModuleType.Gravity).gravityModifier = 0.2f;
        }
    }
}
