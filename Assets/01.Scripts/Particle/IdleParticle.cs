using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleParticle : MonoBehaviour
{
    [SerializeField]
    private float _idleTime = 3f;
    private ParticleSystem _particleSystem = null;

    private IEnumerator Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(_idleTime);
        _particleSystem.Stop();
    }
}
