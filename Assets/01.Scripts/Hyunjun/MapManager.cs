using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapManager : MonoSingleTon<MapManager>
{
    public UnityEvent onPlayerDead;

    [SerializeField]
    private List<Room> rooms = new List<Room>();

    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReSpawn()
    {
        foreach(var room in rooms)
        {
            if(room.transform.GetChild(0).gameObject.activeSelf)
            {
                //GameManager.Instance.Player.transform.position = room.spawnPosition;
            }
        }
    }

    public Vector3 GetRespawnPosition()
    {
        Vector3 result = Vector3.zero;
        foreach (var room in rooms)
        {
            if (room.transform.GetChild(0).gameObject.activeSelf)
            {
                result = room.spawnPosition;
            }
        }
        return result;
    }
}
