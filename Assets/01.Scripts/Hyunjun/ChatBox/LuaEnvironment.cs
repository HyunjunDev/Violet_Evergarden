using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class LuaEnvironment : MonoBehaviour
{
    [SerializeField]
    private string loadFile;

    private Script enviro;
    private Stack<MoonSharp.Interpreter.Coroutine> corStack;

    private GameState luaGameState;

    public GameState LuaGameState
    {
        get
        {
            return luaGameState;
        }
    }

    private void Awake()
    {
        luaGameState = new GameState();

        GameObject[] allObjs = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjs)
        {
            if (System.Enum.IsDefined(typeof(ObjectType), obj.name.ToUpperInvariant()))
            {
                Debug.Log(obj.name.ToUpperInvariant());
                ObjectType objType = (ObjectType)Enum.Parse(typeof(ObjectType), obj.name.ToUpperInvariant());
                luaGameState.Objs.Add(objType, obj);
            }
        }
    }

    private IEnumerator Start()
    {
        Script.DefaultOptions.DebugPrint = (s) => Debug.Log(s);
        UserData.RegisterAssembly();

        corStack = new Stack<MoonSharp.Interpreter.Coroutine>();
        enviro = new Script(CoreModules.Preset_SoftSandbox);
        enviro.Globals["BiggerPlayer"] = (Action<ObjectType, Dictionary<ObjectType, GameObject>>)LuaCommands.BiggerObject;
        enviro.Globals["State"] = UserData.Create(luaGameState);

        yield return 1;

        LoadFile(loadFile);
        AdvanceScript();
    }

    private void LoadFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        DynValue ret = DynValue.Nil;

        using(BufferedStream stream = new BufferedStream(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
        {
            ret = enviro.DoStream(stream);
        }

        if(ret.Type == DataType.Function)
        {
            corStack.Push(enviro.CreateCoroutine(ret).Coroutine);
        }
    }

    public void AdvanceScript()
    {
        if(corStack.Count>0)
        {
            MoonSharp.Interpreter.Coroutine active = corStack.Peek();
            DynValue ret = active.Resume();
            if(active.State==CoroutineState.Dead)
            {
                corStack.Pop();
                Debug.Log("Dialogue complete");
            }
            if (ret.Type == DataType.Function)
            {
                corStack.Push(enviro.CreateCoroutine(ret).Coroutine);
            }
        }
        else
        {
            Debug.Log("No active dialogue");
        }
    }
}
