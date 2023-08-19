using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashModule : PlayerModule
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
            _player.GetModule<JumpModule>(EPlayerModuleType.Jump).jumpUp = true;
            _player.LockActionCharacterByModule(true, EPlayerModuleType.Jump);

            _targetDashPower = input.normalized * _player.movingController.characterMoveDataSO.dashPower;
            _player.playerAnimation.DashAnimation();
            _player.movingController.ResetMovingManager();
            _curDash = Vector2.zero;

            GameObject.Instantiate(_player.DashParticle, _player.transform.position, Quaternion.identity).Play();
            GameObject.Instantiate(_player.DashTrailParticle, _player.transform.position, GetDashRotation(_targetDashPower)).Play();
            _player.playerRenderer.TrailStart(_player.trailColor, _player.trailCycle, _player.duration);

            CameraManager.Instance.ShakeCamera(_player.movingController.characterMoveDataSO.fre,
                _player.movingController.characterMoveDataSO.amp,
                _player.movingController.characterMoveDataSO.animationTime,
                _player.movingController.characterMoveDataSO.easeT);
            Sequence dashSeq = DOTween.Sequence();
            dashSeq.Append(DOTween.To(() => _curDash, x =>
            {
                _curDash = x;
                _player.movingController.currentHorizontalSpeed = _curDash.x;
                _player.movingController.currentVerticalSpeed = _curDash.y * 0.8f;
            }, _targetDashPower, _player.movingController.characterMoveDataSO.dashTime))
                .SetEase(_player.movingController.characterMoveDataSO.dashEase);

            dashSeq.AppendCallback(() =>
            {
                if (_player.movingController.currentVerticalSpeed < 0.2f)
                {
                    _player.playerAnimation.IdleAnimation();
                }
                _player.LockActionCharacterByModule(false, EPlayerModuleType.Jump);
                _player.GetModule<JumpModule>(EPlayerModuleType.Jump).jumpable = true;
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
