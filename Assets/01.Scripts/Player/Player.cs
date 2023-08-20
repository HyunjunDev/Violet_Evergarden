using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ECharacterType _currentCharacterType = ECharacterType.Hana;
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
        _modules.Add(EPlayerModuleType.Dash, new DashModule());
        foreach (var module in _modules.Values)
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
        foreach (var module in _modules.Values)
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
    /// moduleType�� �´� ����� ������ ���� �˻縦 �մϴ�.
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
    /// moduleType�� �´� ����� ������ ����ϴ�.
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
    /// moduleType�� �´� ����� ������ Exit�� �����մϴ�.
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

    /// <summary>
    /// T Ÿ�Կ� �´� ����� �����ɴϴ�.
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
            result.Add(GetModule<PlayerModule>(moduleType));
        }
        return result;
    }
}