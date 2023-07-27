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
        _modules.Add(new MoveModule());
        _modules.Add(new GravityModule());
        _modules.Add(new JumpModule());
    }
}
