using System.Collections;
using UnityEngine;

public abstract class PlayerModule
{
    protected Player _player = null;

    protected bool _locked = false;
    public bool locked { get => _locked; set => _locked = value; }

    protected bool _excuting = false; // �׼� ������?
    public bool excuting => _excuting;

    /// <summary>
    /// ĳ���͸� �����մϴ�.
    /// </summary>
    /// <param name="character"></param>
    public void SettingModule(Player player)
    {
        _player = player;
        InitModule();
        _player.playerCollider.onGrounded += OnGrounded;
        _player.playerCollider.onGroundExited += OnGroundExited;
    }

    protected abstract void InitModule();

    public abstract void Exit();

    public virtual void UpdateModule() { }

    protected Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return _player.StartCoroutine(enumerator);
    }

    protected void StopCoroutine(Coroutine coroutine)
    {
        _player.StopCoroutine(coroutine);
    }

    protected virtual void OnGrounded()
    {

    }

    protected virtual void OnGroundExited()
    {

    }

    #region GroundedRecharge
    protected bool _useable { get; private set; }
    protected Coroutine _groundedRechargeCoroutine = null;
    protected float _maxRechargeTime = 0f;
    protected float _curRechargeTime = 0f;

    protected void SetUseable(bool useableValue)
    {
        _useable = useableValue;
        if(useableValue)
        {
            _curRechargeTime = _maxRechargeTime;
        }
        else
        {
            _curRechargeTime = 0f;
        }
    }

    protected void GroundedRecharge()
    {
        if (_groundedRechargeCoroutine != null)
            StopCoroutine(_groundedRechargeCoroutine);
        _groundedRechargeCoroutine = StartCoroutine(GroundDashRechargeCoroutine());
    }

    private IEnumerator GroundDashRechargeCoroutine()
    {
        _curRechargeTime = 0f;
        while (_curRechargeTime <= _maxRechargeTime)
        {
            _curRechargeTime += Time.deltaTime;
            yield return null;
        }
        _curRechargeTime = _maxRechargeTime;

        if (_player.playerCollider.GetCollision(EBoundType.Down))
        {
            SetUseable(true);
        }
    }
    #endregion
}
