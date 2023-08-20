using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : PoolableObject
{
    private ParticleSystem _particleSystem = null;

    public override void PopInit()
    {
        _particleSystem.Play();
    }

    public override void PushInit()
    {
    }

    public override void StartInit()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(_particleSystem.particleCount == 0)
        {
            PoolManager.Instance.Push(this);
        }
    }
}
