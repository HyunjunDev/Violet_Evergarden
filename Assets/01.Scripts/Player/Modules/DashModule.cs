using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DashModule : PlayerModule
{
    private Vector2 _targetDashPower = Vector2.zero;
    private Vector2 _curDash = Vector2.zero;

    private bool _dashable = true;
    private Coroutine _groundDashRechargeCoroutine = null;

    private Sequence _dashSeq = null;

    public override void Exit()
    {
        _dashSeq?.Kill();
        _targetDashPower = _curDash = Vector2.zero;
        _excuting = false;
    }

    protected override void InitModule()
    {
        _player.playerCollider.onGrounded += OnGrounded;
    }

    private void OnGrounded()
    {
        _dashable = true;
    }

    public void DashStart()
    {
        Vector2 input = _player.playerInput.NormalizedInputVector;

        if (!(input.sqrMagnitude > 0f) || !_dashable)
        {
            return;
        }

        _dashable = false; 
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
        dashTrailParticle.transform.position = _player.transform.position;
        dashTrailParticle.transform.rotation = GetDashRotation(_targetDashPower);
        GameObject dashFlowerParticle = PoolManager.Instance.Pop(EPoolType.HanaFlowerParticle).gameObject;
        dashFlowerParticle.transform.position = _player.transform.position;
        _player.playerRenderer.StartTrail(_player.DashDataSO.trailColor, _player.DashDataSO.trailCycle, _player.DashDataSO.duration);
        CameraManager.Instance.ShakeCamera(_player.DashDataSO.frequency,
            _player.DashDataSO.amplitude,
            _player.DashDataSO.shakeTime,
            _player.DashDataSO.easeType);

        //Jump
        _player.GetModule<JumpModule>(EPlayerModuleType.Jump).JumpEnd();
        _player.LockModules(true, EPlayerModuleType.Jump);

        //DashSeq
        _dashSeq?.Kill();
        _dashSeq = DOTween.Sequence();
        _dashSeq.Append(DOTween.To(() => _curDash, SettingDashSpeed, _targetDashPower, _player.DashDataSO.dashTime))
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
        DashRecharge();
    }

    private void DashRecharge()
    {
        if (_groundDashRechargeCoroutine != null)
            StopCoroutine(_groundDashRechargeCoroutine);
        _groundDashRechargeCoroutine = StartCoroutine(GroundDashRechargeCoroutine());
    }

    private IEnumerator GroundDashRechargeCoroutine()
    {
        yield return new WaitForSeconds(_player.DashDataSO.dashRechargeTime);
        if(_player.playerCollider.GetCollision(EBoundType.Down, false))
        {
            _dashable = true;
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
