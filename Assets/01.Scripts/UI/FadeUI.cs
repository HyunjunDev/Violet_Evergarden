using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    private CanvasGroup _fadeCanvasGroup = null;
    private Sequence _fadeSeq = null;

    private void Start()
    {
        _fadeCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeStart(float startAlpha, float endAlpha, float animationTime)
    {
        if(_fadeCanvasGroup == null)
        {
            return;
        }
        _fadeSeq?.Kill();
        _fadeSeq = DOTween.Sequence();
        _fadeCanvasGroup.alpha = startAlpha;
        _fadeCanvasGroup.interactable = true;
        _fadeCanvasGroup.blocksRaycasts = true;
        _fadeSeq.Append(_fadeCanvasGroup.DOFade(endAlpha, animationTime));
        _fadeSeq.AppendCallback(() =>
        {
            _fadeCanvasGroup.interactable = false;
            _fadeCanvasGroup.blocksRaycasts = false;
        });
    }
}
