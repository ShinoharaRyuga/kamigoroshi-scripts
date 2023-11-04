using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScrollViewReset : MonoBehaviour, IUIResetView
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] float _resetValue = 0f;
    [Space(10)]
    [SerializeField] float _animDuration = 0.1f;
    [SerializeField] Ease _animEase = Ease.Linear;

    public void ResetView()
    {
        DOVirtual.Float(_scrollbar.value, _resetValue, _animDuration, value => _scrollbar.value = value)
            .SetEase(_animEase);
        //_scrollbar.value = _resetValue;
    }
}
