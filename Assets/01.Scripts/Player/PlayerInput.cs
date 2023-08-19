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
        _player.GetModule<MoveModule>(ECharacterModuleType.Move)?.Move(Input.GetAxisRaw("Horizontal"));

        if (_player.GetModule<JumpModule>(ECharacterModuleType.Jump) != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _player.GetModule<JumpModule>(ECharacterModuleType.Jump).jumpDown = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _player.GetModule<JumpModule>(ECharacterModuleType.Jump).jumpUp = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(_player.GetModule<DashModule>(ECharacterModuleType.Dash) != null)
            {
                _player.GetModule<DashModule>(ECharacterModuleType.Dash).DashStart(
                    new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            }
        }
    }
}
