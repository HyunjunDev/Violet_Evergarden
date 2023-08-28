using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModule : PlayerModule
{
    public override void Exit()
    {
        _excuting = false;
        _player.movingController.currentVerticalSpeed = 0f;
    }

    protected override void InitModule()
    {
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        CalculateGravity();
    }

    private void CalculateGravity()
    {
        if(_locked)
        {
            return;
        }

        if (_player.playerCollider.GetCollision(EBoundType.Down))
        {
            // 떨어지고 있을 때 바닥에 닿았다면 속도 초기화
            if (_player.movingController.currentVerticalSpeed < 0f)
            {
                _player.movingController.currentVerticalSpeed = 0f;
            }
        }
        else
        {
            JumpModule jumpModule = _player.GetModule<JumpModule>(EPlayerModuleType.Jump);
            float fallSpeed = 0f;

            if (jumpModule != null)
            {
                fallSpeed = jumpModule.jumpEndEarly && _player.movingController.currentVerticalSpeed > 0f ?
                    jumpModule.fallSpeed * _player.JumpDataSO.jumpEndEarlyGravityModifier : jumpModule.fallSpeed;
            }

            _player.movingController.currentVerticalSpeed -= fallSpeed * Time.deltaTime;
            _excuting = fallSpeed > 0f;

            if (_player.movingController.currentVerticalSpeed < _player.GravityDataSO.fallClamp)
            {
                _player.movingController.currentVerticalSpeed = _player.GravityDataSO.fallClamp;
            }
        }
    }
}
