using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPassObstacle : Obstacle
{
    public override void ReStart()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.onPlayerSpawn.AddListener(() => GetComponentInChildren<BoxCollider2D>().isTrigger = false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("¡¢√À");
        if(1 << collision.gameObject.layer == LayerMask.GetMask("Dash"))
        {
            GetComponentInChildren<BoxCollider2D>().isTrigger = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("¡¢√À");
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Dash"))
        {
            GetComponentInChildren<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Dash"))
        {
            GetComponentInChildren<BoxCollider2D>().isTrigger = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            EventManager.Instance.onPlayerDead.Invoke();
        }
    }
}
