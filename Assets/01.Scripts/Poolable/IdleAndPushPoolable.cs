using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAndPushPoolable : PoolableObject
{
    [SerializeField]
    private float _idleTime = 3f;
    private Coroutine _idleAndPushCoroutine = null;

    public override void PopInit()
    {
        if (_idleAndPushCoroutine != null)
        {
            StopCoroutine(_idleAndPushCoroutine);
        }
        _idleAndPushCoroutine = StartCoroutine(IdleAndPushCoroutine());
    }

    public override void PushInit()
    {
        if (_idleAndPushCoroutine != null)
        {
            StopCoroutine(_idleAndPushCoroutine);
        }
    }

    public override void StartInit()
    {
    }

    private IEnumerator IdleAndPushCoroutine()
    {
        yield return new WaitForSeconds(_idleTime);
        PoolManager.Instance.Push(this);
    }
}
