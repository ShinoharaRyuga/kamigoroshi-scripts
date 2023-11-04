using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TMP_Text))]
public class InteractHelpTextView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [Space(10)]
    [SerializeField] DotParameter _fadeParam;

    Tween _fadeTween;

    private void Reset()
    {
        TryGetComponent(out _text);
    }

    public void SetActiveText(bool flag)
    {
        var alpha = 0f;

        if (flag)
        {
            _text.enabled = true;
            alpha = 1f;
        }

        if(_fadeTween != null)
        {
            _fadeTween.Kill();
        }

        _fadeTween = _text.DOFade(alpha, _fadeParam.Speed).SetEase(_fadeParam.Ease)
            .OnComplete(() =>
            {
                if (flag) return;

                _text.enabled = false;
            });
    }
}
