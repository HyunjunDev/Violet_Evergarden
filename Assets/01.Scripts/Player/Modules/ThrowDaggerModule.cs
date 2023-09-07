using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.Windows;

public class ThrowDaggerModule : PlayerModule
{
    public override void Exit()
    {
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        UIManager.Instance.SetFillUI(EFillUIType.Special, _curRechargeTime, _maxRechargeTime);
    }

    protected override void InitModule()
    {
        _maxRechargeTime = _player.DashDataSO.dashRechargeTime;

        SetUseable(true);
    }

    protected override void OnGrounded()
    {
        base.OnGrounded();
        SetUseable(true);
    }

    public void ThrowStart()
    {
        Vector2 input = _player.playerInput.NormalizedInputVector;
        if (!(input.sqrMagnitude > 0f))
        {
            input = _player.playerRenderer.currentFlipState == EFlipState.Left
                ? _player.transform.right * -1f : _player.transform.right;
        }

        if (!_useable)
        {
            return;
        }
        SetUseable(false);

        //SpawnDagger
        DaggerPoolable dagger = PoolManager.Instance.Pop(EPoolType.Dagger) as DaggerPoolable;
        Vector2 daggerPosition = new Vector2(_player.transform.position.x + input.x * 0.2f, _player.transform.position.y);
        dagger.transform.SetTransform(daggerPosition, _player.GetLocalScale());
        dagger.SettingDagger(input, _player);
        dagger.SetRotation();

        //DaggerParticle
        GameObject throwDaggerParticle = PoolManager.Instance.Pop(EPoolType.GenThrowDaggerParticle).gameObject;
        Vector2 particlePosition = _player.transform.position + (Vector3)(input * 0.8f);
        throwDaggerParticle.transform.SetTransform(particlePosition, _player.GetLocalScale(), Utility.GetRotationByVector(input, 90));

        //Shake Camera
        CameraManager.Instance.ShakeCamera(_player.ThrowDaggerDataSO.th_shakeCameraData);

        GroundedRecharge();
    }
}
