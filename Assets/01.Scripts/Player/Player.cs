using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ECharacterType _startCharacterType = ECharacterType.Hana;

    private List<MyCharacter> _characters = new List<MyCharacter>();

    private int _characterIdx = 0;

    private MyCharacter _currentCharacter = null;
    public MyCharacter currentCharacter => _currentCharacter;

    private PlayerInput _playerInput = null;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _characters = transform.Find("CharacterContainer").GetComponentsInChildren<MyCharacter>().ToList();
        SetStartCharacter();
    }

    private void SetStartCharacter()
    {
        for (int i = 0; i < _characters.Count; i++)
        {
            if (_characters[i].characterType == _startCharacterType)
            {
                _currentCharacter = _characters[i];
                _characters[i].gameObject.SetActive(true);
            }
            else
            {
                _characters[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 모든 캐릭터의 리셋을 수행합니다.
    /// </summary>
    public void ResetAllCharacters()
    {
        for(int i = 0; i < _characters.Count; i++)
        {
            _characters[i].ResetCharacter();
        }
    }

    /// <summary>
    /// characterType에 맞는 캐릭터를 가져와 위치와 회전값을 설정합니다.
    /// </summary>
    /// <param name="characterType"></param>
    public void SetPositionAndRotationCharacter(ECharacterType characterType, Vector3 position, Quaternion rotation)
    {
        GetCharacter(characterType).transform.SetPositionAndRotation(position, rotation);
    }

    /// <summary>
    /// 캐릭터를 태그합니다. 리스트 안에 있는 순서대로 태그됩니다.
    /// </summary>
    public void TagCharacter()
    {
        _characterIdx = (_characterIdx + 1) % _characters.Count;
        CharacterChange();
    }

    /// <summary>
    /// characterType에 맞는 캐릭터로 태그합니다.
    /// </summary>
    public void TagCharacter(ECharacterType characterType)
    {
        _characterIdx = _characters.IndexOf(GetCharacter(characterType));
        CharacterChange();
    }

    private void CharacterChange()
    {
        MyCharacter oldCharacter = _currentCharacter;
        _currentCharacter = _characters[_characterIdx];
        for (int i = 0; i < _characters.Count; i++)
        {
            _characters[i].TagCharacter(oldCharacter, _currentCharacter, _characters[i].characterType == _currentCharacter.characterType);
        }
        ResetAllCharacters();
    }

    /// <summary>
    /// characterType에 맞는 캐릭터를 반환합니다.
    /// </summary>
    /// <param name="characterType"></param>
    /// <returns></returns>
    public MyCharacter GetCharacter(ECharacterType characterType)
    {
        return _characters.Find(x => x.characterType == characterType);
    }

    /// <summary>
    /// T 타입의 캐릭터를 반환합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetCharacter<T>() where T : MyCharacter
    {
        return _characters.Find(x => x is T) as T;
    }
}