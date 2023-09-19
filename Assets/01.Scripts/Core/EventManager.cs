using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoSingleTon<EventManager>
{
    public UnityEvent onPlayerDead;

    public UnityEvent onFadeIn;

    public UnityEvent onPlayerSpawn;

    private bool isDeading;
    public bool IsDeading
    { get { return isDeading; } }

    private void Start()
    {
        onPlayerDead.AddListener(() => isDeading = true);
        onPlayerSpawn.AddListener(() => isDeading = false);
    }
}
