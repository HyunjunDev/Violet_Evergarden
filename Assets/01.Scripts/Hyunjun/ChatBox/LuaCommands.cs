using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public static void BiggerObject(ObjectType name,Dictionary<ObjectType, GameObject> obj)
    {
        GameObject val = null;
        if (obj.TryGetValue(name,out val))
        {
            val.transform.localScale *= 2f;
        }
    }
}
