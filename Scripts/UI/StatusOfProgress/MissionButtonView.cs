using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public class MissionButtonView : MonoBehaviour, ISelectHandler
{
    [SerializeField] MissionData _data;
    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] Image _backGround;
    float _value;

    public delegate void ScrollEvent(float value);
    ScrollEvent _onScroll;

    public delegate void SelectEvent(MissionData param);
    SelectEvent _onSelect;

    private void Awake()
    {
        _textMeshPro.text = _data.Title;
    }

    public void Setup(Action<Selectable> selectionEvent, SelectionWindowView nextWindow)
    {
        if (TryGetComponent(out Button button))
        {
            button.onClick.AddListener(nextWindow.OnSwitchToValidWindow);
            button.onClick.AddListener(() => selectionEvent(button));
        }
    }

    /// <summary>
    /// 選択されたときに実行するデリゲートを設定する
    /// </summary>
    /// <param name="e"></param>
    public void SetCallback(ScrollEvent e)
    {
        _onScroll = e;
    }

    /// <summary>
    /// 選択されたときに実行するデリゲートを設定する
    /// </summary>
    /// <param name="e"></param>
    public void SetCallback(SelectEvent e)
    {
        _onSelect = e;
    }

    /// <summary>
    /// デリゲートの引数に指定する値を設定する
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        _value = value;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _onScroll?.Invoke(_value);
        _onSelect?.Invoke(_data);
        _backGround.sprite = _data.BackGroundSprite;
    }
}
