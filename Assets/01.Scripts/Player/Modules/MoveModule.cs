using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModule : CharacterModule
{
    public void Move(float xInput)
    {
        _myCharacter.Rigid.velocity = _myCharacter.transform.right * xInput * 3f;
    }

    public override void Exit()
    {
    }

    protected override void InitModule()
    {
    }
}
