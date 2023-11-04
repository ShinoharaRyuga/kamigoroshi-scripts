using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

public class Quest01Manager : QuestManagerBase
{
    public static Quest01Manager Instance;
    
    [Header("システム")] 
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Volume _postProsess;
    [SerializeField] private VolumeProfile _battlePostProsess;
    [SerializeField] private VolumeProfile _stage1;

    [Header("オブジェクト")] [SerializeField] private GameObject _portal;
    [SerializeField] private GameObject _clearWall; // Subscriptionの解除用
    [SerializeField] private GameObject _clearPlayerPos;
    [SerializeField] private GameObject _clearYajimaPos;
    [SerializeField] private GameObject _dcal;
    [SerializeField] private List<Renderer> _chochinPatterned;
    [SerializeField] private List<Renderer> _chochinPatternless;

    [Header("マテリアル")] [SerializeField] private Material _chochinPatternedMaterial;
    [SerializeField] private Material _chochinPatternlessMaterial;
    [SerializeField] private Material _chochinIsekaiPatternedMaterial;
    [SerializeField] private Material _chochinIsekaiPatternlessMaterial;

    [Header("会話データ")] 
    [SerializeField] private EventData _talk2;
    [SerializeField] private EventData _talk3;

    private bool _isSearchingSound = false;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
    }
    
    private void Start()
    {
        Initialize();

        _enemyManager.OnEnemyDeactivation
            .TakeUntilDestroy(this)
            .Subscribe(_ => OnQuestEnd())
            .AddTo(this);

        _dcal.SetActive(false);

        DeadTask().Forget();
    }

    protected override async UniTask DeadTask()
    {
        var dead = UniTask.WaitUntil(() => PlayerManager.Instance.Hp.Value <= 0, cancellationToken: _ct);
        var timeOver = UniTask.WaitUntil(() => TimeManager.IsEnded() == true, cancellationToken: _ct);
        await UniTask.WhenAny(dead, timeOver);
        await OnStopSound();
        await StopSoundNight();
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("Quest01");
        GameManager.Instance.CurrentQuestType.Value = QuestType.Quest01;
        _clearWall.gameObject.SetActive(false);
    }
    
    public void OnPlaySound()
    {
        if (!_isSearchingSound)
        {
            _isSearchingSound = true;
            AudioManager.Instance.PlaySound(SoundType.BGM, "BGM_Battle");
            AudioManager.Instance.PlaySound(SoundType.SE, "SE_HeartBeat");
        }
    }

    public async UniTask OnStopSound()
    {
        if (_isSearchingSound)
        {
            _isSearchingSound = false;
            AudioManager.Instance.StopSound(SoundType.BGM, "BGM_Battle");
            AudioManager.Instance.StopSound(SoundType.SE, "SE_HeartBeat");
        }

        await UniTask.CompletedTask;
    }

    public void OnQuestStart()
    {
        EventManager.Instance.EventStart(_talk2,
            () =>
            {
                //PauseManager.Instance.ExecutePause();
                _enemyManager.StartQuest();
            },
            () =>
            {
                _dcal.SetActive(true);
                _chochinPatterned.ForEach(renderer => renderer.material = _chochinIsekaiPatternedMaterial);
                _chochinPatternless.ForEach(renderer => renderer.material = _chochinIsekaiPatternlessMaterial);
                _postProsess.profile = _battlePostProsess;
                _portal.SetActive(false);
            });
    }

    private async UniTask OnQuestEnd()
    {
        Debug.Log("クリア");
        
        _clearWall.gameObject.SetActive(true);
        OnStopSound().Forget();
        EventManager.Instance.EventStart(_talk3);
    }

    public async UniTask QuestEnd()
    {
        await _wallObj.WallDissolve();
        await EventManager.Instance.FadeController.FadeOut();
        _dcal.SetActive(false);
        PlayerManager.Instance.transform.position = _clearPlayerPos.transform.position;
        YajimaMove.Instance.transform.position = _clearYajimaPos.transform.position;
        _chochinPatterned.ForEach(renderer => renderer.material = _chochinPatternedMaterial);
        _chochinPatternless.ForEach(renderer => renderer.material = _chochinPatternlessMaterial);
        _postProsess.profile = _stage1;
        OnStopSound().Forget();
        await EventManager.Instance.FadeController.FadeIn();
    }
}