using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashModule : CharacterModule
{
    private Vector2 _targetDashPower = Vector2.zero;
    private Vector2 _curDash = Vector2.zero;

    public override void Exit()
    {
    }

    protected override void InitModule()
    {
    }

    public void DashStart(Vector2 input)
    {
        if (input.sqrMagnitude > 0f)
        {
            _myCharacter.GetModule<JumpModule>(ECharacterModuleType.Jump).jumpUp = true;

            _targetDashPower = input.normalized * _myCharacter.characterMovingManager.characterMoveDataSO.dashPower;
            _myCharacter.characterAnimation.DashAnimation(_targetDashPower);
            _myCharacter.characterMovingManager.ResetMovingManager();
            _curDash = Vector2.zero;

            CameraManager.Instance.ShakeCamera(_myCharacter.characterMovingManager.characterMoveDataSO.fre,
                _myCharacter.characterMovingManager.characterMoveDataSO.amp,
                _myCharacter.characterMovingManager.characterMoveDataSO.animationTime,
                _myCharacter.characterMovingManager.characterMoveDataSO.easeT);
            Sequence dashSeq = DOTween.Sequence();
            dashSeq.Append(DOTween.To(() => _curDash, x =>
            {
                _curDash = x;
                _myCharacter.characterMovingManager.currentHorizontalSpeed = _curDash.x;
                _myCharacter.characterMovingManager.currentVerticalSpeed = _curDash.y * 0.8f;
            }, _targetDashPower, _myCharacter.characterMovingManager.characterMoveDataSO.dashTime))
                .SetEase(_myCharacter.characterMovingManager.characterMoveDataSO.dashEase);

            dashSeq.AppendCallback(() =>
            {
                if (_myCharacter.characterMovingManager.currentVerticalSpeed < 0.2f)
                {
                    _myCharacter.characterAnimation.IdleAnimation();
                }
                _myCharacter.GetModule<JumpModule>(ECharacterModuleType.Jump).jumpable = true;
            });
        }
    }
}
