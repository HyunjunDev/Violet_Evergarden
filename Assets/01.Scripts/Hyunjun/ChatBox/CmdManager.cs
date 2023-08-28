using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class CmdManager : MonoBehaviour
{
    public GameObject cmdUI;
    public TMP_InputField inputField;

    private LuaEnvironment lua;

    private void Awake()
    {
        lua = FindObjectOfType<LuaEnvironment>();
    }

    void Update()
    {
        OnOffCmd();
    }

    public void OnOffCmd()
    {
        if (!cmdUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Time.timeScale = 0;
                cmdUI.gameObject.SetActive(true);
                inputField.ActivateInputField();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Time.timeScale = 1;
                cmdUI.gameObject.SetActive(false);
            }
        }
    }

    public void EnterCmd()
    {
        CheckCmd(inputField.text);
        inputField.text = "";
    }

    public void CheckCmd(string text)
    {
        string[] words = text.Split(' ');
        ObjectType objType = (ObjectType)Enum.Parse(typeof(ObjectType), words[1].ToUpperInvariant());
        lua.LuaGameState.SelectedObj = objType;
        switch(words[0])
        {
            case "/Big":
            case "/Small":
                lua.AdvanceScript();
                break;
        }
    }
}

[Serializable]
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
}