using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDaggerModule : PlayerModule
{

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
        if (_player.playerInput.NormalizedInputVector == Vector2.zero || !_useable)
        {
            return;
        }
        _useable = false;

        //SpawnDagger
        DaggerPoolable dagger = PoolManager.Instance.Pop(EPoolType.Dagger) as DaggerPoolable;
        dagger.transform.localScale = _player.transform.localScale;
        dagger.transform.position = new Vector3(_player.transform.position.x + _player.playerInput.NormalizedInputVector.x * 0.2f, _player.transform.position.y, 0);
        dagger.Dir = _player.playerInput.NormalizedInputVector;
        dagger.Player = _player;
        dagger.SetRotation();

        //DaggerParticle
        GameObject throwDaggerParticle = PoolManager.Instance.Pop(EPoolType.GenThrowDaggerParticle).gameObject;
        throwDaggerParticle.transform.localScale = _player.transform.localScale;
        throwDaggerParticle.transform.position = _player.transform.position + (Vector3)(_player.playerInput.NormalizedInputVector * 0.8f);
        throwDaggerParticle.transform.rotation = Utility.GetRotationByVector(_player.playerInput.NormalizedInputVector, 90);

        //Shake Camera
        CameraManager.Instance.ShakeCamera(_player.ThrowDaggerDataSO.th_shakeCameraData);

        GroundedRecharge();
    }

    public void Landed(Vector2 hitPosition, Vector2 startPosition, Vector2 endPosition)
    {
        //LandedParticle
        GameObject landedParticle = PoolManager.Instance.Pop(EPoolType.GenDaggerLandedParticle).gameObject;
        landedParticle.transform.position = hitPosition;
        landedParticle.transform.localScale = _player.transform.localScale;

        //FadeUI
        UIManager.Instance.FadeStart(0.5f, 0f, 0.5f);

        //Trail
        float t = 0f;
        for (int i = 0; i < 4; i++)
        {
            TrailPoolable trail = PoolManager.Instance.Pop(EPoolType.DashTrail) as TrailPoolable;
            trail.transform.position = Vector2.Lerp(startPosition, endPosition, t);
            trail.transform.localScale = _player.transform.localScale;
            trail.StartTrail(_player.playerRenderer.spriteRenderer.sprite, _player.ThrowDaggerDataSO.trailData);
            t += 0.25f;
        }

        //Shake Camera
        CameraManager.Instance.ShakeCamera(_player.ThrowDaggerDataSO.la_shakeCameraData);
    }
}
