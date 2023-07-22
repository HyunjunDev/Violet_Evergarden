using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Player _player = null;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        _player.CurrentCharacter.GetModule<MoveModule>()?.Move(Input.GetAxisRaw("Horizontal"));
    }
}
