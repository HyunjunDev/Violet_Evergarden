using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugController : MonoBehaviour
{
    [SerializeField]
    private string loadFile;

    private Script luaScript;

    private static DebugController instance;
    public static DebugController Instance => instance;

    bool showConsole;
    bool showHelp;
    public bool ShowHelp
    {
        get { return showHelp; }
        set { showHelp = value; }
    }

    string input;
    TextEditor editor;
     

    Vector2 scroll;

    public static DebugCommand SET_SCALE_OBJ;
    public static DebugCommand HELP;
    public static DebugCommand SET_DASH;
    public static DebugCommand SET_SPEED;
    public static DebugCommand SET_GRAVITY;
    public static DebugCommand SET_DAGGER_SPEED;
    public static DebugCommand SET_COLOR;

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
        instance = this;

        SET_SCALE_OBJ = new DebugCommand("set_scale", "Adjust the scale value of the desired object", FuncType.SET_SCALE_OBJ);

        HELP = new DebugCommand("help", "Shows a list of commands", FuncType.HELP);

        SET_DASH = new DebugCommand("set_dash", "���翱", FuncType.SET_DASH);

        SET_SPEED = new DebugCommand("set_speed", "���翱2", FuncType.SET_SPEED);

        SET_GRAVITY = new DebugCommand("set_gravity", "���翱2", FuncType.SET_GRAVITY);

        SET_DAGGER_SPEED = new DebugCommand("set_dagger_speed", "���翱3", FuncType.SET_DAGGER_SPEED);

        SET_COLOR = new DebugCommand("set_color", "���翱3", FuncType.SET_COLOR);

        commandList = new List<object>
        {
            SET_SCALE_OBJ,
            SET_DASH,
            SET_SPEED,
            SET_GRAVITY,
            SET_DAGGER_SPEED,
            SET_COLOR,
            HELP
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

        GUIStyle gUIStyle = new GUIStyle(GUI.skin.textField);
        gUIStyle.fontSize = UnityEngine.Screen.width / 55;


        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 150),"");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 45 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 150), scroll, viewport);

            for(int i=0;i<commandList.Count; i++)
            {
                DebugCommand command = commandList[i] as DebugCommand;

                string label = $"{command.commandId} - {command.CommandDescription}";

                Rect labelRect = new Rect(5, 45 * i, viewport.width, 45);

                GUI.Label(labelRect, label, gUIStyle);
            }
            GUI.EndScrollView();


            y += 150;
        }

        GUI.Box(new Rect(0, y, Screen.width, 60), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
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
        input = GUI.TextField(new Rect(10f, y + 10f, Screen.width - 20f, 45f), input, gUIStyle);
        GUI.FocusControl("");

        if (showHelp)
        {
            if (input != "")
            {
                showHelp = false;
                editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

                Debug.Log(editor + " " + editor.text);

                editor.SelectNone();
                editor.MoveTextEnd();
            }
        }


        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommand command = commandList[i] as DebugCommand;

            if (command.commandId.ToString().Contains(input))
            {
                Debug.Log(command.commandId);
            }
        }
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
                    case FuncType.SET_SCALE_OBJ:
                        if (properties.Length == 3)
                            RunLuaFunction(command.funcType.ToString(), properties[1], properties[2]);
                        break;
                    case FuncType.SET_DASH:
                        if (properties.Length == 2)
                            RunLuaFunction(command.funcType.ToString(), properties[1]);
                        break;
                    case FuncType.SET_SPEED:
                        if (properties.Length == 2)
                            RunLuaFunction(command.funcType.ToString(), properties[1]);
                        break;
                    case FuncType.SET_GRAVITY:
                        if (properties.Length == 2)
                            RunLuaFunction(command.funcType.ToString(), properties[1]);
                        break;
                    case FuncType.SET_DAGGER_SPEED:
                        if (properties.Length == 2)
                            RunLuaFunction(command.funcType.ToString(), properties[1]);
                        break;
                    case FuncType.SET_COLOR:
                        if (properties.Length == 2)
                            RunLuaFunction(command.funcType.ToString(), properties[1]);
                        break;
                    case FuncType.HELP:
                        if (properties.Length == 1)
                            RunLuaFunction(command.funcType.ToString());
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