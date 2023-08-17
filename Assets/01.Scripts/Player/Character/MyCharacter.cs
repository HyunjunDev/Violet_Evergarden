using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MyCharacter : MonoBehaviour
{
    [SerializeField]
    private ECharacterType _characterType = ECharacterType.None;
    public ECharacterType characterType => _characterType;

    protected Dictionary<ECharacterModuleType, CharacterModule> _modulesDic = new Dictionary<ECharacterModuleType, CharacterModule>();

    private CharacterMovingManager _characterMovingManager = null;
    public CharacterMovingManager characterMovingManager => _characterMovingManager;

    private CharacterCollider _characterCollider = null;
    public CharacterCollider characterCollider => _characterCollider;

    private CharacterRenderer _characterRenderer = null;
    public CharacterRenderer characterRenderer => _characterRenderer;

    private CharacterAnimation _characterAnimation = null;
    public CharacterAnimation characterAnimation => _characterAnimation;

    [SerializeField]
    private ParticleSystem _jumpParticle = null;
    public ParticleSystem JumpParticle => _jumpParticle;

    [SerializeField]
    private ParticleSystem _landingParticle = null;
    public ParticleSystem LandingParticle => _landingParticle;

    #region ���� ������Ʈ
    private Rigidbody2D _rigid = null;
    public Rigidbody2D rigid => _rigid;
    #endregion

    protected virtual void Awake()
    {
        _characterMovingManager = GetComponent<CharacterMovingManager>();
        _characterCollider = GetComponent<CharacterCollider>();
        _characterRenderer = transform.Find("Renderer").GetComponent<CharacterRenderer>();
        _characterAnimation = _characterRenderer.GetComponent<CharacterAnimation>();
        _rigid = GetComponent<Rigidbody2D>();
        ModuleSetting();
        foreach(var module in _modulesDic.Values)
        {
            module.SetCharacter(this);
        }
    }

    private void Update()
    {
        foreach (var module in _modulesDic.Values)
        {
            module.UpdateModule();
        }
    }

    /// <summary>
    /// ĳ���Ϳ��� ����� ����� �����մϴ�.
    /// </summary>
    protected abstract void ModuleSetting();

    /// <summary>
    /// ĳ������ �±� �̺�Ʈ�� �߻��� �� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="characterType"></param>
    public abstract void TagCharacter(MyCharacter oldCharacter, MyCharacter changeCharacter, bool myTurn);

    /// <summary>
    /// �ʱ�ȭ �۾��� �����մϴ�.
    /// </summary>
    public void ResetCharacter()
    {
        ExitActionAllModule();
        _characterMovingManager.ResetMovingManager();
    }

    /// <summary>
    /// ��� ����� Exit �Լ��� �����մϴ�.
    /// </summary>
    public void ExitActionAllModule()
    {
        foreach (var module in _modulesDic.Values)
        {
            module.Exit();
        }
    }

    /// <summary>
    /// moduleType�� �´� ����� ������ ����ϴ�.
    /// </summary>
    /// <param name="moduleType"></param>
    public void LockActionCharacterByModule(bool value, params ECharacterModuleType[] moduleTypes)
    {
        List<CharacterModule> modules = GetModules(moduleTypes);
        foreach (var module in modules)
        {
            module.locked = value;
        }
    }

    /// <summary>
    /// moduleType�� �´� ����� ������ Exit�� �����մϴ�.
    /// </summary>
    /// <param name="moduleType"></param>
    public void ExitActionCharacterByModule(params ECharacterModuleType[] moduleTypes)
    {
        List<CharacterModule> modules = GetModules(moduleTypes);
        foreach (var module in modules)
        {
            module.Exit();
        }
    }
    
    /// <summary>
    /// T Ÿ�Կ� �´� ����� �����ɴϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModule<T>(ECharacterModuleType moduleType) where T : CharacterModule
    {
        return _modulesDic[moduleType] as T;
    }

    public List<CharacterModule> GetModules(params ECharacterModuleType[] moduleTypes)
    {
        List<CharacterModule> result = new List<CharacterModule>();
        foreach (var moduleType in moduleTypes)
        {
            result.Add(GetModule<CharacterModule>(moduleType));
        }
        return result;
    }
}
