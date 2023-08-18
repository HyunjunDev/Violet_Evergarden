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

    #region 공통 컴포넌트
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
    /// 캐릭터에서 사용할 모듈을 생성합니다.
    /// </summary>
    protected abstract void ModuleSetting();

    /// <summary>
    /// 캐릭터의 태그 이벤트가 발생할 때 호출됩니다.
    /// </summary>
    /// <param name="characterType"></param>
    public abstract void TagCharacter(MyCharacter oldCharacter, MyCharacter changeCharacter, bool myTurn);

    /// <summary>
    /// 초기화 작업을 수행합니다.
    /// </summary>
    public void ResetCharacter()
    {
        ExitActionAllModule();
        _characterMovingManager.ResetMovingManager();
    }

    /// <summary>
    /// 모든 모듈의 Exit 함수를 실행합니다.
    /// </summary>
    public void ExitActionAllModule()
    {
        foreach (var module in _modulesDic.Values)
        {
            module.Exit();
        }
    }

    /// <summary>
    /// moduleType에 맞는 모듈을 가져와 잠굼니다.
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
    /// moduleType에 맞는 모듈을 가져와 Exit를 실행합니다.
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
    /// T 타입에 맞는 모듈을 가져옵니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModule<T>(ECharacterModuleType moduleType) where T : CharacterModule
    {
        if (_modulesDic.ContainsKey(moduleType) == false)
            return null;
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
