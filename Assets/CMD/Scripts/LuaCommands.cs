using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;
using UnityEngine.UIElements;

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

    public void SetDaggerSpeed(string _scale)
    {
        float scale = 1f;
        if (float.TryParse(_scale, out scale))
            LuaCommands.Instance.SetDaggerSpeed(scale);
    }

    public void SetColorPlayer(string _colorValue)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(_colorValue, out color))
            LuaCommands.Instance.SetColorPlayer(color);
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
        player.MultiplierDataSO.dashMultiplier = scale;
    }

    public void SetSpeedPlayer(float scale)
    {
        Player player = gameinfo.Objs[ObjectType.PLAYER].GetComponent<Player>();
        player.GetModule<MoveModule>(EPlayerModuleType.Move).ResetHorizontalSpeed();
        player.MultiplierDataSO.speedMultiplier = scale;
    }

    public void SetGravityPlayer(float scale)
    {
        Player player = gameinfo.Objs[ObjectType.PLAYER].GetComponent<Player>();
        player.MultiplierDataSO.gravityMultiplier = scale;
    }

    public void SetDaggerSpeed(float scale)
    {
        Player player = gameinfo.Objs[ObjectType.PLAYER].GetComponent<Player>();
        player.MultiplierDataSO.throwSpeedMultiplier = scale;
    }

    public void SetColorPlayer(Color color)
    {
        Player player = gameinfo.Objs[ObjectType.PLAYER].GetComponent<Player>();
        player.GetComponentInChildren<SpriteRenderer>().color = color;
        player.DashDataSO.trailData.trailColor = color;
    }

    public void Help()
    {
        DebugController.Instance.ShowHelp = true;
    }
}
