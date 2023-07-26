using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanaCharacter : MyCharacter
{
    public override void TagCharacter(ECharacterType characterType)
    {

    }

    protected override void ModuleSetting()
    {
        _modules.Add(new MoveModule());
        _modules.Add(new GravityModule());
        _modules.Add(new JumpModule());
    }
}
