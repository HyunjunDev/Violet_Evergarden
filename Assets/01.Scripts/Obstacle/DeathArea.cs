using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            EventManager.Instance.onPlayerDead.Invoke();
        }
    }
}
