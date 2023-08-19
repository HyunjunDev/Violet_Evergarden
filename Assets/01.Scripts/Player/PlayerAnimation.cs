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

    public void DashAnimation(Vector2 dashPower)
    {
        float verti = dashPower.y;
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

    public void IdleAnimation()
    {
        _animator.Play("Idle");
    }
}
