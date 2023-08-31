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

        // ������, �ִϸ��̼� ����
        _player.playerAnimation.MoveInputAnimation(xInput);
        _player.playerRenderer.MoveInputFlip(xInput);

        if(xInput != 0f)
        {
            // Set horizontal move speed
            _player.movingController.currentHorizontalSpeed += xInput * _player.MovementDataSO.acceleration * Time.deltaTime;

            // clamped by max frame movement
            _player.movingController.currentHorizontalSpeed = Mathf.Clamp(_player.movingController.currentHorizontalSpeed, 
                -_player.MovementDataSO.moveClamp, _player.MovementDataSO.moveClamp);

            // apexBonus ����
            float apexBonus = 0f;
            if (_player.GetModule<JumpModule>(EPlayerModuleType.Jump) != null)
            {
                apexBonus = Mathf.Sign(xInput) * _player.MovementDataSO.apexBonus * _player.GetModule<JumpModule>(EPlayerModuleType.Jump).apexPoint;
            }

            _player.movingController.currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else
        {
            // ���� �̵����� �ʾҴٸ� ���� �پ���
            _player.movingController.currentHorizontalSpeed = Mathf.MoveTowards(_player.movingController.currentHorizontalSpeed, 
                0, _player.MovementDataSO.deAcceleration * Time.deltaTime);
        }

        // �����̳� ������ ����� ��
        if (_player.movingController.currentHorizontalSpeed < 0f && _player.playerCollider.GetCollision(EBoundType.Left) || 
            _player.movingController.currentHorizontalSpeed > 0f && _player.playerCollider.GetCollision(EBoundType.Right))
        {
            _player.movingController.currentHorizontalSpeed = 0;
        }
    }

    public override void Exit()
    {
        _player.movingController.currentHorizontalSpeed = 0f;
    }

    protected override void InitModule()
    {
    }
}
