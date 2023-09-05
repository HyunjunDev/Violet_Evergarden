using System.Collections;
using UnityEngine;

public abstract class PlayerModule
{
    protected Player _player = null;

    protected bool _locked = false;
    public bool locked { get => _locked; set => _locked = value; }

    protected bool _excuting = false; // 액션 실행중?
    public bool excuting => _excuting;

    /// <summary>
    /// 캐릭터를 설정합니다.
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
    protected bool _useable = true;
    protected Coroutine _groundedRechargeCoroutine = null;
    protected float _rechargeTime = 0f;

    protected void GroundedRecharge()
    {
        if (_groundedRechargeCoroutine != null)
            StopCoroutine(_groundedRechargeCoroutine);
        _groundedRechargeCoroutine = StartCoroutine(GroundDashRechargeCoroutine());
    }

    private IEnumerator GroundDashRechargeCoroutine()
    {
        yield return new WaitForSeconds(_rechargeTime);
        if (_player.playerCollider.GetCollision(EBoundType.Down))
        {
            _useable = true;
        }
    }
    #endregion
}
