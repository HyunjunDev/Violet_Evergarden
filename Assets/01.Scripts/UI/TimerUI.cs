using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private float _currentTime;
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = transform.Find("contentText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        SetTimer();
    }

    private void SetTimer()
    {
        _currentTime += Time.deltaTime;
        TimeSpan timespan = TimeSpan.FromSeconds(_currentTime);
        string timer = string.Format("{0:00}:{1:00}",
            timespan.Minutes, timespan.Seconds);
        _text.SetText(timer);
    }
}
