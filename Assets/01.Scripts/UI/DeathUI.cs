using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    private TextMeshProUGUI _text = null;
    private int _deathCount = 0;

    private void Start()
    {
        _text = transform.Find("contentText").GetComponent<TextMeshProUGUI>();
        MapManager.Instance.onPlayerDead.AddListener(DeathCountUp);
    }

    private void DeathCountUp()
    {
        _deathCount++;
        _text.SetText(_deathCount.ToString());
    }
}
