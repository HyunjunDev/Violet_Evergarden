using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyCharacter : MonoBehaviour
{
    [SerializeField]
    private ECharacterType _characterType = ECharacterType.None;
    public ECharacterType CharacterType => _characterType;

    protected List<CharacterModule> _modules = new List<CharacterModule>();

    #region ���� ������Ʈ
    private Rigidbody2D _rigid = null;
    public Rigidbody2D Rigid => _rigid;
    #endregion

    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        ModuleSetting();
        for (int i = 0; i < _modules.Count; i++)
        {
            _modules[i].SetCharacter(this);
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
