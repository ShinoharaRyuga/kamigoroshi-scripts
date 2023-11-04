using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MissionScrollView : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] RectTransform _contents;
    [Space(10)]
    [SerializeField] float _animDuration = 0.1f;
    [SerializeField] Ease _animEase = Ease.Linear;
    [Space(10)]
    [SerializeField] MissionButtonView[] _buttons;

    private void Awake()
    {
        var deff = 1f / (_buttons.Length - 1);
        var value = 0f;

        foreach(var button in _buttons)
        {
            button.SetCallback(UpdateScrollValue);

            button.SetValue(value);
            value += deff;
        }

        ChangeWidth();
    }

    [ContextMenu("Init")]
    void ChangeWidth()
    {
        if (!_contents) return;

        var x = 490 + (147 * _buttons.Length);

        _contents.sizeDelta = new Vector2(x, _contents.sizeDelta.y);
    }

    void UpdateScrollValue(float value)
    {
        DOVirtual.Float(_scrollbar.value, value, _animDuration, v => _scrollbar.value = v)
            .SetEase(_animEase);
    }
}
