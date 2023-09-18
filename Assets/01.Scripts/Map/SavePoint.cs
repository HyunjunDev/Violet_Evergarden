using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private Room room;

    private bool isFirst = true;

    private void Start()
    {
        room = transform.parent.GetComponent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")&&isFirst)
        {
            isFirst = false;
            room.SpawnCount++;
        }
    }
}
