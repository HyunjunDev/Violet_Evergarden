using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{
    private EFlipState _currentFlipState = EFlipState.None;
    public EFlipState currentFlipState => _currentFlipState;

    public void MoveInputFlip(float moveX)
    {
        if(moveX != 0f)
        {
            Flip(moveX < 0f ? EFlipState.Left : EFlipState.Right);
        }
    }

    /// <summary>
    /// flipState 방향으로 회전시킵니다.
    /// </summary>
    /// <param name="flipState"></param>
    public void Flip(EFlipState flipState)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        switch (flipState)
        {
            case EFlipState.None:
                return;
            case EFlipState.Left:
                localScale.x = -localScale.x;
                break;
            case EFlipState.Right:
                break;
            default:
                return;
        }
        _currentFlipState = flipState;
        transform.localScale = localScale;
    }
}
