using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _animator = null;
    private MyCharacter _myCharacter = null;

    private void Awake()
    {
        _myCharacter = transform.parent.GetComponent<MyCharacter>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _myCharacter.characterCollider.onGrounded += () => _animator.SetBool("IsGround", true);
        _myCharacter.characterCollider.onGroundExited += () => _animator.SetBool("IsGround", false);
    }

    private void Update()
    {
        _animator.SetFloat("YVelocity", _myCharacter.rigid.velocity.y);
    }

    public void JumpAnimation()
    {
        _animator.SetTrigger("Jump");
    }

    public void MoveInputAnimation(float moveX)
    {
        _animator.SetBool("Move", moveX != 0f);
    }
}
