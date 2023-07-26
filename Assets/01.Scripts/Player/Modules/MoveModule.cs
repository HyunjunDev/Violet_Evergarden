using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModule : CharacterModule
{
    // 점프 정점에서 1이 됨
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

            // apexBonus 적용
            var apexBonus = Mathf.Sign(xInput) * 2f * _apexPoint;
            moveMgr.currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else
        {
            // 만약 이동하지 않았다면 가속 줄어들기
            moveMgr.currentHorizontalSpeed = Mathf.MoveTowards(moveMgr.currentHorizontalSpeed, 
                0, moveMgr.characterMoveDataSO.deAcceleration * Time.deltaTime);
        }

        // 왼쪽이나 오른쪽 닿았을 때
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
