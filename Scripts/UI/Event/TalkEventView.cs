using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class TalkEventView : MonoBehaviour
{
    [SerializeField] RectTransform _fieldRect;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] Image _characterIcon;
    [SerializeField] Image _dialogIcon;
    [Space(10)]
    [SerializeField] DotParameter _fadeParam;
    [SerializeField] DotParameter _switchingTextParam;
    [Space(10)]
    [SerializeField] DotParameter _dialogIconParam;

    int _currentPage;
    Tween _dialogTween;

    private void Reset()
    {
        TryGetComponent(out _fieldRect);
        TryGetComponent(out _canvasGroup);
    }

    private void Awake()
    {
        if (!_fieldRect)
        {
            Reset();
        }
    }

    public bool SetView(string text, string name, Sprite sprite)
    {
        _characterIcon.sprite = sprite;
        _nameText.text = name;
        _messageText.text = "";

        _messageText.pageToDisplay = 1;
        _currentPage = 0;

        //‘Ò‹@ˆ—“ü‚ê‚È‚­‚Ä‚à“®‚¢‚Ä‚é‚©‚çˆê’U‚±‚ê‚Ås‚­

        _messageText.DOFade(0f, _switchingTextParam.Speed).SetEase(_switchingTextParam.Ease)
            .OnComplete(() =>
            {
                _messageText.text = text;
                _messageText.DOFade(1f, _switchingTextParam.Speed).SetEase(_switchingTextParam.Ease);
            });

        return _messageText.textInfo.pageCount <= _currentPage;
    }

    public bool GoToNextPage()
    {
        var count = _messageText.textInfo.pageCount;

        _messageText.pageToDisplay++;
        _currentPage++;

        if(_currentPage >= count)
        {
            return true;
        }

        return false;
    }

    public void ChangeMaxVisibleCharacters(int count)
    {
        _messageText.maxVisibleCharacters = count;
    }

    public void OnOpen()
    {
        var seq = DOTween.Sequence();

        seq
            .Join(_canvasGroup.DOFade(1, _fadeParam.Speed).SetEase(_fadeParam.Ease));

        seq.Play();

        _dialogTween = _dialogIcon.DOFade(1f, _dialogIconParam.Speed).SetEase(_dialogIconParam.Ease).SetLoops(-1, LoopType.Yoyo).From(0f);
    }

    public Sequence OnClose()
    {
        var seq = DOTween.Sequence();

        seq
            .Join(_canvasGroup.DOFade(0, _fadeParam.Speed).SetEase(_fadeParam.Ease));

        _dialogTween.Kill();
        _dialogIcon.DOFade(1f, 0f);

        return seq;
    }
}

[System.Serializable]
public class DotParameter
{
    [SerializeField] float _speed = 0.1f;
    [SerializeField] Ease _ease = Ease.Linear;

    public float Speed => _speed;
    public Ease Ease => _ease;
}
