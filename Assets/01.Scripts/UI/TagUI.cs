using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagUI : MonoBehaviour
{
    private Image _selectedImage = null;
    private Image _deselectImage = null;
    [SerializeField]
    private Sprite _hanaFaceSprite = null;
    [SerializeField]
    private Sprite _genFaceSprite = null;

    public void SetTagUI(ECharacterType switchType)
    {
        if (_selectedImage == null)
        {
            InitUI();
        }

        switch (switchType)
        {
            case ECharacterType.Hana:
                _selectedImage.sprite = _hanaFaceSprite;
                _deselectImage.sprite = _genFaceSprite;
                break;
            case ECharacterType.Gen:
                _selectedImage.sprite = _genFaceSprite;
                _deselectImage.sprite = _hanaFaceSprite;
                break;
            default:
                break;
        }
    }

    private void InitUI()
    {
        _selectedImage = transform.Find("SelectedImage").GetComponent<Image>();
        _deselectImage = transform.Find("DeselectImage").GetComponent<Image>();
    }
}
