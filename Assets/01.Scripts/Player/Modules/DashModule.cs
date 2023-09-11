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
        DashEnd();
        _targetDashPower = _curDash = Vector2.zero;
        _excuting = false;
        _player.playerRenderer.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    protected override void InitModule()
    {
        _maxRechargeTime = _player.DashDataSO.dashRechargeTime;

        SetUseable(true);
    }

    protected override void OnGrounded()
    {
        SetUseable(true);
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        UIManager.Instance.SetFillUI(EFillUIType.Special, _curRechargeTime, _maxRechargeTime);
    }

    public void DashStart()
    {
        Vector2 input = _player.playerInput.NormalizedInputVector;
        if (!(input.sqrMagnitude > 0f))
        {
            input = _player.playerRenderer.currentFlipState == EFlipState.Left
                ? _player.transform.right * -1f : _player.transform.right;
        }

        if (!_useable || _locked)
        {
            return;
        }

        _player.playerRenderer.gameObject.layer = LayerMask.NameToLayer("Dash");
        SetUseable(false);
        _excuting = true;

        //Reset
        _player.movingController.ResetMovingManager();
        _player.playerAnimation.DashAnimation();
        _player.playerAnimation.SetDashParameter(true);

        //DashPowerSetting
        _targetDashPower = input * _player.DashDataSO.dashPower;
        _curDash = Vector2.zero;

        //Effect
        GameObject dashTrailParticle = PoolManager.Instance.Pop(EPoolType.HanaDashParticle).gameObject;
        dashTrailParticle.transform.SetTransform(_player.GetMiddlePosition(), _player.GetLocalScale());
        dashTrailParticle.transform.rotation = Utility.GetRotationByVector(_targetDashPower, 90);
        GameObject dashFlowerParticle = PoolManager.Instance.Pop(EPoolType.HanaFlowerParticle).gameObject;
        dashFlowerParticle.transform.SetTransform(_player.GetMiddlePosition(), _player.GetLocalScale());
        _player.playerRenderer.StartTrail(_player.DashDataSO.trailCycle, _player.DashDataSO.duration, _player.DashDataSO.trailData);
        CameraManager.Instance.ShakeCamera(_player.DashDataSO.shakeCameraData);

        //Jump
        _player.GetModule<JumpModule>(EPlayerModuleType.Jump).JumpEnd();
        _player.LockModules(true, EPlayerModuleType.Jump);

        //DashSeq
        _dashSeq?.Kill();
        _dashSeq = DOTween.Sequence();
        _dashSeq.Append(DOTween.To(() => _curDash, SettingDashSpeed, _targetDashPower * _player.MultiplierDataSO.dashMultiplier, _player.DashDataSO.dashTime))
            .SetEase(_player.DashDataSO.dashEase);
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
        _player.playerAnimation.SetDashParameter(false);
        _player.LockModules(false, EPlayerModuleType.Jump);
        _excuting = false;
        GroundedRecharge();
        _player.playerRenderer.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
