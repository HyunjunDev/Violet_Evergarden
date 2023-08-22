using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathModule : PlayerModule
{
    public override void Exit()
    {
    }

    protected override void InitModule()
    {
    }

    public void Death()
    {
        _player.ExitModules(_player.GetAllModuleType());
        _player.LockModules(true, _player.GetAllModuleType());
        _player.playerAnimation.DeathAnimation();
    }

    public void ReStart()
    {
        _player.LockModules(false, _player.GetAllModuleType());
        _player.playerAnimation.ResetUpLayer();
    }
}
