using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoSingleTon<DialogManager>
{
    private Coroutine _dialogCoroutine = null;
    private bool _excuting = false;
    private bool _input = false;
    private StringBuilder _sb = null;

    [SerializeField]
    private float _dialogCooltime = 0.6f;
    private bool _dialogLock = false;

    [SerializeField]
    private GameObject _dialogCanvas = null;
    [SerializeField]
    private Image _image = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _subNameText = null;
    [SerializeField]
    private TextMeshProUGUI _dialogText = null;

    private void Awake()
    {
        _sb = new StringBuilder();
        _dialogCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!_excuting || _input)
            return;
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            _input = true;
        }
    }

    /// <summary>
    /// Dialog를 강제로 종료시킵니다.
    /// </summary>
    public void DialogForceExit()
    {
        StopCoroutine(_dialogCoroutine);
        DialogEnd();
    }

    private void DialogEnd()
    {
        _image.sprite = null;
        _nameText.SetText("");
        _subNameText.SetText("");
        _dialogText.SetText("");
        _excuting = false;
        _input = false;
        _dialogCanvas.SetActive(false);
        if (!_dialogLock)
            StartCoroutine(DialogCooltimeCoroutine());
    }


    /// <summary>
    /// 다이얼로그를 시작합니다.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="Callback"></param>
    /// <returns></returns>
    public bool DialogStart(DialogDataSO data)
    {
        return DialogStart(data, null);
    }

    /// <summary>
    /// 다이얼로그를 시작합니다. 다이얼로그가 끝난 후 callback을 실행합니다.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public bool DialogStart(DialogDataSO data, Action callback)
    {
        if (_dialogLock || _excuting)
            return false;
        _excuting = true;
        _input = false;
        _dialogCanvas.SetActive(true);
        _dialogCoroutine = StartCoroutine(DialogCoroutine(data, callback));
        return true;
    }

    private IEnumerator DialogCoroutine(DialogDataSO data, Action callback = null)
    {
        _sb.Clear();
        DialogData curData = null;
        for (int i = 0; i < data.dialogDatas.Count; i++)
        {
            _sb.Clear();
            curData = data.dialogDatas[i];
            _image.sprite = curData.characterData.faceSprite;
            _nameText.SetText(curData.characterData.npcName);
            _subNameText.SetText(curData.characterData.subName);
            _subNameText.color = curData.characterData.subColor;

            for (int j = 0; j < curData.dialogs.Count; j++)
            {
                string targetText = curData.dialogs[j];
                for (int k = 0; k < targetText.Length; k++) // 텍스트 제작
                {
                    if (_input)
                    {
                        _input = false;
                        _dialogText.SetText(targetText);
                        break;
                    }

                    _sb.Append(targetText[k]);
                    _dialogText.SetText(_sb.ToString());
                    yield return new WaitForSeconds(curData.nextCharDelay);
                }
                yield return new WaitUntil(() => _input);
                _input = false;
                _sb.Clear();
            }
        }

        DialogEnd();
        callback?.Invoke();
    }

    private IEnumerator DialogCooltimeCoroutine()
    {
        _dialogLock = true;
        yield return new WaitForSeconds(_dialogCooltime);
        _dialogLock = false;
    }
}
