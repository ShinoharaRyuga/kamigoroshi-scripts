using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class GetItemMessageView : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] CanvasGroup _canvasGroup;
    [Space(10)]
    [SerializeField] DotParameter _fadeParam;

    private void Reset()
    {
        TryGetComponent(out _canvasGroup);
    }

    public UniTask Show(ItemData data, float interval)
    {
        _iconImage.sprite = data.ItemSprite;
        _text.text = data.ItemName;

        var seq = DOTween.Sequence();

        seq
            .Join(_canvasGroup.DOFade(1f, _fadeParam.Speed).SetEase(_fadeParam.Ease).From(0f, true))
            .AppendInterval(interval)
            .Append(_canvasGroup.DOFade(0f, _fadeParam.Speed).SetEase(_fadeParam.Ease))
            .OnComplete(() =>
            {
                Destroy(this.gameObject);
            });

        seq.Play();

        return UniTask.CompletedTask;
    }
}
