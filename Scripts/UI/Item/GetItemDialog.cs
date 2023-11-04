using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using InputControl;
using UnityEngine.UI;
using TMPro;
using System;

public class GetItemDialog : MonoBehaviour
{
    [SerializeField] float _moveAnchorPosY = -10;
    [SerializeField] float _moveSpeed = 0.25f;
    [SerializeField] Ease _moveEase = Ease.Linear;
    [SerializeField] float _fadeSpeed = 0.25f;
    [SerializeField] Ease _fadeEase = Ease.Linear;
    [Space(10)]
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] RectTransform _rectTransform;
    [Space(10)]
    [SerializeField] Image _itemImage;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _summaryText;

    bool _isShowDialog;

    private void Reset()
    {
        if (TryGetComponent(out _canvasGroup))
        {
            _canvasGroup.TryGetComponent(out _rectTransform);
        }
    }

    private void Awake()
    {
        if (!_canvasGroup || !_rectTransform)
        {
            if (TryGetComponent(out _canvasGroup))
            {
                _canvasGroup.TryGetComponent(out _rectTransform);
            }
        }

        PlayerInputs.Instance.AddUIAction(ActionMaps.Events, UIActionType.Submit, UnityEngine.InputSystem.InputActionPhase.Performed, HideDialog);
    }
    private void Start()
    {
        ItemManager.Instance.ShowDialogEvent = ShowDialog;
    }

    public void ShowDialog(ItemData data)
    {
        _itemImage.sprite = data.ItemSprite;
        _nameText.text = data.ItemName;
        _summaryText.text = data.ItemText;

        var seq = DOTween.Sequence();

        seq
            .Join(_canvasGroup.DOFade(1f, _fadeSpeed).SetEase(_fadeEase))
            .Join(_rectTransform.DOAnchorPosY(_moveAnchorPosY, _moveSpeed).SetEase(_moveEase).From())
            .OnComplete(() =>
            {
                _isShowDialog = true;
            });

        seq.Play();

        PauseManager.Instance.ExecutePause();
        PlayerInputs.Instance.ChangeActionMap(ActionMaps.Events);
    }

    void HideDialog()
    {
        if (!_isShowDialog) return;

        _isShowDialog = false;

        _canvasGroup.DOFade(0f, _fadeSpeed).SetEase(_fadeEase)
            .OnComplete(() =>
            {
                //‚±‚±‚Å‰ğœˆ—
                PauseManager.Instance.ExecutePause();
                PlayerInputs.Instance.ChangeActionMap(ActionMaps.InGame);
                ItemManager.Instance.NamePlateSetEvent.Value = false;
                if(ItemManager.Instance.IsAction == true)
                {
                    ItemManager.Instance.ActionEvent();
                }
            });

    }
}
