using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPassBlock : Obstacle
{
    public override void ReStart()
    {
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log(collision.gameObject.GetComponentInChildren<BoxCollider2D>().gameObject.layer.ToString());
    //    if(1 << collision.gameObject.GetComponentInChildren<BoxCollider2D>().gameObject.layer == LayerMask.GetMask("Dash"))
    //    {
    //        GetComponentInChildren<BoxCollider2D>().isTrigger = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (1 << collision.gameObject.GetComponentInChildren<BoxCollider2D>().gameObject.layer == LayerMask.GetMask("Dash"))
    //    {
    //        GetComponentInChildren<BoxCollider2D>().isTrigger = false;
    //    }
    //}
}
