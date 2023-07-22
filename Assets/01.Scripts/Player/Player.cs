using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<MyCharacter> _characters = new List<MyCharacter>();

    private int _characterIdx = 0;
    public MyCharacter _currentCharacter = null;
    public MyCharacter CurrentCharacter => _currentCharacter;

    private PlayerInput _playerInput = null;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _characters = transform.Find("CharacterContainer").GetComponentsInChildren<MyCharacter>().ToList();
        for(int i = 0; i < _characters.Count; i++)
        {
            _characters[i].gameObject.SetActive(_currentCharacter == _characters[i]);
        }
    }

    /// <summary>
    /// 캐릭터를 태그합니다. 리스트 안에 있는 순서대로 태그됩니다.
    /// </summary>
    public void TagCharacter()
    {
        _characterIdx = Mathf.Clamp(_characterIdx++, 0, _characters.Count - 1);
        _currentCharacter = _characters[_characterIdx];
        for(int i = 0; i < _characters.Count; i++)
        {
            _characters[i].TagCharacter(_currentCharacter.CharacterType);
        }
    }
}