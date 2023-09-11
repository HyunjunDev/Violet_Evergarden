using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapManager : MonoSingleTon<MapManager>
{
    public UnityEvent onPlayerDead;

    [SerializeField]
    private List<Room> rooms = new List<Room>();

    private Room _currentRoom = null;

    private void Start()
    {
        ChangeRoom(rooms[0]);
    }

    public void ChangeRoom(Room room)
    {
        _currentRoom = room;
        CameraManager.Instance.ChangeRoomCamera(room.cameraAreaCollider);
    }

    public Vector3 GetRespawnPosition()
    {
        if(_currentRoom == null)
        {
            return Vector3.zero;
        }
        return _currentRoom.GetSpawnPoint();
    }
}
