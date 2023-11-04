using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GetItemFieldView : MonoBehaviour
{
    [SerializeField] GetItemMessageView _messagePrefab;
    [SerializeField] float _displayTime = 0.5f;
    [Space(10)]
    [SerializeField] RectTransform _root;
    [SerializeField] RectTransform _listLayoutGroup;
    [Space(10)]
    [SerializeField] float _anchorPosY = 75f;
    [SerializeField] DotParameter _moveParam;

    bool _isShowing;
    List<ItemData> _datas = new();

    private void Reset()
    {
        TryGetComponent(out _root);
    }

    private void Start()
    {
        ItemManager.Instance.ShowMessageEvent = SetMessageData;
    }

    void SetMessageData(ItemData data)
    {
        _datas.Add(data);

        if (_isShowing)
        {
            return;
        }

        var ct = this.GetCancellationTokenOnDestroy();
        ShowMessage(_datas[0], ct);
    }

    public async void ShowMessage(ItemData data, CancellationToken ct)
    {
        _isShowing = true;

        var message = Instantiate(_messagePrefab, Vector2.zero, Quaternion.identity);
        message.transform.SetParent(_root);

        await message.Show(_datas[0], _displayTime + _moveParam.Speed);

        message.GetComponent<RectTransform>()
            .DOAnchorPosY(_anchorPosY, _moveParam.Speed).SetEase(_moveParam.Ease).From(Vector2.zero, true);

        await _listLayoutGroup
            .DOAnchorPosY(_anchorPosY, _moveParam.Speed).SetEase(_moveParam.Ease).AsyncWaitForCompletion();

        _listLayoutGroup.DOAnchorPosY(0, 0f);

        await UniTask.Yield(ct);

        message.transform.SetParent(_listLayoutGroup);

        _datas.RemoveAt(0);
        _isShowing = false;

        if(_datas.Count > 0)
        {
            ShowMessage(_datas[0], ct);
        }
    }
}
