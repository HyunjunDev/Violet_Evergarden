using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void MoveInputAnimation(float moveX)
    {
        _animator.SetBool("Move", moveX != 0f);
    }
}
