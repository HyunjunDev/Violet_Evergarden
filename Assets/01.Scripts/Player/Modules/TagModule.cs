using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagModule : PlayerModule
{
    private ECharacterType _currentCharacterType = ECharacterType.Hana;
    public ECharacterType CurrentCharacterType => _currentCharacterType;

    private Action<ECharacterType> _onTaged = null;
    public Action<ECharacterType> onTaged { get => _onTaged; set => _onTaged = value; }

    public override void Exit()
    {

    }

    protected override void InitModule()
    {
        _maxRechargeTime = _player.DashDataSO.dashRechargeTime;
    }

    public override void UpdateModule()
    {
        base.UpdateModule();
        UIManager.Instance.SetFillUI(EFillUIType.Tag, _curRechargeTime, _maxRechargeTime);
    }

    protected override void OnGrounded()
    {
        base.OnGrounded();
        SetUseable(true);
    }

    public void TagWithInput()
    {
        if(!_useable)
        {
            return;
        }
        SetUseable(false);
        _currentCharacterType = (ECharacterType)((int)(_currentCharacterType + 1) % ((int)ECharacterType.Size));
        TagCharacter(_currentCharacterType);
        GroundedRecharge();
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
                dashFlowerParticle.transform.SetTransform(_player.transform.position, _player.GetLocalScale());
                break;
            case ECharacterType.Gen:
                _player.ExitModules(EPlayerModuleType.Dash);
                _player.DetachModule(EPlayerModuleType.Dash);
                _player.AttachModule(EPlayerModuleType.WallGrab, new WallGrabModule());
                _player.AttachModule(EPlayerModuleType.ThrowDagger, new ThrowDaggerModule());
                _player.playerAnimation.ChangeAnimator(_player.TagDataSO.genAnimatorController);
                GameObject genDaggerParticle = PoolManager.Instance.Pop(EPoolType.GenDaggerParticle).gameObject;
                genDaggerParticle.transform.SetTransform(_player.transform.position, _player.GetLocalScale());
                break;
            default:
                break;
        }

        onTaged?.Invoke(_currentCharacterType);
        UIManager.Instance.SetTagUI(_currentCharacterType);
        CameraManager.Instance.ShakeCamera(_player.TagDataSO.shakeCameraData);
    }
}
