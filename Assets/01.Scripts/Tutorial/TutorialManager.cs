using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoSingleTon<TutorialManager>
{
    [SerializeField]
    private DialogDataSO _startDialog = null;

    private Player _player = null;

    private void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _player.playerInput.InputLock = true;
        DialogManager.Instance.DialogStart(_startDialog, () => _player.playerInput.InputLock = false); ;
    }
}
