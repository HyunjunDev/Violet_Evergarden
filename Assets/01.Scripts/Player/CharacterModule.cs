using UnityEngine;

public abstract class CharacterModule
{
    [SerializeField]
    protected ECharacterModuleType _characterModuleType = ECharacterModuleType.None;
    public ECharacterModuleType CharacterModuleType => _characterModuleType;
    protected MyCharacter _myCharacter = null;

    protected bool _locked = false;
    public bool Locked { get => _locked; set => _locked = value; }

    protected bool _excuting = false; // �׼� ������?
    public bool Excuting => _excuting;

    /// <summary>
    /// ĳ���͸� �����մϴ�.
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
