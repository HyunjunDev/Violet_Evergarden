using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    private EFlipState _currentFlipState = EFlipState.None;
    public EFlipState currentFlipState => _currentFlipState;

    private SpriteRenderer _spriteRenderer = null;
    private Coroutine _rendererTrailCoroutine = null;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void MoveInputFlip(float moveX)
    {
        if(moveX != 0f)
        {
            Flip(moveX < 0f ? EFlipState.Left : EFlipState.Right);
        }
    }

    /// <summary>
    /// flipState 방향으로 회전시킵니다.
    /// </summary>
    /// <param name="flipState"></param>
    public void Flip(EFlipState flipState)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        switch (flipState)
        {
            case EFlipState.None:
                return;
            case EFlipState.Left:
                localScale.x = -localScale.x;
                break;
            case EFlipState.Right:
                break;
            default:
                return;
        }
        _currentFlipState = flipState;
        transform.localScale = localScale;
    }

    public void StartTrail(Color trailColor, float trailCycle, float duration)
    {
        if (_rendererTrailCoroutine != null)
            StopCoroutine(_rendererTrailCoroutine);
        _rendererTrailCoroutine = StartCoroutine(RendererTrailCoroutine(trailColor, trailCycle, duration));
    }

    private IEnumerator RendererTrailCoroutine(Color trailColor, float trailCycle, float duration)
    {
        float time = 0f;
        while (time <= duration)
        {
            TrailPoolable trailPoolable = PoolManager.Instance.Pop(EPoolType.DashTrail) as TrailPoolable;
            trailPoolable.StartTrail(_spriteRenderer.sprite, trailColor, trailCycle, duration);
            trailPoolable.transform.position = transform.position;
            trailPoolable.transform.localScale = transform.localScale;
            trailPoolable.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(trailCycle);
            time += trailCycle;
        }
    }
}
