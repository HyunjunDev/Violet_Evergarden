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
    /// ��� ĳ������ ������ �����մϴ�.
    /// </summary>
    public void ResetAllCharacters()
    {
        for(int i = 0; i < _characters.Count; i++)
        {
            _characters[i].ResetCharacter();
        }
    }

    /// <summary>
    /// characterType�� �´� ĳ���͸� ������ ��ġ�� ȸ������ �����մϴ�.
    /// </summary>
    /// <param name="characterType"></param>
    public void SetPositionAndRotationCharacter(ECharacterType characterType, Vector3 position, Quaternion rotation)
    {
        GetCharacter(characterType).transform.SetPositionAndRotation(position, rotation);
    }

    /// <summary>
    /// ĳ���͸� �±��մϴ�. ����Ʈ �ȿ� �ִ� ������� �±׵˴ϴ�.
    /// </summary>
    public void TagCharacter()
    {
        _characterIdx = (_characterIdx + 1) % _characters.Count;
        CharacterChange();
    }

    /// <summary>
    /// characterType�� �´� ĳ���ͷ� �±��մϴ�.
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
    /// characterType�� �´� ĳ���͸� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="characterType"></param>
    /// <returns></returns>
    public MyCharacter GetCharacter(ECharacterType characterType)
    {
        return _characters.Find(x => x.characterType == characterType);
    }

    /// <summary>
    /// T Ÿ���� ĳ���͸� ��ȯ�մϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetCharacter<T>() where T : MyCharacter
    {
        return _characters.Find(x => x is T) as T;
    }
}