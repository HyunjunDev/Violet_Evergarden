using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploParticle : MonoBehaviour
{
    private ParticleSystem _particleSystem = null;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(_particleSystem == null)
        {
            return;
        }

        if(_particleSystem.particleCount == 0)
        {
            _particleSystem.Stop();
        }
    }
}
