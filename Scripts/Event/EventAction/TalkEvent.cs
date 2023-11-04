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
    [SerializeReference, SubclassSelector, Header("�L�����N�^�[�A�C�R��")]
    EventCharacterData _characterData = new Yashima();

    [SerializeField, Header("�J�����̈ʒu")] CutSceneCameraPosition _cameraPosition = CutSceneCameraPosition.RightBack;

    [SerializeField, Header("�J������؂�ւ��鎞�̐ݒ�")]
    CinemachineBlendDefinition _blendDefinition =
        new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);

    [SerializeField, TextArea(5, 5), Header("���e")]
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
            //入力がされるまで征E��E
            // MEMO 刁E���E選択でParam.IsNextがtrueになってしまぁE�EでここでfalseにしてめE
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

            //全てのチE��ストを読み込み終わったら終亁E
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