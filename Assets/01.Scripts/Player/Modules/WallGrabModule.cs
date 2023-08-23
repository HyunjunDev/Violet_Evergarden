using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabModule : PlayerModule
{
    public override void Exit()
    {
        _excuting = false;
        _player.LockModules(false, EPlayerModuleType.Gravity);
    }

    protected override void InitModule()
    {
    }

    public void StartWallGrab()
    {
        _excuting = true;
        _player.GetModule<JumpModule>(EPlayerModuleType.Jump).JumpRecharge();
        _player.LockModules(true, EPlayerModuleType.Gravity);
    }
}
