using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleTon<UIManager>
{
    private static FadeUI _fadeUI;
    public static FadeUI fadeUI => Utility.TryGetInstnace<FadeUI>(_fadeUI);

    private static TagUI _tagUI;
    public static TagUI tagUI => Utility.TryGetInstnace<TagUI>(_tagUI);

    private static FillUIController _fillUIController;
    public static FillUIController fillUIController => Utility.TryGetInstnace<FillUIController>(_fillUIController);

    public void FadeStart(float startAlpha, float endAlpha, float animationTime)
    {
        fadeUI.FadeStart(startAlpha, endAlpha, animationTime);
    }

    public void SetTagUI(ECharacterType type)
    {
        tagUI.SetTagUI(type);
    }

    public void SetFillUI(EFillUIType uiType, float cur, float max)
    {
        fillUIController.SetFillUI(uiType, cur, max);
    }
}
