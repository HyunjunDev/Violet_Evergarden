using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyCharacter : MonoBehaviour
{
    [SerializeField]
    private ECharacterType _characterType = ECharacterType.None;
    public ECharacterType characterType => _characterType;

    protected List<CharacterModule> _modules = new List<CharacterModule>();

    private CharacterMovingManager _characterMovingManager = null;
    public CharacterMovingManager characterMovingManager => _characterMovingManager;

    private CharacterCollider _characterCollider = null;
    public CharacterCollider characterCollider => _characterCollider;

    private CharacterRenderer _characterRenderer = null;
    public CharacterRenderer characterRenderer => _characterRenderer;

    private CharacterAnimation _characterAnimation = null;
    public CharacterAnimation characterAnimation => _characterAnimation;


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
        for (int i = 0; i < _modules.Count; i++)
        {
            _modules[i].SetCharacter(this);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _modules.Count; i++)
        {
            _modules[i].UpdateModule();
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
    public abstract void TagCharacter(ECharacterType characterType);

    /// <summary>
    /// ��� ����� Exit �Լ��� �����մϴ�.
    /// </summary>
    public void ExitActionAllModule()
    {
        for(int i = 0; i < _modules.Count; i++) 
        {
            _modules[i].Exit();
        }
    }

    /// <summary>
    /// moduleType�� �´� ����� ������ Exit�� �����մϴ�.
    /// </summary>
    /// <param name="moduleType"></param>
    public T ExitActionCharacterByModule<T>() where T : CharacterModule
    {
        T module = GetModule<T>();
        module.Exit();
        return module;
    }
    
    /// <summary>
    /// T Ÿ�Կ� �´� ����� �����ɴϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModule<T>() where T : CharacterModule
    {
        return _modules.Find(x => x is T) as T;
    }
}
