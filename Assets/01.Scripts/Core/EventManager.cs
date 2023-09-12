using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoSingleTon<EventManager>
{
    public UnityEvent onPlayerDead;

    public UnityEvent onFadeIn;
}
