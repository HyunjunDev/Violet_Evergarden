using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [SerializeField]
    private string loadFile;

    private Script luaScript;

    bool showConsole;

    string input;

    public static DebugCommand DEBUG_TEXT;
    public static DebugCommand SET_SCALE_OBJ;


    public List<object> commandList;


    public void OnToggleDebug()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if(showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void Awake()
    {
        DEBUG_TEXT = new DebugCommand("debug_text", "Debug Text", FuncType.DEBUG_TEXT);

        SET_SCALE_OBJ = new DebugCommand("set_scale", "Scale Obj", FuncType.SET_SCALE_OBJ);

        commandList = new List<object>
        {
            DEBUG_TEXT,
            SET_SCALE_OBJ,
        };
    }

    private void Start()
    {
        UserData.RegisterAssembly();
        UserData.RegisterType<UnityAPI>();

        luaScript = new Script();
        luaScript.Globals["Unity"] = new UnityAPI();

        LoadLuaScript(loadFile);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            OnToggleDebug();
        }
    }


    private void OnGUI()
    {
        if(!showConsole)
        {
            input = "";
            return;
        }

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 120), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.textField);
        gUIStyle.fontSize = UnityEngine.Screen.width / 25;
        if (GUI.GetNameOfFocusedControl() == "")
        {
            if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
            {
                OnReturn();
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.BackQuote)
            {
                OnToggleDebug();
            }
        }
        GUI.SetNextControlName("");
        input = GUI.TextField(new Rect(10f, y + 10f, Screen.width - 20f, 90f), input, gUIStyle);
        GUI.FocusControl("");
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' '); 
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommand command = commandList[i] as DebugCommand;

            if(input.Contains(command.commandId))
            {
                switch(command.funcType)
                {
                    case FuncType.DEBUG_TEXT:
                        if (properties.Length == 2)
                            RunLuaFunction(command.funcType.ToString(), properties[1]);
                        break;
                    case FuncType.SET_SCALE_OBJ:
                        if (properties.Length == 3)
                            RunLuaFunction(command.funcType.ToString(), properties[1], properties[2]);
                        break;
                }
            }
        }
    }

    private void LoadLuaScript(string scriptFileName)
    {
        string scriptFilePath = Path.Combine(Application.streamingAssetsPath, scriptFileName);

        if (File.Exists(scriptFilePath))
        {
            string scriptCode = File.ReadAllText(scriptFilePath);
            luaScript.DoString(scriptCode);
        }
        else
        {
            Debug.LogError("Lua script file not found: " + scriptFilePath);
        }
    }

    private void RunLuaFunction(string functionName, params object[] parameters)
    {
        DynValue luaFunction = luaScript.Globals.Get(functionName);
        if (luaFunction != null && luaFunction.Type == DataType.Function)
        {
            luaScript.Call(luaFunction, parameters);
        }
        else
        {
            Debug.LogError("Lua function not found: " + functionName);
        }
    }
}
