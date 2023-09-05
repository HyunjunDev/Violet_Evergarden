using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator = null;
    private Player _player = null;

    private void Awake()
    {
        _player = transform.parent.GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _player.playerCollider.onGrounded += () => _animator.SetBool("IsGround", true);
        _player.playerCollider.onGroundExited += () => _animator.SetBool("IsGround", false);
    }

    private void Update()
    {
        _animator.SetFloat("YVelocity", _player.rigid.velocity.y);
    }

    public void ChangeAnimator(RuntimeAnimatorController runtimeAnimatorController)
    {
        if(_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        _animator.runtimeAnimatorController = runtimeAnimatorController;
    }

    public void DeathAnimation()
    {
        _animator.Rebind();
        _animator.Play("Death");
        _animator.Update(0);
    }

    public void ResetUpLayer()
    {
        _animator.Play("UpLayerEmpty");
        _animator.Update(0);
    }

    public void JumpAnimation()
    {
        _animator.SetFloat("YVelocity", 0f);
        _animator.Rebind();
        _animator.SetTrigger("Jump");
    }

    public void MoveInputAnimation(float moveX)
    {
        _animator.SetBool("Move", moveX != 0f);
    }

    public void DashAnimation()
    {
        float verti = _player.playerInput.InputVector.y;
        if(verti > 0f)
        {
            _animator.Play("Jump");
        }
        else if (verti < 0f)
        {
            _animator.Play("Fall");
        }
        else
        {
            _animator.Play("Dash");
        }
    }

    public void SetDashParameter(bool value)
    {
        _animator.SetBool("Dash", value);
    }

    public void IdleAnimation()
    {
        _animator.Play("Idle");
    }

    public void SpawnDeathEffect()
    {
        switch (_player.GetModule<TagModule>(EPlayerModuleType.Tag).CurrentCharacterType)
        {
            case ECharacterType.Hana:
                GameObject dashFlowerParticle = PoolManager.Instance.Pop(EPoolType.HanaFlowerParticle).gameObject;
                dashFlowerParticle.transform.SetTransform(_player.transform.position, _player.GetLocalScale());
                break;
            case ECharacterType.Gen:
                GameObject genDaggerParticle = PoolManager.Instance.Pop(EPoolType.GenDaggerParticle).gameObject;
                genDaggerParticle.transform.SetTransform(_player.transform.position, _player.GetLocalScale());
                break;
            default:
                break;
        }
        CameraManager.Instance.ShakeCamera(_player.TagDataSO.shakeCameraData);
    }

    public void StartDeathFade()
    {
        StartCoroutine(DeathFadeCoroutine());
    }

    private IEnumerator DeathFadeCoroutine()
    {
        UIManager.Instance.FadeStart(0f, 1f, 0.4f);
        yield return new WaitForSeconds(0.4f);
        _player.transform.position = MapManager.Instance.GetRespawnPosition();
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.FadeStart(1f, 0f, 0.4f);
        yield return new WaitForSeconds(0.15f);
        _player.playerInput.InputLock = false;
    }
}
