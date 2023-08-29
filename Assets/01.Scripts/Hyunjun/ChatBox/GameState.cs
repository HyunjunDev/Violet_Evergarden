using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    NONE,
    PLAYER,
    ENEMY
}

[MoonSharpUserData]
public class GameState
{
    private string _selectedObj = "ENEMY";

    public string SelectedObj
    {
        get { return _selectedObj; }
        [MoonSharpHidden]
        set { _selectedObj = value; }
    }

    private Dictionary<ObjectType, GameObject> _objs;

    public Dictionary<ObjectType, GameObject> Objs
    {
        get { return _objs; }
        [MoonSharpHidden]
        set { _objs = value; }
    }

    [MoonSharpHidden]
    public GameState()
    {
        Objs = new Dictionary<ObjectType, GameObject>();
    }
}
