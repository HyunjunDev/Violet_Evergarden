using UnityEngine;

public abstract class CharacterModule
{
    [SerializeField]
    protected ECharacterModuleType _characterModuleType = ECharacterModuleType.None;
    public ECharacterModuleType CharacterModuleType => _characterModuleType;
    protected MyCharacter _myCharacter = null;

    protected bool _locked = false;
    public bool Locked { get => _locked; set => _locked = value; }

    protected bool _excuting = false; // 액션 실행중?
    public bool Excuting => _excuting;

    /// <summary>
    /// 캐릭터를 설정합니다.
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacter(MyCharacter character)
    {
        _myCharacter = character;
        InitModule();
    }

    protected abstract void InitModule();

    public abstract void Exit();
}
