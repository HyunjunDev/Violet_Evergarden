using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagModule : PlayerModule
{
    private ECharacterType _currentCharacterType = ECharacterType.Hana;


    public override void Exit()
    {

    }

    protected override void InitModule()
    {

    }

    public void TagWithInput()
    {
        _currentCharacterType = (ECharacterType)((int)(_currentCharacterType + 1) % ((int)ECharacterType.Size));
        TagCharacter(_currentCharacterType);
    }

    public void TagCharacter(ECharacterType targetType)
    {
        // 0, 1
        _currentCharacterType = targetType;
        switch (_currentCharacterType)
        {
            case ECharacterType.Hana:
                _player.ExitModules(EPlayerModuleType.WallGrab, EPlayerModuleType.ThrowDagger);
                _player.DetachModule(EPlayerModuleType.WallGrab, EPlayerModuleType.ThrowDagger);
                _player.AttachModule(EPlayerModuleType.Dash, new DashModule());
                _player.playerAnimation.ChangeAnimator(_player.TagDataSO.hanaAnimatorController);
                GameObject dashFlowerParticle = PoolManager.Instance.Pop(EPoolType.HanaFlowerParticle).gameObject;
                dashFlowerParticle.transform.position = _player.transform.position;
                break;
            case ECharacterType.Gen:
                _player.ExitModules(EPlayerModuleType.Dash);
                _player.DetachModule(EPlayerModuleType.Dash);
                _player.AttachModule(EPlayerModuleType.WallGrab, new WallGrabModule());
                _player.AttachModule(EPlayerModuleType.ThrowDagger, new ThrowDaggerModule());
                _player.playerAnimation.ChangeAnimator(_player.TagDataSO.genAnimatorController);
                GameObject genDaggerParticle = PoolManager.Instance.Pop(EPoolType.GenDaggerParticle).gameObject;
                genDaggerParticle.transform.position = _player.transform.position;
                break;
            default:
                break;
        }
        CameraManager.Instance.ShakeCamera(_player.TagDataSO.frequency,
            _player.TagDataSO.amplitude,
            _player.TagDataSO.shakeTime,
            _player.TagDataSO.easeType);
    }
}
