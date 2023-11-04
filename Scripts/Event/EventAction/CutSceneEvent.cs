using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CutSceneEvent : IEventAction
{
    [SerializeField] PlayableDirector _cutScene;

    public SelectEvent[] NextEvents => null;

    public async UniTask OnEnter(CancellationToken ct)
    {
        await UniTask.CompletedTask;
        return;
    }

    public async UniTask<IEventAction[]> OnExit(CancellationToken ct)
    {
        await UniTask.CompletedTask;
        return null;
    }

    public async UniTask OnRunning(CancellationToken ct, EventParam Param)
    {
        if (!_cutScene) return;

        //仮でステートがPlayじゃないときは終了するようにしている
        await UniTask.WaitUntil(() => _cutScene.state != PlayState.Playing);
        _cutScene.Stop();

        return;
    }
}
