using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using System;

public class LuaEnvironment : MonoBehaviour
{
    [SerializeField]
    private string loadFile;

    private Script luaScript;

    private GameState luaGameState;

    public GameState LuaGameState
    {
        get
        {
            return luaGameState;
        }
    }

    private void Start()
    {
        luaScript = new Script();

        // Register Unity function for Lua script
        UserData.RegisterAssembly();
        UserData.RegisterType<UnityAPI>();
        luaScript.Globals["unity"] = new UnityAPI();
        luaScript.Globals["State"] = UserData.Create(luaGameState);

        LoadAndExecuteLuaScript(loadFile);
    }

    private void LoadAndExecuteLuaScript(string scriptFileName)
    {
        string scriptFilePath = Path.Combine(Application.streamingAssetsPath, scriptFileName);
        string scriptCode = File.ReadAllText(scriptFilePath);

        luaScript.DoString(scriptCode);

        DynValue mainFunction = luaScript.Globals.Get("main");
        if (mainFunction != null && mainFunction.Type == DataType.Function)
        {
            DynValue result = luaScript.Call(mainFunction);
            if (result.Type == DataType.String)
            {
                string output = result.String;
                Debug.Log(output);
            }
        }
        else
        {
            Debug.LogWarning("No 'main' function found in Lua script.");
        }
    }

    public class UnityAPI
    {
        LuaCommands luaCmd;
        LuaEnvironment luaEnv;

        public UnityAPI()
        {
            luaCmd = FindObjectOfType<LuaCommands>();
            luaEnv = FindObjectOfType<LuaEnvironment>();

            luaEnv.luaGameState = new GameState();

            GameObject[] allObjs = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjs)
            {
                if (System.Enum.IsDefined(typeof(ObjectType), obj.name.ToUpperInvariant()))
                {
                    ObjectType objType = (ObjectType)Enum.Parse(typeof(ObjectType), obj.name.ToUpperInvariant());
                    luaEnv.luaGameState.Objs.Add(objType, obj);
                }
            }
        }

        public void CallUnityFunction(string functionName, object argument)
        {
            luaCmd.StartCoroutine(functionName, argument);
        }

        public void BiggerObject(string objectTypeStr)
        {
            ObjectType objectType;
            if (System.Enum.TryParse<ObjectType>(objectTypeStr, out objectType))
            {
                luaCmd.BiggerObject(objectType, luaEnv.luaGameState);
            }
            else
            {
                Debug.LogWarning("Invalid ObjectType: " + objectTypeStr);
            }
        }
    }
}
