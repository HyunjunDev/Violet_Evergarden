using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Player _player = null;

    private Vector2 _inputVector = Vector2.zero;
    public Vector2 InputVector => _inputVector;
    public Vector2 NormalizedInputVector => _inputVector.normalized;

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
        _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetKeyDown(KeyCode.R))
        {
            _player.TagCharacter();
        }

        MoveModule moveModule = _player.GetModule<MoveModule>(EPlayerModuleType.Move);
        if(moveModule != null)
        {
            moveModule.Move(_inputVector.x);
        }

        JumpModule jumpModule = _player.GetModule<JumpModule>(EPlayerModuleType.Jump);
        if (jumpModule != null)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                jumpModule.jumpDown = true;
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                jumpModule.jumpUp = true;
            }
        }

        DashModule dashModule = _player.GetModule<DashModule>(EPlayerModuleType.Dash);
        if (dashModule != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                dashModule.DashStart();
            }
        }
    }
}
