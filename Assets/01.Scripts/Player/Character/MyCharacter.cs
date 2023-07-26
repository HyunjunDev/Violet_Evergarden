using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyCharacter : MonoBehaviour
{
    [SerializeField]
    private ECharacterType _characterType = ECharacterType.None;
    public ECharacterType CharacterType => _characterType;

    protected List<CharacterModule> _modules = new List<CharacterModule>();

    private CharacterMovingManager _characterMovingManager = null;
    public CharacterMovingManager CharacterMovingManager => _characterMovingManager;

    #region 공통 컴포넌트
    private Rigidbody2D _rigid = null;
    public Rigidbody2D Rigid => _rigid;
    #endregion

    protected virtual void Awake()
    {
        _characterMovingManager = GetComponent<CharacterMovingManager>();   
        _rigid = GetComponent<Rigidbody2D>();
        ModuleSetting();
        for (int i = 0; i < _modules.Count; i++)
        {
            _modules[i].SetCharacter(this);
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
    public abstract void TagCharacter(ECharacterType characterType);

    /// <summary>
    /// 모든 모듈의 Exit 함수를 실행합니다.
    /// </summary>
    public void ExitActionAllModule()
    {
        for(int i = 0; i < _modules.Count; i++) 
        {
            _modules[i].Exit();
        }
    }

    /// <summary>
    /// moduleType에 맞는 모듈을 가져와 Exit를 실행합니다.
    /// </summary>
    /// <param name="moduleType"></param>
    public T ExitActionCharacterByModule<T>() where T : CharacterModule
    {
        T module = GetModule<T>();
        module.Exit();
        return module;
    }
    
    /// <summary>
    /// T 타입에 맞는 모듈을 가져옵니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModule<T>() where T : CharacterModule
    {
        return _modules.Find(x => x is T) as T;
    }
}
