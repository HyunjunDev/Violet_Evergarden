using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModule : PlayerModule
{
    public void Move(float xInput)
    {
        if (_locked)
        {
            return;
        }

        // 렌더러, 애니메이션 적용
        _player.playerAnimation.MoveInputAnimation(xInput);
        _player.playerRenderer.MoveInputFlip(xInput);

        if(xInput != 0f)
        {
            // Set horizontal move speed
            _player.movingController.currentHorizontalSpeed += xInput * _player.MovementDataSO.acceleration * _player.MultiplierDataSO.speedMultiplier * Time.deltaTime;

            // clamped by max frame movement
            _player.movingController.currentHorizontalSpeed = Mathf.Clamp(_player.movingController.currentHorizontalSpeed, 
                -_player.MovementDataSO.moveClamp, _player.MovementDataSO.moveClamp * _player.MultiplierDataSO.speedMultiplier);

            // apexBonus 적용
            float apexBonus = 0f;
            if (_player.GetModule<JumpModule>(EPlayerModuleType.Jump) != null)
            {
                apexBonus = Mathf.Sign(xInput) * _player.MovementDataSO.apexBonus * _player.GetModule<JumpModule>(EPlayerModuleType.Jump).apexPoint;
            }

            _player.movingController.currentHorizontalSpeed += apexBonus * _player.MultiplierDataSO.speedMultiplier * Time.deltaTime;
        }
        else
        {
            // 만약 이동하지 않았다면 가속 줄어들기
            _player.movingController.currentHorizontalSpeed = Mathf.MoveTowards(_player.movingController.currentHorizontalSpeed, 
                0, _player.MovementDataSO.deAcceleration * _player.MultiplierDataSO.speedMultiplier * Time.deltaTime);
        }

        // 왼쪽이나 오른쪽 닿았을 때
        if (_player.movingController.currentHorizontalSpeed < 0f && _player.playerCollider.GetCollision(EBoundType.Left) || 
            _player.movingController.currentHorizontalSpeed > 0f && _player.playerCollider.GetCollision(EBoundType.Right))
        {
            _player.movingController.currentHorizontalSpeed = 0;
        }
    }

    public void ResetHorizontalSpeed()
    {
        _player.movingController.currentHorizontalSpeed = 0f;
    }

    public override void Exit()
    {
        _player.movingController.currentHorizontalSpeed = 0f;
    }

    protected override void InitModule()
    {
    }
}
