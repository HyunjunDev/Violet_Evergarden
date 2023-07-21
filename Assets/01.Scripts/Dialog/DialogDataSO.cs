using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DialogData")]
public class DialogDataSO : ScriptableObject
{
    public List<DialogData> dialogDatas = new List<DialogData>();
    public DialogDataSO nextData = null;
}