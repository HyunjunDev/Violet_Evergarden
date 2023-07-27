using System;
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
        GetInput();
    }

    private void GetInput()
    {
        _player.currentCharacter.GetModule<MoveModule>()?.Move(Input.GetAxisRaw("Horizontal"));

        if (_player.currentCharacter.GetModule<JumpModule>() != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _player.currentCharacter.GetModule<JumpModule>().jumpDown = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _player.currentCharacter.GetModule<JumpModule>().jumpUp = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            _player.TagCharacter();
        }
    }
}
