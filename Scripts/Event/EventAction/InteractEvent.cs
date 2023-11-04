using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using InputControl;
using System;
using UniRx;
using System.Text;
using Cinemachine;

[System.Serializable]
public class InteractEvent : IEventAction
{
    [SerializeField, Header("カメラの位置")] CutSceneCameraPosition _cameraPosition = CutSceneCameraPosition.RightBack;
    [SerializeField, Header("カメラを切り替える時の設定")] CinemachineBlendDefinition _blendDefinition = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);
    [SerializeField, TextArea(5, 5)] string[] _talkTexts = new string[] {"New Message"};
    
    InteractEventView _view;
    int _currentIndex;
    bool _isLastPage;
    
    public SelectEvent[] NextEvents => null;
    public async UniTask OnEnter(CancellationToken ct)
    {
        _currentIndex = 0;

        _view = EventManager.Instance.InteractView;
        _isLastPage = _view.SetView(_talkTexts[_currentIndex].ToString());

        _view.OnOpen();

        CameraManager.Instance.IntaractCamera(_blendDefinition);

        await UniTask.CompletedTask;

        return;
    }

    public async UniTask<IEventAction[]> OnExit(CancellationToken ct)
    {
        await _view.OnClose().AsyncWaitForCompletion();
        await UniTask.CompletedTask;

        return null;
    }

    public async UniTask OnRunning(CancellationToken ct, EventParam Param)
    {
        while (true)
        {
            await UniTask.WaitUntil(() => Param.IsNext);

            if (!_isLastPage)
            {
                _isLastPage = _view.GoToNextPage();
            }

            if (_isLastPage)
            {
                _currentIndex++;
            }
            Param.IsNext = false;
            
            if (_currentIndex >= _talkTexts.Length) return;

            if (_isLastPage)
            {
                _isLastPage = _view.SetView(_talkTexts[_currentIndex].ToString());
            }

            await UniTask.Yield(ct);
        }
    }
}
