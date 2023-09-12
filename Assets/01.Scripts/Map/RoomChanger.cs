using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChanger : MonoBehaviour
{
    [SerializeField]
    private Room _room = null;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MapChanger"))
        {
            _room.ChangeRoom();
            _room.StartTimeDelay();
        }
    }
}
