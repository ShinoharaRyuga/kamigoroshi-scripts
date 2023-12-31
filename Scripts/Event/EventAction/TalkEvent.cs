using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Text;
using System.Threading;
using UnityEngine;
using EventCharacterIcon;
using Cinemachine;

[System.Serializable]
public class TalkEvent : IEventAction
{
    [SerializeReference, SubclassSelector, Header("LN^[ACR")]
    EventCharacterData _characterData = new Yashima();

    [SerializeField, Header("JÌÊu")] CutSceneCameraPosition _cameraPosition = CutSceneCameraPosition.RightBack;

    [SerializeField, Header("JðØèÖ¦éÌÝè")]
    CinemachineBlendDefinition _blendDefinition =
        new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);

    [SerializeField, TextArea(5, 5), Header("àe")]
    string[] _talkTexts = new string[] { "New Message" };

    TalkEventView _view;
    int _currentIndex;
    bool _isLastPage;

    public SelectEvent[] NextEvents => null;

    public async UniTask OnEnter(CancellationToken ct)
    {
        _currentIndex = 0;
        _characterData.SetupData();

        _view = EventManager.Instance.TalkView;

        _isLastPage = _view.SetView(_talkTexts[_currentIndex].ToString(), _characterData.Name, _characterData.Icon);

        _view.OnOpen();

        CameraManager.Instance.ExchangeCutSceneCamera(_cameraPosition, _blendDefinition);

        AudioManager.Instance.PlaySound(SoundType.SE, "SE_Text", 1f);

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
            //å¥åããããã¾ã§å¾E©E
            // MEMO åE²ãEé¸æã§Param.IsNextãtrueã«ãªã£ã¦ãã¾ãEEã§ããã§falseã«ãã¦ãE
            Param.IsNext = false;
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

            //å¨ã¦ã®ãE­ã¹ããèª­ã¿è¾¼ã¿çµãã£ããçµäºE
            if (_currentIndex >= _talkTexts.Length) return;

            if (_isLastPage)
            {
                _isLastPage = _view.SetView(_talkTexts[_currentIndex].ToString(), _characterData.Name,
                    _characterData.Icon);
            }

            AudioManager.Instance.PlaySound(SoundType.SE, "SE_Text", 1f);

            await UniTask.Yield(ct);
        }
    }
}