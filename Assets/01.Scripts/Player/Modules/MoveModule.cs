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

        // 렌더러, 애니메이션 적용
        _myCharacter.characterAnimation.MoveInputAnimation(xInput);
        _myCharacter.characterRenderer.MoveInputFlip(xInput);

        CharacterMovingManager moveMgr = _myCharacter.characterMovingManager;

        if(xInput != 0f)
        {
            // Set horizontal move speed
            moveMgr.currentHorizontalSpeed += xInput * moveMgr.characterMoveDataSO.acceleration * Time.deltaTime;

            // clamped by max frame movement
            moveMgr.currentHorizontalSpeed = Mathf.Clamp(moveMgr.currentHorizontalSpeed, 
                -moveMgr.characterMoveDataSO.moveClamp, moveMgr.characterMoveDataSO.moveClamp);

            // apexBonus 적용
            float apexBonus = 0f;
            if (_myCharacter.GetModule<JumpModule>(ECharacterModuleType.Jump) != null)
            {
                apexBonus = Mathf.Sign(xInput) * moveMgr.characterMoveDataSO.apexBonus * _myCharacter.GetModule<JumpModule>(ECharacterModuleType.Jump).apexPoint;
            }

            moveMgr.currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else
        {
            // 만약 이동하지 않았다면 가속 줄어들기
            moveMgr.currentHorizontalSpeed = Mathf.MoveTowards(moveMgr.currentHorizontalSpeed, 
                0, moveMgr.characterMoveDataSO.deAcceleration * Time.deltaTime);
        }

        // 왼쪽이나 오른쪽 닿았을 때
        if (moveMgr.currentHorizontalSpeed < 0f && _myCharacter.characterCollider.GetCollision(EBoundType.Left, false) || 
            moveMgr.currentHorizontalSpeed > 0f && _myCharacter.characterCollider.GetCollision(EBoundType.Right, false))
        {
            moveMgr.currentHorizontalSpeed = 0;
        }
    }

    public override void Exit()
    {
        _myCharacter.characterMovingManager.currentHorizontalSpeed = 0f;
    }

    protected override void InitModule()
    {
    }
}
