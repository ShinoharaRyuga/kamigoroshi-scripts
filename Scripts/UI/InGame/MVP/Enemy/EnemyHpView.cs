using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class EnemyHpView : MonoBehaviour
{
    [SerializeField] Slider _view;
    [SerializeField] float _duration = 0.5f;
    [SerializeField] Ease _ease = Ease.Linear;

    float? _maxValue = null;

    public void SetMaxValue(float value)
    {
        _maxValue = value;
    }

    public void SetValue(float value)
    {
        if(_maxValue == null)
        {
            Debug.LogError("’l‚ÌÅ‘å’l‚ªÝ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
            return;
        }

        var from = _view.value;
        var to = value / _maxValue.Value;

        DOVirtual.Float(from, to, _duration, v => _view.value = v).SetEase(_ease);
    }
}
