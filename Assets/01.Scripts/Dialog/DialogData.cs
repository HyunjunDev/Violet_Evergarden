using System.Collections.Generic;
[System.Serializable]
public class DialogData
{
    public NPCDataSO characterData = null;

    public float nextCharDelay = 0.02f;
    public List<string> dialogs = new List<string>();
    public string endActionKey = "";
}