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
        if(input.sqrMagnitude > 0f)
        {
            //StartCoroutine(DashCoroutine(input));
            _myCharacter.LockActionCharacterByModule<JumpModule>(true);
            _myCharacter.LockActionCharacterByModule<MoveModule>(true);
            _myCharacter.ExitActionCharacterByModule<JumpModule>();
            _myCharacter.ExitActionCharacterByModule<MoveModule>();
            _myCharacter.ExitActionCharacterByModule<GravityModule>();
            Sequence dashSeq = DOTween.Sequence();
            StartCoroutine(DashCoroutine(input));
            _targetDashPower = input.normalized * _myCharacter.characterMovingManager.characterMoveDataSO.dashPower;
            _myCharacter.characterMovingManager.ResetMovingManager();
            _curDash = Vector2.zero;
            if (_targetDashPower.x < 0.1f)
                _myCharacter.LockActionCharacterByModule<GravityModule>(true);
            dashSeq.Append(DOTween.To(() => _curDash, x =>
            {
                _curDash = x;
                _myCharacter.characterMovingManager.currentHorizontalSpeed = _curDash.x;
                _myCharacter.characterMovingManager.currentVerticalSpeed = _curDash.y * 0.8f;
            },
            _targetDashPower, _myCharacter.characterMovingManager.characterMoveDataSO.dashTime))
                .SetEase(_myCharacter.characterMovingManager.characterMoveDataSO.dashEase);
            dashSeq.AppendCallback(() =>
            {
                _myCharacter.LockActionCharacterByModule<JumpModule>(false);
                _myCharacter.LockActionCharacterByModule<MoveModule>(false);
                _myCharacter.LockActionCharacterByModule<GravityModule>(false);
            });
        }
    }

    private IEnumerator DashCoroutine(Vector2 input)
    {
        _myCharacter.characterMovingManager.ResetMovingManager();
        _myCharacter.LockActionCharacterByModule<JumpModule>(true);
        _myCharacter.LockActionCharacterByModule<MoveModule>(true);
        _myCharacter.LockActionCharacterByModule<GravityModule>(true);
        Vector2 dashPower = input.normalized * _myCharacter.characterMovingManager.characterMoveDataSO.dashPower;
        _myCharacter.characterMovingManager.currentHorizontalSpeed = dashPower.x;
        _myCharacter.characterMovingManager.currentVerticalSpeed = dashPower.y;
        yield return new WaitForSeconds(_myCharacter.characterMovingManager.characterMoveDataSO.dashTime);
        _myCharacter.LockActionCharacterByModule<JumpModule>(false);
        _myCharacter.LockActionCharacterByModule<MoveModule>(false);
        _myCharacter.LockActionCharacterByModule<GravityModule>(false);
        yield break;
    }
}
