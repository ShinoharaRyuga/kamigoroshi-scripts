using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManagerAttachment : MonoBehaviour
{
    [SerializeField] TalkEventView _talkView;
    [SerializeField] InteractEventView _interactView;
    [SerializeField] BranchEventDailogView _branchView;
    [SerializeField] EventEnterView _enterView;
    [SerializeField] CanvasGroup _eventUIGroup;
    [SerializeField] CanvasGroup _inGameUIGroup;
    [SerializeField] CanvasGroup _interactUIGroup;
    [SerializeField] FadeController _fadeController;
    [SerializeField] SceneChanger _sceneChanger;
    [SerializeField] LoadingTips _loadingTips;
  
    public TalkEventView TalkView => _talkView;
    public InteractEventView InteractView => _interactView;
    public BranchEventDailogView BranchView => _branchView;
    public EventEnterView EnterView => _enterView;
    public CanvasGroup EventUIGroup => _eventUIGroup;
    public CanvasGroup InGameUIGroup => _inGameUIGroup;
    public CanvasGroup InteractUIGroup => _interactUIGroup;
    public FadeController FadeController => _fadeController;
    public SceneChanger SceneChanger => _sceneChanger;
    public LoadingTips LoadingTips => _loadingTips;

    void Start()
    {
        EventManager.Instance.Initialize(this);

        var g = GameManager.Instance;
        var isDebug = g ? false : true;

        if (isDebug) return;
    }
}
