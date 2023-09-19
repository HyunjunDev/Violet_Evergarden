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

    private Room _lastRoom = null;
    private Room _currentRoom = null;

    [SerializeField]
    private float timeDelay = 0.4f;
    private Coroutine _timeDelayCoroutine = null;

    private void Start()
    {
        ChangeRoom(rooms[0]);
    }

    public void ChangeRoom(Room room)
    {
        if(room == _currentRoom)
        {
            return;
        }
        else if (room == _lastRoom)
        {
            GameManager.Instance.Player.ReStart();
            return;
        }
        StartTimeDelay();
        _lastRoom = _currentRoom;
        _currentRoom = room;
        _currentRoom.OnEnterRoom?.Invoke();
        CameraManager.Instance.ChangeRoomCamera(room.cameraAreaCollider);
    }

    public Vector3 GetRespawnPosition()
    {
        if (_currentRoom == null)
        {
            return Vector3.zero;
        }
        return _currentRoom.GetSpawnPoint();
    }

    public void StartTimeDelay()
    {
        if (_timeDelayCoroutine != null)
        {
            StopCoroutine(_timeDelayCoroutine);
        }
        _timeDelayCoroutine = StartCoroutine(TimeDelay(timeDelay));
    }

    IEnumerator TimeDelay(float time)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1.0f;
    }
}
