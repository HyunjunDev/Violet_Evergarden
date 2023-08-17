using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenCharacter : MyCharacter
{
    public override void TagCharacter(MyCharacter oldCharacter, MyCharacter changeCharacter, bool myTurn)
    {
        if (myTurn)
        {
            gameObject.SetActive(true);
            transform.position = oldCharacter.transform.position;
            oldCharacter.gameObject.SetActive(false);
            characterRenderer.Flip(oldCharacter.characterRenderer.currentFlipState);
        }
    }

    protected override void ModuleSetting()
    {
        _modulesDic.Add(ECharacterModuleType.Move, new MoveModule());
        _modulesDic.Add(ECharacterModuleType.Gravity, new GravityModule());
        _modulesDic.Add(ECharacterModuleType.Jump, new JumpModule());
        _modulesDic.Add(ECharacterModuleType.Dash, new DashModule());
    }
}
