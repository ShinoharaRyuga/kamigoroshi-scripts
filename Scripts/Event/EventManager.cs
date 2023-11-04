using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputControl;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine.SceneManagement;
using Cinemachine;

public class EventManager : MonoBehaviour, IManager
{
    [SerializeField] CinemachineBlendDefinition _seamlessDefinition = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);
    [SerializeField] CinemachineBlendDefinition _fadeoutDefinition = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
    [Space(10)]
    [SerializeField] bool _isDebug;

    static EventManager _instance;
    public static EventManager Instance => _instance;

    TalkEventView _talkView;
    InteractEventView _interactView;
    BranchEventDailogView _branchEventDailogView;
    EventEnterView _eventEnterView;
    FadeController _fadeController;
    SceneChanger _sceneChanger;
    LoadingTips _loadingTips;
    CanvasGroup _eventUIGroup;
    CanvasGroup _inGameUIGroup;
    CanvasGroup _interactUIGroup;

    bool _isPlay;
    EventParam _eventParam = new();

    ReactiveProperty<bool> _namePlateSetEvent = new ReactiveProperty<bool>();

    CancellationTokenSource _cts;

    public TalkEventView TalkView => _talkView;
    public InteractEventView InteractView => _interactView;
    public BranchEventDailogView BranchView => _branchEventDailogView;
    public SceneChanger SceneChanger => _sceneChanger;
    public LoadingTips LoadingTips => _loadingTips;
    public FadeController FadeController => _fadeController;
    public IReadOnlyReactiveProperty<bool> NamePlateSetEvent => _namePlateSetEvent;
    public bool IsPlaying => _isPlay;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            SceneManager.sceneUnloaded += OnCancel;
        }
    }

    public void Initialize(EventManagerAttachment attachment)
    {
        _talkView = attachment.TalkView;
        _interactView = attachment.InteractView;
        _branchEventDailogView = attachment.BranchView;
        _eventEnterView = attachment.EnterView;
        _fadeController = attachment.FadeController;
        _sceneChanger = attachment.SceneChanger;
        _loadingTips = attachment.LoadingTips;
        _inGameUIGroup = attachment.InGameUIGroup;
        _eventUIGroup = attachment.EventUIGroup;
        _interactUIGroup = attachment.InteractUIGroup;

        _cts = new();

        PlayerInputs.Instance.AddUIAction(ActionMaps.Events, UIActionType.Submit, UnityEngine.InputSystem.InputActionPhase.Performed, () => _eventParam.IsNext = true);
    }

    void OnCancel(Scene scene)
    {
        if (_cts == null) return;

        _cts.Cancel();
        _cts.Dispose();
        _cts = null;

        if (!_isPlay) return;

        CameraManager.Instance.ResetCameraPriority(new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f));

        OnFinish("Exceptional Event End");
    }

    public async void EventStart(EventData data, System.Action onComplete = null, System.Action onStart = null, EventData[] nexts = null)
    {
        if (!data) return;

        if (_isPlay) return;

        _isPlay = true;
        CameraManager.Instance.SetFlag();

        if (!_isDebug)
        {
            PauseManager.Instance.ExecutePause();
        }

        PlayerInputs.Instance.ChangeActionMap(ActionMaps.Events);

        await OnStartProcess(data, onStart);

        _eventParam.IsNext = false;

        Debug.Log("Event Start");

        RunEvent(data, _cts.Token, onComplete, nexts).Forget();
    }

    async UniTask RunEvent(EventData data, CancellationToken ct, System.Action onComplete = null, EventData[] nexts = null)
    {
        await _eventEnterView.Play(ct);

        var evns = data.Events;
        var currentIndex = 0;
        var nextIndex = 0;

        while (true)
        {
            await evns[currentIndex].OnEnter(ct);

            //イベントの結果を待機する
            await evns[currentIndex].OnRunning(ct, _eventParam);

            //分岐用に次のイベントを戻り値として受け取る
            var newEvent = await evns[currentIndex].OnExit(ct);

            currentIndex++;

            if (currentIndex >= evns.Length)
            {
                currentIndex = 0;

                if (newEvent == null)
                {
                    if (nexts == null || nextIndex >= nexts.Length)
                    {
                        await OnCompleteProcess(data, onComplete);  //イベント終了時処理
                        return;
                    }

                    evns = nexts[nextIndex].Events;
                    nextIndex++;
                }
                else
                {
                    evns = newEvent;
                }
            }

            await UniTask.Yield(ct);
        }
    }

    private void OnFinish(string log)
    {
        _isPlay = false;
        _namePlateSetEvent.Value = false;

        if (!_isDebug)
        {
            PauseManager.Instance.ExecutePause();
        }

        Debug.Log(log);
    }

    /// <summary> 各UIの表示/非表示を操作する関数</summary>
    /// <param name="type">イベントの種類</param>
    /// <param name="isActive">表示or非表示</param>
    async UniTask ControlUIGroup(EventType type, bool isActive)
    {
        if (isActive)
        {
            switch (type)
            {
                case EventType.Talk:
                    _eventUIGroup.alpha = 1;
                    break;
                case EventType.Interact:
                    _interactUIGroup.alpha = 1;
                    break;
            }

            _inGameUIGroup.alpha = 0;
        }
        else
        {
            switch (type)
            {
                case EventType.Talk:
                    _eventUIGroup.alpha = 0;
                    break;
                case EventType.Interact:
                    await _interactUIGroup.DOFade(0, 0.2f).AsyncWaitForCompletion();
                    break;
            }

            _inGameUIGroup.alpha = 1;
            _eventEnterView.CanvasGroup.alpha = 0;
        }
    }

    /// <summary>イベント終了時に行う処理 </summary>
    /// <param name="type">イベントの種類</param>
    /// <param name="onComplete">終了時処理</param>
    async UniTask OnCompleteProcess(EventData data, System.Action onComplete = null)
    {
        switch (data.FinishTransitionType)
        {
            case EventTransitionType.Fade:
                
                await _fadeController.FadeOut();

                await ControlUIGroup(data.EventType, false);

                PlayerInputs.Instance.ChangeActionMap(ActionMaps.InGame);
                CameraManager.Instance.ResetCameraPriority(new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f));
                onComplete?.Invoke();

                await _fadeController.FadeIn();
                break;
            case EventTransitionType.Seamless:

                await ControlUIGroup(data.EventType, false);

                PlayerInputs.Instance.ChangeActionMap(ActionMaps.InGame);
                CameraManager.Instance.ResetCameraPriority(new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f));
                onComplete?.Invoke();
                break;
        }

        OnFinish("Event End");
    }

    async UniTask OnStartProcess(EventData data, System.Action onStart = null)
    {
        switch (data.StartTransitionType)
        {
            case EventTransitionType.Fade:
                await _fadeController.FadeOut();

                _namePlateSetEvent.Value = true;
                CameraManager.Instance.ExchangeCutSceneCamera(CutSceneCameraPosition.RightBack, _fadeoutDefinition);
                await ControlUIGroup(data.EventType, true);   //UIを操作

                onStart?.Invoke();

                await _fadeController.FadeIn();
                break;
            case EventTransitionType.Seamless:
                onStart?.Invoke();
                _namePlateSetEvent.Value = true;
                CameraManager.Instance.ExchangeCutSceneCamera(CutSceneCameraPosition.RightBack, _seamlessDefinition);
                await ControlUIGroup(data.EventType, true);   //UIを操作
                break;
        }
    }

    public void DestroyObject()
    {
        SceneManager.sceneUnloaded -= OnCancel;
        _instance = null;
        Destroy(this.gameObject);
    }
}