using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashModule : CharacterModule
{
    private Vector2 _targetDashPower = Vector2.zero;
    private Vector2 _curDash = Vector2.zero;

    private bool _dashing = false;
    public bool Dashing => _dashing;

    public override void Exit()
    {
        _dashing = false;
    }

    protected override void InitModule()
    {
    }

    public void DashStart(Vector2 input)
    {
        if (input.sqrMagnitude > 0f)
        {
            _dashing = true;
            _myCharacter.GetModule<JumpModule>(ECharacterModuleType.Jump).jumpUp = true;
            _myCharacter.LockActionCharacterByModule(true, ECharacterModuleType.Jump);

            _targetDashPower = input.normalized * _myCharacter.characterMovingManager.characterMoveDataSO.dashPower;
            _myCharacter.characterAnimation.DashAnimation(_targetDashPower);
            _myCharacter.characterMovingManager.ResetMovingManager();
            _curDash = Vector2.zero;

            HanaCharacter hana = _myCharacter as HanaCharacter;
            GameObject.Instantiate(hana.DashParticle, _myCharacter.transform.position, Quaternion.identity).Play();
            GameObject.Instantiate(hana.DashTrailParticle, _myCharacter.transform.position, GetDashRotation(_targetDashPower)).Play();
            _myCharacter.characterRenderer.TrailStart(hana.trailColor, hana.trailCycle, hana.duration);

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
                _myCharacter.LockActionCharacterByModule(false, ECharacterModuleType.Jump);
                _myCharacter.GetModule<JumpModule>(ECharacterModuleType.Jump).jumpable = true;
                _dashing = false;
            });
        }
    }

    private Quaternion GetDashRotation(Vector2 dir)
    {
        float angle = Vector2.Angle(Vector2.down, dir);
        angle *= Mathf.Sign(dir.x);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        return rot;
    }
}
