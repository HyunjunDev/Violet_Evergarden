using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCDataSO _npcDataSO = null;
    public NPCDataSO npcDataSO => _npcDataSO;

    [SerializeField]
    private DialogDataSO _dialogDataSO = null;
    public DialogDataSO dialogDataSO => _dialogDataSO;

    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _subNameText = null;

    [SerializeField]
    private GameObject _doInteractObj = null;

    private bool _dialoging = false;


    protected virtual void Start()
    {
        _doInteractObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            TryDialog();
    }

    private void OnValidate()
    {
        if (_npcDataSO == null)
            return;

        _nameText.SetText(_npcDataSO.npcName);
        _subNameText.SetText(_npcDataSO.subName);
        _subNameText.color = _npcDataSO.subColor;
    }

    public virtual void TryDialog()
    {
        if (!_doInteractObj.activeSelf || _dialoging || _dialogDataSO == null)
            return;
        if(DialogManager.Instance.DialogStart(_dialogDataSO, () => { _dialogDataSO = _dialogDataSO.nextData; _dialoging = false; }))
        {
            _dialoging = true;
        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _doInteractObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _doInteractObj.SetActive(false);
        }
    }
}
