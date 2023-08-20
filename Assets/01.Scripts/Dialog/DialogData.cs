using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogData
{
    public NPCDataSO characterData = null;

    public float nextCharDelay = 0.02f;
    [TextArea]
    public List<string> dialogs = new List<string>();
    public string endActionKey = "";
}