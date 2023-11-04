using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EventEnterView : MonoBehaviour
{
    [SerializeField] Image _topBar;
    [SerializeField] Image _bottomBar;
    [SerializeField] CanvasGroup _canvasGroup;
    [Space(10)]
    [SerializeField] DotParameter _animParam;

    public CanvasGroup CanvasGroup => _canvasGroup;

    public async UniTask Play(CancellationToken ct)
    {
        _canvasGroup.alpha = 1;

        var seq = DOTween.Sequence();

        seq
            .Join(_topBar.transform.DOLocalMoveY(630f, _animParam.Speed).SetEase(_animParam.Ease).From())
            .Join(_bottomBar.transform.DOLocalMoveY(-630f, _animParam.Speed).SetEase(_animParam.Ease).From());

        await seq.AsyncWaitForCompletion();

        return;
    }
}
