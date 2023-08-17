using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanaCharacter : MyCharacter
{
    [SerializeField]
    private ParticleSystem _dashParticle = null;
    public ParticleSystem DashParticle => _dashParticle;
    [SerializeField]
    private ParticleSystem _dashTrailParticle = null;
    public ParticleSystem DashTrailParticle => _dashTrailParticle;
    [SerializeField]
    private Color _trailColor = Color.white;
    public Color trailColor => _trailColor;
    [SerializeField]
    private float _trailCycle = 0.08f;
    public float trailCycle => _trailCycle;
    [SerializeField]
    private float _duration = 0.2f;
    public float duration => _duration;

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
