using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleTon<UIManager>
{
    private static FadeUI _fadeUI;
    public static FadeUI fadeUI => Utility.TryGetInstnace<FadeUI>(_fadeUI);

    private static TagUI _tagUI;
    public static TagUI tagUI => Utility.TryGetInstnace<TagUI>(_tagUI);

    public void FadeStart(float startAlpha, float endAlpha, float animationTime)
    {
        fadeUI.FadeStart(startAlpha, endAlpha, animationTime);
    }

    public void SetTagUI(ECharacterType type)
    {
        tagUI.SetTagUI(type);
    }
}
