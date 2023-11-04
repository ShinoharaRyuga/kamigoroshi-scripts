using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeSliderView : MonoBehaviour
{
    [SerializeField] Image _backImage;
    [SerializeField] float _duration = 0.25f;
    [SerializeField] Ease _ease;

    float? _maxValue = null;

    public void SetMaxValue(float value)
    {
        _maxValue = value;
    }

    public void SetValue(float value)
    {
        if(_maxValue == null)
        {
            Debug.LogError("’l‚ÌÅ‘å’l‚ª–¢Ý’è‚Å‚·");
            return;
        }

        var from = _backImage.fillAmount;
        var to = value / _maxValue.Value;
        DOVirtual.Float(from, to, _duration, v => _backImage.fillAmount = v).SetEase(_ease);
    }
}
