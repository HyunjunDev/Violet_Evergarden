using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabModule : PlayerModule
{
    public override void Exit()
    {
        _excuting = false;
        _player.LockModules(false, EPlayerModuleType.Gravity, EPlayerModuleType.Move);
    }

    protected override void InitModule()
    {
    }

    public void StartWallGrab()
    {
        _excuting = true;
        _player.LockModules(true, EPlayerModuleType.Gravity, EPlayerModuleType.Move);
        _player.GetModule<JumpModule>(EPlayerModuleType.Jump).JumpRecharge();
    }
}
