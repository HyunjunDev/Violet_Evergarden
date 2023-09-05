using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Player _player = null;
    private bool _inputLock = false;
    public bool InputLock
    {
        get => _inputLock;
        set
        {
            _inputLock = value;
            if (_inputLock)
            {
                _inputVector = Vector2.zero;
            }
        }
    }

    private Vector2 _inputVector = Vector2.zero;
    public Vector2 InputVector => _inputVector;
    public Vector2 NormalizedInputVector => _inputVector.normalized;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if (_inputLock)
        {
            return;
        }
        GetInput();
    }

    private void GetInput()
    {
        _inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        TagModule tagModule = _player.GetModule<TagModule>(EPlayerModuleType.Tag);
        if (tagModule != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                tagModule.TagWithInput();
            }
        }

        MoveModule moveModule = _player.GetModule<MoveModule>(EPlayerModuleType.Move);
        if (moveModule != null)
        {
            moveModule.Move(_inputVector.x);
        }

        JumpModule jumpModule = _player.GetModule<JumpModule>(EPlayerModuleType.Jump);
        if (jumpModule != null)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                jumpModule.JumpKeyDown();
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                jumpModule.jumpUp = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DashModule dashModule = _player.GetModule<DashModule>(EPlayerModuleType.Dash);
            if (dashModule != null)
            {
                dashModule.DashStart();
            }

            ThrowDaggerModule throwDaggerModule = _player.GetModule<ThrowDaggerModule>(EPlayerModuleType.ThrowDagger);
            if (throwDaggerModule != null)
            {
                throwDaggerModule.ThrowStart();
            }
        }
    }
}
