using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleTon<GameManager>
{
    private Player _player;
    public Player Player => _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }
}
