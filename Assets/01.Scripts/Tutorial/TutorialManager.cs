using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoSingleTon<TutorialManager>
{
    [SerializeField]
    private DialogDataSO _startDialog = null;
    [SerializeField]
    private DialogDataSO _dashDialog = null;
    [SerializeField]
    private DialogDataSO _tagDialog = null;

    private Player _player = null;

    private void Start()
    {
        UIManager.Instance.FadeStart(1f, 0f, 1f);
        _player = GameObject.FindObjectOfType<Player>();
        _player.playerInput.InputLock = true;
        DialogManager.Instance.DialogStart(_startDialog, () =>
        {
            _player.playerInput.InputLock = false;
            _player.LockModules(true, EPlayerModuleType.Tag, EPlayerModuleType.Dash);
        });
    }

    public void DashDialog()
    {
        _player.PlayerStopForce();
        _player.LockModules(false, EPlayerModuleType.Dash);
        _player.playerInput.InputLock = true;
        DialogManager.Instance.DialogStart(_dashDialog, () => _player.playerInput.InputLock = false);
    }

    public void TagDialog()
    {
        _player.PlayerStopForce();
        _player.LockModules(false, EPlayerModuleType.Tag);
        _player.playerInput.InputLock = true;
        DialogManager.Instance.DialogStart(_tagDialog, () => _player.playerInput.InputLock = false);
    }
}
