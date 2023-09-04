using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FuncType
{
    NONE,
    SET_SCALE_OBJ,
    SET_DASH,
    SET_SPEED,
    SET_JUMP,
    SET_GRAVITY,
    SET_DAGGER_SPEED,
    SET_COLOR,
    HELP
}

public class DebugCommand
{
    private string _commandId;
    private string _commandDescription;
    private FuncType _funcType;

    public string commandId { get { return _commandId; } }
    public string CommandDescription { get { return _commandDescription; } }
    public FuncType funcType { get { return _funcType; } }

    public DebugCommand(string id, string description, FuncType funcType)
    {
        _commandId = id;
        _commandDescription = description;
        _funcType = funcType;
    }
}