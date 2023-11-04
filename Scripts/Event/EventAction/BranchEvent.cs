using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using UniRx;

[System.Serializable]
public class BranchEvent : IEventAction
{
    [SerializeField] SelectEvent[] _branchEvents;

    BranchEventDailogView _view;
    IEventAction[] _nextEvent;

    bool _isNext;

    public SelectEvent[] NextEvents => _branchEvents;

    public async UniTask OnEnter(CancellationToken ct)
    {
        _view = EventManager.Instance.BranchView;
        _view.OnOpen();

        var onClicks = new Action[NextEvents.Length];

        for(int i = 0; i < onClicks.Length; i++)
        {
            var num = i;

            onClicks[i] = () =>
            {
                _nextEvent = NextEvents[num].SelectEvents;
                _isNext = true;
            };
        }

        _view.SetButtonMethod(NextEvents, onClicks);

        await UniTask.Yield(ct);
        return;
    }

    public async UniTask<IEventAction[]> OnExit(CancellationToken ct)
    {
        //Sequence‚ÌI—¹‚ð‘Ò‹@
        await _view.OnClose().AsyncWaitForCompletion();
        return _nextEvent;
    }

    public async UniTask OnRunning(CancellationToken ct, EventParam Param)
    {
        await UniTask.WaitUntil(() => _isNext);
        _isNext = false;

        return;
    }
}
