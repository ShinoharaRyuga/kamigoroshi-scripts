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
    [SerializeReference, SubclassSelector, Header("キャラクターアイコン")]
    EventCharacterData _characterData = new Yashima();

    [SerializeField, Header("カメラの位置")] CutSceneCameraPosition _cameraPosition = CutSceneCameraPosition.RightBack;

    [SerializeField, Header("カメラを切り替える時の設定")]
    CinemachineBlendDefinition _blendDefinition =
        new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);

    [SerializeField, TextArea(5, 5), Header("内容")]
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
            //蜈･蜉帙′縺輔ｌ繧九∪縺ｧ蠕・ｩ・
            // MEMO 蛻・ｲ舌・驕ｸ謚槭〒Param.IsNext縺荊rue縺ｫ縺ｪ縺｣縺ｦ縺励∪縺・・縺ｧ縺薙％縺ｧfalse縺ｫ縺励※繧・
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

            //蜈ｨ縺ｦ縺ｮ繝・く繧ｹ繝医ｒ隱ｭ縺ｿ霎ｼ縺ｿ邨ゅｏ縺｣縺溘ｉ邨ゆｺ・
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