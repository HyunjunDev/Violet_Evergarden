using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class LuaCommands : MonoBehaviour
{
    private static LuaCommands instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    public void BiggerObject(ObjectType name, GameState gameState)
    {
        GameObject val = null;
        if (gameState.Objs.TryGetValue(name, out val))
        {
            val.transform.localScale *= 2f;
        }
    }

    public void DebugText(string text)
    {
        Debug.Log(text);
    }

    public void PrintFromLua(string message)
    {
        Debug.Log("Lua says: " + message);
    }
}
