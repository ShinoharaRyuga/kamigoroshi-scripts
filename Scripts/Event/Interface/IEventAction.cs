using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using System;

public interface IEventAction
{
    public SelectEvent[] NextEvents { get; }

    public UniTask OnEnter(CancellationToken ct);
    public UniTask<IEventAction[]> OnExit(CancellationToken ct);
    public UniTask OnRunning(CancellationToken ct, EventParam Param);
}

public class EventParam
{
    public bool IsNext;

    public EventParam() { }
}
