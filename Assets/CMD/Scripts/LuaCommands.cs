using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;

public enum ObjectType
{
    NONE,
    PLAYER
}

public class GameInfo
{
    private Dictionary<ObjectType, GameObject> _objs;

    public Dictionary<ObjectType, GameObject> Objs
    {
        get { return _objs; }
        set { _objs = value; }
    }

    public GameInfo()
    {
        Objs = new Dictionary<ObjectType, GameObject>();
    }
}

public class UnityAPI
{
    public void SetScaleObj(string _name, string _scale)
    {
        ObjectType objType = ObjectType.NONE;
        float scale = 1f;
        if (Enum.TryParse<ObjectType>(_name.ToUpperInvariant(), out objType) && float.TryParse(_scale, out scale))
        {
            LuaCommands.Instance.SetScaleObj(objType, scale);
        }
    }

    public void SetDashPlayer(string _scale)
    {
        float scale = 1f;
        if (float.TryParse(_scale, out scale))
            LuaCommands.Instance.SetDashPlayer(scale);
    }

    public void SetSpeedPlayer(string _scale)
    {
        float scale = 1f;
        if (float.TryParse(_scale, out scale))
            LuaCommands.Instance.SetSpeedPlayer(scale);
    }

    public void SetGravityPlayer(string _scale)
    {
        float scale = 1f;
        if (float.TryParse(_scale, out scale))
            LuaCommands.Instance.SetGravityPlayer(scale);
    }

    public void Help()
    {
        LuaCommands.Instance.Help();
    }
}

public class LuaCommands : MonoBehaviour
{
    private static LuaCommands instance;

    public static LuaCommands Instance => instance;

    private GameInfo gameinfo;
    public GameInfo GInfo => gameinfo;

    private void Awake()
    {
        instance = this;
        gameinfo = new GameInfo();

        GameObject[] allObjs = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjs)
        {
            if (System.Enum.IsDefined(typeof(ObjectType), obj.name.ToUpperInvariant()))
            {
                ObjectType objType = (ObjectType)Enum.Parse(typeof(ObjectType), obj.name.ToUpperInvariant());
                gameinfo.Objs.Add(objType, obj);
            }
        }
    }

    public void SetScaleObj(ObjectType objType, float scale)
    {
        gameinfo.Objs[objType].transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetDashPlayer(float scale)
    {
        Player player = gameinfo.Objs[ObjectType.PLAYER].GetComponent<Player>();
        player.GetModule<DashModule>(EPlayerModuleType.Dash).dashMultiplier = scale;
    }

    public void SetSpeedPlayer(float scale)
    {
        Player player = gameinfo.Objs[ObjectType.PLAYER].GetComponent<Player>();
        player.GetModule<MoveModule>(EPlayerModuleType.Move).moveMultiplier = scale;
    }

    public void SetGravityPlayer(float scale)
    {
        Player player = gameinfo.Objs[ObjectType.PLAYER].GetComponent<Player>();
        player.GetModule<GravityModule>(EPlayerModuleType.Gravity).gravityMultiplier = scale;
    }

    public void Help()
    {
        DebugController.Instance.ShowHelp = true;
    }
}
