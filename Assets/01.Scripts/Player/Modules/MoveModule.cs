using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModule : CharacterModule
{
    // ���� �������� 1�� ��
    private float _apexPoint = 0f;

    public void Move(float xInput)
    {
        if (_locked)
        {
            return;
        }

        CharacterMovingManager moveMgr = _myCharacter.characterMovingManager;

        if(xInput != 0f)
        {
            // Set horizontal move speed
            moveMgr.currentHorizontalSpeed += xInput * moveMgr.characterMoveDataSO.acceleration * Time.deltaTime;

            // clamped by max frame movement
            moveMgr.currentHorizontalSpeed = Mathf.Clamp(moveMgr.currentHorizontalSpeed, 
                -moveMgr.characterMoveDataSO.moveClamp, moveMgr.characterMoveDataSO.moveClamp);

            // apexBonus ����
            var apexBonus = Mathf.Sign(xInput) * 2f * _apexPoint;
            moveMgr.currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else
        {
            // ���� �̵����� �ʾҴٸ� ���� �پ���
            moveMgr.currentHorizontalSpeed = Mathf.MoveTowards(moveMgr.currentHorizontalSpeed, 
                0, moveMgr.characterMoveDataSO.deAcceleration * Time.deltaTime);
        }

        // �����̳� ������ ����� ��
        if (moveMgr.currentHorizontalSpeed > 0 && _myCharacter.characterCollider.GetCollision(EBoundType.Left, false) || 
            moveMgr.currentHorizontalSpeed < 0 && _myCharacter.characterCollider.GetCollision(EBoundType.Right, false))
        {
            moveMgr.currentHorizontalSpeed = 0;
        }
    }

    public override void Exit()
    {
    }

    protected override void InitModule()
    {
    }
}
