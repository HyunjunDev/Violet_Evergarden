using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillUI : MonoBehaviour
{
    [SerializeField]
    private EFillUIType _uiType = EFillUIType.None;
    public EFillUIType UiType => _uiType;

    private Image _fillImage = null;

    public void FillingUI(float cur, float max)
    {
        if (_fillImage == null)
        {
            InitUI();
        }
        _fillImage.fillAmount = cur / max;
    }

    private void InitUI()
    {
        _fillImage = transform.Find("FillImage").GetComponent<Image>();
    }
}
