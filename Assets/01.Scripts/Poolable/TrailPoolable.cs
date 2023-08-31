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

    public void StartTrail(Sprite sprite, TrailDataSO data)
    {
        _spriteRenderer.color = data.trailColor;
        _spriteRenderer.sprite = sprite;
        _trailSeq?.Kill();
        _trailSeq = DOTween.Sequence();
        _trailSeq.AppendInterval(data.duration);
        _trailSeq.Append(_spriteRenderer.DOFade(0f, data.fadeOutTime));
        _trailSeq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }
}
