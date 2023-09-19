using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !EventManager.Instance.IsDeading)
        {
            EventManager.Instance.onPlayerDead.Invoke();
        }
    }
}
