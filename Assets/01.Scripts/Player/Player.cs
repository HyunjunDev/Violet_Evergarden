using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ECharacterType _currentCharacterType = ECharacterType.Hana;
    private Dictionary<ECharacterModuleType, CharacterModule> _modules = new Dictionary<ECharacterModuleType, CharacterModule>();

    private PlayerInput _playerInput = null;
    public PlayerInput playerInput => _playerInput;
    private MovingController _movingController = null;
    public MovingController movingController => _movingController;
    private PlayerRenderer _playerRenderer = null;
    public PlayerRenderer playerRenderer => _playerRenderer;
    private PlayerAnimation _playerAnimation = null;
    public PlayerAnimation playerAnimation => _playerAnimation;
    private PlayerCollider _playerCollider = null;
    public PlayerCollider playerCollider => _playerCollider;
    private Rigidbody2D _rigid = null;
    public Rigidbody2D rigid => _rigid;

    [SerializeField]
    private ParticleSystem _jumpParticle = null;
    public ParticleSystem JumpParticle => _jumpParticle;
    [SerializeField]
    private ParticleSystem _landingParticle = null;
    public ParticleSystem LandingParticle => _landingParticle;
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

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movingController = GetComponent<MovingController>();
        _playerCollider = GetComponent<PlayerCollider>();
        _playerRenderer = transform.Find("Renderer").GetComponent<PlayerRenderer>();
        _playerAnimation = _playerRenderer.GetComponent<PlayerAnimation>();
        _modules.Add(ECharacterModuleType.Move, new MoveModule());
        _modules.Add(ECharacterModuleType.Gravity, new GravityModule());
        _modules.Add(ECharacterModuleType.Jump, new JumpModule());
        _modules.Add(ECharacterModuleType.Dash, new DashModule());
        foreach(var module in  _modules.Values)
        {
            module.SettingModule(this);
        }
    }

    private void Update()
    {
        ModuleUpdate();
    }

    private void ModuleUpdate()
    {
        foreach(var module in _modules.Values)
        {
            module.UpdateModule();
        }
    }

    public void AttachModule()
    {

    }

    public void DetachModule()
    {

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
        if (_modules.ContainsKey(moduleType) == false)
            return null;
        return _modules[moduleType] as T;
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