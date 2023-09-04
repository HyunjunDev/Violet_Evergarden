using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.Windows;

public class ThrowDaggerModule : PlayerModule
{
    public float throwSpeedMultiplier = 1f;

    public override void Exit()
    {
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
    }

    protected override void InitModule()
    {
        _rechargeTime = _player.DashDataSO.dashRechargeTime;
    }

    protected override void OnGrounded()
    {
        base.OnGrounded();
        _useable = true;
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
        _useable = false;

        //SpawnDagger
        DaggerPoolable dagger = PoolManager.Instance.Pop(EPoolType.Dagger) as DaggerPoolable;
        Vector2 daggerPosition = new Vector2(_player.transform.position.x + input.x * 0.2f, _player.transform.position.y);
        dagger.transform.SetTransform(daggerPosition, _player.GetLocalScale());
        dagger.Dir = input;
        dagger.Player = _player;
        dagger.SetRotation();

        //DaggerParticle
        GameObject throwDaggerParticle = PoolManager.Instance.Pop(EPoolType.GenThrowDaggerParticle).gameObject;
        Vector2 particlePosition = _player.transform.position + (Vector3)(input * 0.8f);
        throwDaggerParticle.transform.SetTransform(particlePosition, _player.GetLocalScale(), Utility.GetRotationByVector(input, 90));

        //Shake Camera
        CameraManager.Instance.ShakeCamera(_player.ThrowDaggerDataSO.th_shakeCameraData);

        GroundedRecharge();
    }

    public void Landed(Vector2 hitPosition, Vector2 startPosition, Vector2 endPosition)
    {
        //LandedParticle
        GameObject landedParticle = PoolManager.Instance.Pop(EPoolType.GenDaggerLandedParticle).gameObject;
        landedParticle.transform.SetTransform(hitPosition, _player.GetLocalScale());

        //FadeUI
        UIManager.Instance.FadeStart(0.5f, 0f, 0.5f);

        //Trail
        float t = 0f;
        for (int i = 0; i < 4; i++)
        {
            TrailPoolable trail = PoolManager.Instance.Pop<TrailPoolable>(EPoolType.DashTrail);
            trail.transform.SetTransform(Vector2.Lerp(startPosition, endPosition, t), _player.GetLocalScale());
            trail.StartTrail(_player.playerRenderer.spriteRenderer.sprite, _player.ThrowDaggerDataSO.trailData);
            t += 0.25f;
        }

        //Shake Camera
        CameraManager.Instance.ShakeCamera(_player.ThrowDaggerDataSO.la_shakeCameraData);
    }
}
