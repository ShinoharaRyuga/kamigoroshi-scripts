using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EndingController : MonoBehaviour
{
    [SerializeField] EndingImageView _endingImageView;
    [SerializeField] SceneChanger _sceneChanger;
    [SerializeField] FadeController _fadeController;
    [SerializeField] LoadingTips _loadingTips;
    [SerializeField] AudioPlayer _audioPlayer;
    [Space(10)]
    [SerializeField] string _nextSceneName = "Title";
    [SerializeField] float _sceneChangeInterval = 5f;
    [Space(10)]
    [SerializeField] EndingType _endingType;    //クエストの分岐クラスが出来たら変更します

    private async void Start()
    {
        PlayerManager.Instance.DestroyObject();
        YajimaMove.Instance.DestroyObject();

        var gm = GameManager.Instance;
        var ending = _endingType;

        if(gm)
        {
            ending = gm.CurrentEndingType;
        }

        _endingImageView.OnSetSprite(ending);

        await _fadeController.FadeIn();

        await StartCoroutine(Timer(_sceneChangeInterval));

        await TransitionToTitleScene();
    }

    private async UniTask TransitionToTitleScene()
    {
        PauseManager.Instance.ExecutePause();

        var async = _sceneChanger.ReturnAsyncOperation(_nextSceneName);
        async.allowSceneActivation = false;

        await _fadeController.FadeOut();

        Debug.Log("Tipsを表示します");

        await _loadingTips.OnOpen();
        async.allowSceneActivation = true;
        _audioPlayer.StopSound();
        
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        yield break;
    }
}

public enum EndingType
{
    Normal = 1,
    Bad = 2,
    True = 3,
}