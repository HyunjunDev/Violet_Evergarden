using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleTon<UIManager>
{
    private static FadeUI _fadeUI;
    public static FadeUI fadeUI => Utility.TryGetInstnace<FadeUI>(_fadeUI);

    public void FadeStart(float startAlpha, float endAlpha, float animationTime)
    {
        fadeUI.FadeStart(startAlpha, endAlpha, animationTime);
    }
}
