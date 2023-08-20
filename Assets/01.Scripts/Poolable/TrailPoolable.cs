using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPoolable : PoolableObject
{
    private SpriteRenderer _spriteRenderer = null;
    private Sequence _trailSeq = null;

    public override void PopInit()
    {
    }

    public override void PushInit()
    {
        _trailSeq?.Kill();
        _spriteRenderer.sprite = null;
        _spriteRenderer.color = Color.white;
    }

    public override void StartInit()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartTrail(Sprite sprite, Color trailColor, float trailCycle, float duration)
    {
        _spriteRenderer.color = trailColor;
        _spriteRenderer.sprite = sprite;
        _trailSeq?.Kill();
        _trailSeq = DOTween.Sequence();
        _trailSeq.Append(_spriteRenderer.DOFade(0f, 0.5f));
        _trailSeq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }
}
