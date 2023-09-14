using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerPassObstacle : MonoBehaviour
{

    private void Start()
    {
        EventManager.Instance.onPlayerSpawn.AddListener(() => GetComponentInChildren<BoxCollider2D>().isTrigger = false);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Dagger"))
        {
            Debug.Log("Á¢ÃË");
            if(collision.gameObject.GetComponentInParent<DaggerPoolable>().ArrivePos!=Vector2.zero)
            {
                Debug.Log(collision.gameObject.GetComponentInParent<DaggerPoolable>().ArrivePos);
                if (collision.gameObject.GetComponentInParent<DaggerPoolable>().ArrivePos.y >= transform.position.y - transform.localScale.y * 0.5f - collision.gameObject.GetComponentInParent<DaggerPoolable>().Player.transform.localScale.y*0.5f && collision.gameObject.GetComponentInParent<DaggerPoolable>().ArrivePos.y <= transform.position.y + transform.localScale.y * 0.5f + collision.gameObject.GetComponentInParent<DaggerPoolable>().Player.transform.localScale.y * 0.5f)
                {
                    if (collision.gameObject.GetComponentInParent<DaggerPoolable>().ArrivePos.x >= transform.position.x - transform.localScale.x * 0.5f && collision.gameObject.GetComponentInParent<DaggerPoolable>().ArrivePos.x <= transform.position.x + transform.localScale.x * 0.5f)
                    {
                        GetComponentInChildren<BoxCollider2D>().isTrigger = true;
                        EventManager.Instance.onPlayerDead.Invoke();
                    }
                }
            }
        }
    }
}
