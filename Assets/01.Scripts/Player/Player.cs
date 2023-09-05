using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Player : MonoBehaviour, IReStartable
{
    [SerializeField]
    private ECharacterType _startCharcterType = ECharacterType.Hana;

    private Dictionary<EPlayerModuleType, PlayerModule> _modules = new Dictionary<EPlayerModuleType, PlayerModule>();

    #region Data
    [SerializeField]
    private DashDataSO _dashDataSO = null;
    public DashDataSO DashDataSO => _dashDataSO;
    [SerializeField]
    private MovementDataSO _movementDataSO = null;
    public MovementDataSO MovementDataSO => _movementDataSO;
    [SerializeField]
    private GravityDataSO _gravityDataSO = null;
    public GravityDataSO GravityDataSO => _gravityDataSO;
    [SerializeField]
    private JumpDataSO _jumpDataSO = null;
    public JumpDataSO JumpDataSO => _jumpDataSO;
    [SerializeField]
    private TagDataSO _tagDataSO = null;
    public TagDataSO TagDataSO => _tagDataSO;
    [SerializeField]
    private DaggerDataSO _daggerDataSO = null;
    public DaggerDataSO DaggerDataSO => _daggerDataSO;
    [SerializeField]
    private ThrowDaggerDataSO _throwDaggerDataSO = null;
    public ThrowDaggerDataSO ThrowDaggerDataSO => _throwDaggerDataSO;
    [SerializeField]
    private MultiplierDataSO _multiplierDataSO = null;
    public MultiplierDataSO MultiplierDataSO => _multiplierDataSO;
    #endregion

    #region Renderer
    private PlayerRenderer _playerRenderer = null;
    public PlayerRenderer playerRenderer => _playerRenderer;
    private PlayerAnimation _playerAnimation = null;
    public PlayerAnimation playerAnimation => _playerAnimation;
    #endregion

    private PlayerInput _playerInput = null;
    public PlayerInput playerInput => _playerInput;
    private PlayerCollider _playerCollider = null;
    public PlayerCollider playerCollider => _playerCollider;
    private Rigidbody2D _rigid = null;
    public Rigidbody2D rigid => _rigid;

    private MovingController _movingController = null;
    public MovingController movingController => _movingController;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movingController = GetComponent<MovingController>();
        _playerCollider = GetComponent<PlayerCollider>();
        _playerRenderer = transform.Find("Renderer").GetComponent<PlayerRenderer>();
        _playerAnimation = _playerRenderer.GetComponent<PlayerAnimation>();

        _modules.Add(EPlayerModuleType.Move, new MoveModule());
        _modules.Add(EPlayerModuleType.Gravity, new GravityModule());
        _modules.Add(EPlayerModuleType.Jump, new JumpModule());
        _modules.Add(EPlayerModuleType.Death, new DeathModule());
        _modules.Add(EPlayerModuleType.Tag, new TagModule());

        MapManager.Instance.onPlayerDead.AddListener(ReStart);
        foreach (var module in _modules.Values)
        {
            module.SettingModule(this);
        }
    }

    private void Start()
    {
        //TagCharacter(_currentCharacterType);
        GetModule<TagModule>(EPlayerModuleType.Tag).
            TagCharacter(_startCharcterType);
    }

    private void Update()
    {
        ModuleUpdate();
    }

    private void ModuleUpdate()
    {
        foreach (var module in _modules.Values)
        {
            module.UpdateModule();
        }
    }

    public void AttachModule(EPlayerModuleType moduleType, PlayerModule module)
    {
        if (!_modules.ContainsKey(moduleType))
        {
            _modules.Add(moduleType, module);
            module.SettingModule(this);
        }
    }

    public void DetachModule(params EPlayerModuleType[] moduleTypes)
    {
        foreach(var moduleType in moduleTypes)
        {
            if (_modules.ContainsKey(moduleType))
            {
                _modules.Remove(moduleType);
            }
        }
    }

    /// <summary>
    /// moduleType�� �´� ������ ������ ���� �˻縦 �մϴ�.
    /// </summary>
    /// <param name="moduleTypes"></param>
    /// <returns></returns>
    public bool CheckExcutingModules(params EPlayerModuleType[] moduleTypes)
    {
        List<PlayerModule> modules = GetModules(moduleTypes);
        foreach (var module in modules)
        {
            if (module.excuting)
                return true;
        }
        return false;
    }

    /// <summary>
    /// moduleType�� �´� ������ ������ �����ϴ�.
    /// </summary>
    /// <param name="moduleType"></param>
    public void LockModules(bool value, params EPlayerModuleType[] moduleTypes)
    {
        List<PlayerModule> modules = GetModules(moduleTypes);
        foreach (var module in modules)
        {
            module.locked = value;
        }
    }

    /// <summary>
    /// moduleType�� �´� ������ ������ Exit�� �����մϴ�.
    /// </summary>
    /// <param name="moduleType"></param>
    public void ExitModules(params EPlayerModuleType[] moduleTypes)
    {
        List<PlayerModule> modules = GetModules(moduleTypes);
        foreach (var module in modules)
        {
            module.Exit();
        }
    }

    public EPlayerModuleType[] GetAllModuleType()
    {
        List<EPlayerModuleType> moduleTypes = new List<EPlayerModuleType>();
        for(int i = 0; i < (int)EPlayerModuleType.Size; i++)
        {
            moduleTypes.Add((EPlayerModuleType)i);
        }
        return moduleTypes.ToArray();
    }

    /// <summary>
    /// T Ÿ�Կ� �´� ������ �����ɴϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModule<T>(EPlayerModuleType moduleType) where T : PlayerModule
    {
        if (!_modules.ContainsKey(moduleType))
            return null;
        return _modules[moduleType] as T;
    }

    public List<PlayerModule> GetModules(params EPlayerModuleType[] moduleTypes)
    {
        List<PlayerModule> result = new List<PlayerModule>();
        foreach (var moduleType in moduleTypes)
        {
            PlayerModule module = GetModule<PlayerModule>(moduleType);
            if(module != null)
            {
                result.Add(module);
            }
        }
        return result;
    }

    public Vector3 GetLocalScale()
    {
        Vector3 result = transform.localScale;
        result.x *= _playerRenderer.currentFlipState == EFlipState.Left ? -1f : 1f;
        return result;
    }

    public void ReStart()
    {
        Debug.Log("player 부활");
    }
}
