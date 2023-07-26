using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModule : CharacterModule
{
    public override void Exit()
    {
        _myCharacter.characterMovingManager.currentVerticalSpeed = 0f;
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
        if (_myCharacter.characterCollider.GetCollision(EBoundType.Down, false))
        {
            // 떨어지고 있을 때 바닥에 닿았다면 속도 초기화
            if (_myCharacter.characterMovingManager.currentVerticalSpeed < 0f)
            {
                _myCharacter.characterMovingManager.currentVerticalSpeed = 0f;
            }
        }
        else
        {
            JumpModule jumpModule = _myCharacter.GetModule<JumpModule>();
            float fallSpeed = 0f;

            if (jumpModule != null)
            {
                fallSpeed = jumpModule.jumpEndEarly && _myCharacter.characterMovingManager.currentVerticalSpeed > 0f ?
                    jumpModule.fallSpeed * _myCharacter.characterMovingManager.characterMoveDataSO.jumpEndEarlyGravityModifier : jumpModule.fallSpeed;
            }

            _myCharacter.characterMovingManager.currentVerticalSpeed -= fallSpeed * Time.deltaTime;

            if (_myCharacter.characterMovingManager.currentVerticalSpeed < _myCharacter.characterMovingManager.characterMoveDataSO.fallClamp)
            {
                _myCharacter.characterMovingManager.currentVerticalSpeed = _myCharacter.characterMovingManager.characterMoveDataSO.fallClamp;
            }
        }
    }
}
