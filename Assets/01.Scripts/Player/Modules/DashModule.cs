using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashModule : PlayerModule
{
    private Vector2 _targetDashPower = Vector2.zero;
    private Vector2 _curDash = Vector2.zero;

    private Sequence _dashSeq = null;

    public override void Exit()
    {
        _dashSeq?.Kill();
        _targetDashPower = _curDash = Vector2.zero;
        _excuting = false;
    }

    protected override void InitModule()
    {
    }

    public void DashStart()
    {
        Vector2 input = _player.playerInput.NormalizedInputVector;

        if (!(input.sqrMagnitude > 0f))
        {
            return;
        }

        _excuting = true;

        //Reset
        _player.movingController.ResetMovingManager();
        _player.playerAnimation.DashAnimation();

        //Effect
        GameObject.Instantiate(_player.DashParticle, _player.transform.position, Quaternion.identity).Play();
        GameObject.Instantiate(_player.DashTrailParticle, _player.transform.position, GetDashRotation(_targetDashPower)).Play();
        _player.playerRenderer.TrailStart(_player.trailColor, _player.trailCycle, _player.duration);
        CameraManager.Instance.ShakeCamera(_player.movingController.characterMoveDataSO.fre,
            _player.movingController.characterMoveDataSO.amp,
            _player.movingController.characterMoveDataSO.animationTime,
            _player.movingController.characterMoveDataSO.easeT);

        //DashPowerSetting
        _targetDashPower = input * _player.movingController.characterMoveDataSO.dashPower;
        _curDash = Vector2.zero;

        //Jump
        _player.GetModule<JumpModule>(EPlayerModuleType.Jump).JumpEnd();
        _player.LockActionCharacterByModule(true, EPlayerModuleType.Jump);

        //DashSeq
        _dashSeq?.Kill();
        _dashSeq = DOTween.Sequence();
        _dashSeq.Append(DOTween.To(() => _curDash, SettingDashSpeed, _targetDashPower, _player.movingController.characterMoveDataSO.dashTime))
            .SetEase(_player.movingController.characterMoveDataSO.dashEase);
        _dashSeq.AppendCallback(DashEnd);
    }

    private void SettingDashSpeed(Vector2 speed)
    {
        _curDash = speed;
        _player.movingController.currentHorizontalSpeed = _curDash.x;
        _player.movingController.currentVerticalSpeed = _curDash.y * 0.8f;
    }

    private void DashEnd()
    {
        _player.LockActionCharacterByModule(false, EPlayerModuleType.Jump);
        _player.GetModule<JumpModule>(EPlayerModuleType.Jump).JumpRecharge();
        _excuting = false;
    }

    private Quaternion GetDashRotation(Vector2 dir)
    {
        float angle = Vector2.Angle(Vector2.down, dir);
        angle *= Mathf.Sign(dir.x);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        return rot;
    }
}
