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
    }

    //_player.playerInput.NormalizedInputVector 플레이어 입력한 방향

    public void ThrowStart()
    {
        if (_player.playerInput.NormalizedInputVector == Vector2.zero)
        {
            return;
        }
        DaggerPoolable dagger = PoolManager.Instance.Pop(EPoolType.Dagger) as DaggerPoolable;
        dagger.transform.position = new Vector3(_player.transform.position.x + _player.playerInput.NormalizedInputVector.x * 0.2f, _player.transform.position.y, 0);
        dagger.Dir = _player.playerInput.NormalizedInputVector;
        dagger.Player = _player;
        dagger.SetRotation();

        GameObject throwDaggerParticle = PoolManager.Instance.Pop(EPoolType.GenThrowDaggerParticle).gameObject;
        throwDaggerParticle.transform.position = _player.transform.position + (Vector3)(_player.playerInput.NormalizedInputVector * 0.8f);
        throwDaggerParticle.transform.rotation = Utility.GetDashRotation(_player.playerInput.NormalizedInputVector, 90);


        CameraManager.Instance.ShakeCamera(_player.DashDataSO.frequency,
            _player.DashDataSO.amplitude,
            _player.DashDataSO.shakeTime,
            _player.DashDataSO.easeType);
    }
}
