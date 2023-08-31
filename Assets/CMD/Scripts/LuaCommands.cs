using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;

public enum ObjectType
{
    NONE,
    PLAYER,
    ENEMY,
    MINIMAP
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
    public void DebugText(string str)
    {
        if (str != null)
            LuaCommands.Instance.DebugText(str);
    }

    public void SetScaleObj(string _name, string _scale)
    {
        ObjectType objType = ObjectType.NONE;
        float scale = 1f;
        if (Enum.TryParse<ObjectType>(_name.ToUpperInvariant(), out objType) && float.TryParse(_scale, out scale))
        {
            LuaCommands.Instance.SetScaleObj(objType, scale);
        }
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

    public void DebugText(string str)
    {
        Debug.Log(str);
    }

    public void SetScaleObj(ObjectType objType, float scale)
    {
        gameinfo.Objs[objType].transform.localScale = new Vector3(scale, scale, scale);
    }

    public void Help()
    {
        DebugController.Instance.ShowHelp = true;
    }
}
