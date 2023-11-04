using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

[System.Serializable]
public class ActionEvent : IEventAction
{
    [SerializeReference, SubclassSelector] IGameEvent[] _releaseFlagEvents;

    public SelectEvent[] NextEvents => null;

    public async UniTask OnEnter(CancellationToken ct)
    {
        await UniTask.CompletedTask;
        
        foreach(var e in _releaseFlagEvents)
        {
            await e.ReleaseEvent();
        }

        return;
    }

    public async UniTask<IEventAction[]> OnExit(CancellationToken ct)
    {
        await UniTask.CompletedTask;
        return null;
    }

    public UniTask OnRunning(CancellationToken ct, EventParam Param)
    {
        return UniTask.CompletedTask;
    }
}
