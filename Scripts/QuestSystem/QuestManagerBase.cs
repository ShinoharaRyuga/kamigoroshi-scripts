using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;
using System.Threading;

/// <summary>各クエストのシーンを管理するクラスのベースクラス </summary>
public class QuestManagerBase : MonoBehaviour
{
    [SerializeField, Tooltip("第二フィールド遷移トリガー")]
    protected NextFieldTrigger _fieldTrigger = default;

    [SerializeField]
    FadeController _fadeController = default;

    [SerializeField]
    TimeManager _timeManager = default;

    [SerializeField]
    ExecuteGameOverProcess _executeGameOverProcess = default;

    [SerializeField]
    protected WallObject _wallObj = default;

    public FadeController FadeController => _fadeController;
    public TimeManager TimeManager => _timeManager;
    public ExecuteGameOverProcess ExecuteGameOverProcess => _executeGameOverProcess;

    protected CancellationToken _ct;

    /// <summary>初期化処理 </summary>
    public virtual void Initialize()
    {
        _fadeController.FadeIn(() => StartCoroutine(StartFieldAnim()));

        _fieldTrigger.OnChangeScene += TransitionField02;

        _timeManager.Init(() => _executeGameOverProcess.GameOverByTimeoutOrDied());

        _timeManager.StartTimer();

        _ct = this.GetCancellationTokenOnDestroy();
    }

    /// <summary>第二フィールドに遷移する (シーン遷移) </summary>
    protected async void TransitionField02()
    {
        PauseManager.Instance.ExecutePause();

        var async = EventManager.Instance.SceneChanger.ReturnAsyncOperation("Field02");
        async.allowSceneActivation = false;

        await EventManager.Instance.FadeController.FadeOut();

        Debug.Log("Tipsを表示します");

        await EventManager.Instance.LoadingTips.OnOpen();
        async.allowSceneActivation = true;
    }

    protected async UniTask StopSoundNight()
    {
        AudioManager.Instance.StopSound(SoundType.SE, "SE_BG_Night");

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 死亡時もしくは時間切れの時に音を止める
    /// </summary>
    /// <returns></returns>
    protected virtual async UniTask DeadTask()
    {
        var dead = UniTask.WaitUntil(() => PlayerManager.Instance.Hp.Value <= 0, cancellationToken: _ct);
        var timeOver = UniTask.WaitUntil(() => TimeManager.IsEnded() == true, cancellationToken: _ct);
        await UniTask.WhenAny(dead, timeOver);
        await StopSoundNight();
    }

    private IEnumerator StartFieldAnim()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.ChangeCurrentField(FieldType.Ouka);
    }
}
