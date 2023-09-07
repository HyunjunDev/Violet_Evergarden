using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillUIController : MonoBehaviour
{
    private List<FillUI> _fillUis = new List<FillUI>();

    public void SetFillUI(EFillUIType uiType, float cur, float max)
    {
        if(_fillUis.Count == 0)
        {
            InitUI();
        }

        GetFillUI(uiType).FillingUI(cur, max);
    }

    private void InitUI()
    {
        _fillUis.AddRange(transform.GetComponentsInChildren<FillUI>());
    }

    private FillUI GetFillUI(EFillUIType uiType)
    {
        for(int i = 0; i < _fillUis.Count; i++)
        {
            if (_fillUis[i].UiType == uiType)
            {
                return _fillUis[i];
            }
        }
        return null;
    }
}
