using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IReStartable
{
    [SerializeField]
    protected LayerMask collisionLayer;

    public abstract void ReStart();
}
