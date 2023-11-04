using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingTips : MonoBehaviour
{
    [SerializeField] float _delayTime = 1f;
    [SerializeField] float _changeTipsInterval = 5f;
    [SerializeField] Vector2 _closeWindowInterval = new Vector2(10f, 10f);
    [Space(10)]
    [SerializeField] TextMeshProRuby _titleText;
    [SerializeField] TextMeshProRuby _contentText;
    [Space(10)]
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] CanvasGroup _backCanvasGroup;
    [Space(10)]
    [SerializeField] LoadingAnimView _loadingAnimView;

    TipsData[] _tips;
    int _currentTipIndex = 0;

    readonly string DATA_PATH = "TipsDatas";

    private void Reset()
    {
        TryGetComponent(out _canvasGroup);
    }

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;

        _backCanvasGroup.alpha = 0f;
        _backCanvasGroup.blocksRaycasts = false;

        LoadAllTips();
    }

    void LoadAllTips()
    {
        var datas = Resources.LoadAll<TipsData>(DATA_PATH);

        _tips = datas;

        Resources.UnloadUnusedAssets();
    }

    public IEnumerator OnOpen()
    {
        yield return new WaitForSeconds(_delayTime);

        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        _backCanvasGroup.alpha = 1f;
        _backCanvasGroup.blocksRaycasts = true;

        //ƒVƒƒƒbƒtƒ‹
        _tips = _tips.OrderBy(i => System.Guid.NewGuid()).ToArray();

        StartCoroutine(ChangeTips());
        StartCoroutine(_loadingAnimView.Play());

        yield return OnFinish();
    }

    IEnumerator OnFinish()
    {
        var r = Random.Range(_closeWindowInterval.x, _closeWindowInterval.y);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        r = 1f;
#endif

        yield return new WaitForSeconds(r);

        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;

        yield return new WaitForSeconds(_delayTime);
    }

    IEnumerator ChangeTips()
    {
        while (true)
        {
            var t = _tips[_currentTipIndex % _tips.Length];

            _titleText.Text = t.Title;
            _contentText.Text = t.Content;

            yield return new WaitForSeconds(_changeTipsInterval);

            _currentTipIndex++;
        }
    }
}
