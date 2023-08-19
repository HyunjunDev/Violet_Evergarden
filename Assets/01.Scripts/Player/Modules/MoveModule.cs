using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModule : CharacterModule
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

        MovingController moveMgr = _player.movingController;

        if(xInput != 0f)
        {
            // Set horizontal move speed
            moveMgr.currentHorizontalSpeed += xInput * moveMgr.characterMoveDataSO.acceleration * Time.deltaTime;

            // clamped by max frame movement
            moveMgr.currentHorizontalSpeed = Mathf.Clamp(moveMgr.currentHorizontalSpeed, 
                -moveMgr.characterMoveDataSO.moveClamp, moveMgr.characterMoveDataSO.moveClamp);

            // apexBonus ����
            float apexBonus = 0f;
            if (_player.GetModule<JumpModule>(ECharacterModuleType.Jump) != null)
            {
                apexBonus = Mathf.Sign(xInput) * moveMgr.characterMoveDataSO.apexBonus * _player.GetModule<JumpModule>(ECharacterModuleType.Jump).apexPoint;
            }

            moveMgr.currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else
        {
            // ���� �̵����� �ʾҴٸ� ���� �پ���
            moveMgr.currentHorizontalSpeed = Mathf.MoveTowards(moveMgr.currentHorizontalSpeed, 
                0, moveMgr.characterMoveDataSO.deAcceleration * Time.deltaTime);
        }

        // �����̳� ������ ����� ��
        if (moveMgr.currentHorizontalSpeed < 0f && _player.playerCollider.GetCollision(EBoundType.Left, false) || 
            moveMgr.currentHorizontalSpeed > 0f && _player.playerCollider.GetCollision(EBoundType.Right, false))
        {
            moveMgr.currentHorizontalSpeed = 0;
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
