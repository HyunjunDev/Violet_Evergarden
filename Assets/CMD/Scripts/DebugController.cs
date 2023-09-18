using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugController : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset _cmdTemplate;
    [SerializeField]
    private UIDocument _document;
    private ScrollView _scrollView;
    private TextField _inputField;
    private VisualElement _root;

    [SerializeField]
    private string loadFile;

    private Script luaScript;

    bool showHelp;
    public bool ShowHelp
    {
        get { return showHelp; }
        set { showHelp = value; }
    }

    private static DebugController instance;
    public static DebugController Instance => instance;

    public static DebugCommand SET_SCALE_OBJ;
    public static DebugCommand HELP;
    public static DebugCommand SET_DASH;
    public static DebugCommand SET_SPEED;
    public static DebugCommand SET_GRAVITY;
    public static DebugCommand SET_DAGGER_SPEED;
    public static DebugCommand SET_COLOR;

    public List<object> commandList;

    private string autocompleteCmd;

    private void Awake()
    {
        instance = this;

        _root = _document.rootVisualElement;

        _scrollView = _root.Q<ScrollView>("ScrollView");
        _inputField = _root.Q<TextField>("inputField");

        //_inputField.selectAllOnMouseUp = false;
        //_inputField.selectAllOnFocus = false;

        SET_SCALE_OBJ = new DebugCommand("set_scale", "Adjust the scale value of the desired object", FuncType.SET_SCALE_OBJ);

        HELP = new DebugCommand("help", "Shows a list of commands", FuncType.HELP);

        SET_DASH = new DebugCommand("set_dash", "¾ÃÀç¿±", FuncType.SET_DASH);

        SET_SPEED = new DebugCommand("set_speed", "¾ÃÀç¿±2", FuncType.SET_SPEED);

        SET_GRAVITY = new DebugCommand("set_gravity", "¾ÃÀç¿±2", FuncType.SET_GRAVITY);

        SET_DAGGER_SPEED = new DebugCommand("set_dagger_speed", "¾ÃÀç¿±3", FuncType.SET_DAGGER_SPEED);

        SET_COLOR = new DebugCommand("set_color", "¾ÃÀç¿±3", FuncType.SET_COLOR);

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
        _root.style.display = DisplayStyle.None;

        UserData.RegisterAssembly();
        UserData.RegisterType<UnityAPI>();

        luaScript = new Script();
        luaScript.Globals["Unity"] = new UnityAPI();

        LoadLuaScript(loadFile);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote)&& _root.style.display==DisplayStyle.None)
        {
            OnCmd();
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote) && _root.style.display == DisplayStyle.Flex)
        {
            OffCmd();
        }

        if (_root.style.display == DisplayStyle.Flex)
        {
            _inputField.RegisterCallback<KeyDownEvent>(OnKeyDown);
            OnUIToolkit();
        }
    }

    IEnumerator DelayFocus()
    {
        _inputField.value = string.Empty;
        yield return new WaitForSecondsRealtime(.1f);
        _inputField.Blur();
        _inputField.Focus();
    }

    public void OnCmd()
    {
        Time.timeScale = 0.0f;
        _scrollView.Clear();
        showHelp = false;
        _root.style.display = DisplayStyle.Flex;
        StartCoroutine(DelayFocus());
    }

    public void OffCmd()
    {
        Time.timeScale = 1.0f;
        _root.style.display = DisplayStyle.None;
    }

    private void OnUIToolkit()
    {
        autocompleteCmd = string.Empty;
        _scrollView.Clear();

        if (showHelp)
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommand command = commandList[i] as DebugCommand;

                string label = $"{command.commandId} - {command.CommandDescription}";
                Label labelTmp = new Label(label);
                labelTmp.AddToClassList("Label");

                _scrollView.Add(labelTmp);
            }
        }

        if(!showHelp&& _inputField.value != string.Empty)
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommand command = commandList[i] as DebugCommand;

                if (command.commandId.ToString().Contains(_inputField.value))
                {
                    if (autocompleteCmd == string.Empty)
                        autocompleteCmd = command.commandId.ToString();
                    Debug.Log(command.commandId);
                    string label = $"{command.commandId} - {command.CommandDescription}";
                    Label labelTmp = new Label(label);
                    labelTmp.AddToClassList("Label");

                    _scrollView.Add(labelTmp);
                }
            }
        }
    }

    private void OnKeyDown(KeyDownEvent evt)
    {
        Debug.Log(1);
        if (evt.keyCode == KeyCode.Return)
        {
            HandleInput();
            StartCoroutine(DelayFocus());
        }
        else if(evt.keyCode==KeyCode.Tab)
        {
            Debug.Log(2);
            if (autocompleteCmd != string.Empty)
            {
                _inputField.value = autocompleteCmd;
                _inputField.cursorIndex = _inputField.value.Length;
                _inputField.selectIndex = _inputField.value.Length;
                _inputField.selectAllOnMouseUp = false;
                _inputField.selectAllOnFocus = false;
                _inputField.textSelection.isSelectable = false;
                StartCoroutine(DelayFocus2());
            }
        }
        else if(evt.keyCode!=KeyCode.None)
        {
            Debug.Log(3);
            if (showHelp)
            {
                showHelp = false;
            }
        }
    }

    IEnumerator DelayFocus2()
    {
        yield return new WaitForSecondsRealtime(.1f);
        _inputField.textSelection.isSelectable = true;
        _inputField.Blur();
        _inputField.Focus();
    }

    private void HandleInput()
    {
        string[] properties = _inputField.value.Split(' '); 
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommand command = commandList[i] as DebugCommand;

            if(_inputField.value.Contains(command.commandId))
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
