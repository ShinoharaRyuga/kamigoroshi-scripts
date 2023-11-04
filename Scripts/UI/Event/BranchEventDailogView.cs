using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class BranchEventDailogView : MonoBehaviour
{
    [SerializeField] Button _buttonPrefab;
    [Space(10)]
    [SerializeField] SelectionWindowView _headerWindow;
    [Space(10)]
    [SerializeField] RectTransform _animRect;
    [SerializeField] RectTransform _buttonRect;
    [SerializeField] CanvasGroup _selectView;
    [SerializeField] CanvasGroup _talkView;
    [SerializeField] SelectionWindowView _selectionView;
    [Space(10)]
    [SerializeField] DotParameter _moveParam;
    [SerializeField] DotParameter _fadeParam;

    bool _onClick;
    List<Button> _buttons = new List<Button>();

    public bool OnClick => _onClick;

    private void Reset()
    {
        TryGetComponent(out _buttonRect);
        TryGetComponent(out _selectView);
        TryGetComponent(out _selectionView);
    }

    private void Awake()
    {
        if (!_buttonRect)
        {
            Reset();
        }
    }

    public void OnOpen()
    {
        var seq = DOTween.Sequence();

        seq
            .Join(_animRect.DOAnchorPosX(850f, _moveParam.Speed).SetEase(_moveParam.Ease))
            .Join(_selectView.DOFade(1f, _fadeParam.Speed).SetEase(_fadeParam.Ease))
            .Join(_talkView.DOFade(1f, _fadeParam.Speed).SetEase(_fadeParam.Ease));

        seq.Play();
    }

    public Sequence OnClose()
    {
        var seq = DOTween.Sequence();

        seq
            .Join(_animRect.DOAnchorPosX(906.25f, _moveParam.Speed).SetEase(_moveParam.Ease))
            .Join(_selectView.DOFade(0f, _fadeParam.Speed).SetEase(_fadeParam.Ease));

        return seq;
    }

    public void SetButtonMethod(SelectEvent[] events, Action[] onClicks)
    {
        _onClick = false;
        var listCount = _buttons.Count;

        for(int i = 0; i < listCount; i++)
        {
            var button = _buttons[0];
            _buttons.Remove(button);

            Destroy(button.gameObject);
        }

        for (int i = 0; i < events.Length; i++)
        {
            var button = Instantiate(_buttonPrefab, _buttonRect);
            _buttons.Add(button);

            var child = button.transform.GetChild(1);

            if (child && child.TryGetComponent(out TextMeshProUGUI text))
            {
                text.text = events[i].SelectionName;
            }

            var num = i;

            button.onClick.AddListener(() =>
            {
                onClicks[num]?.Invoke();
                _onClick = true;

                UIManager.Instance.ResetAllWindows(_headerWindow);
                _selectionView.ActiveWindow(false);
            });
        }

        if(_buttons.Count > 0)
        {
            _selectionView.ChangeSelection(_buttons[0]);
            _selectionView.OnSwitchToValidWindow();
        }
    }
}
