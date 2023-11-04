using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

/// <summary>あらすじのテキストを操作するクラス</summary>
public class SynopsisController : MonoBehaviour
{
    [SerializeField, Header("あらすじを表示するテキスト")]
    TMP_Text[] _texts = default;
    [SerializeField, Header("テキスト表示にかかる時間")]
    float _duration = 3f;
    [SerializeField, Header("テキストを表示しつづける時間")]
    float _waitTime = 3f;
    [SerializeField, Header("あらすじ表示までの待機時間")]
    float _delayTime = 2f;


#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void Start()
    {
        _waitTime = 1f;
    }
#endif

    /// <summary>テキストアニメーションを再生する </summary>
    /// <param name="onComplete">アニメーション終了の処理</param>
    public async void StartAnim(Action onComplete = default)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayTime));

        for (var i = 0; i < _texts.Length; i++)
        {
            if (i == _texts.Length - 1) //最後のみ完了時処理を登録する
            {
                TextAnim(_texts[i], onComplete);
                return;
            }

            TextAnim(_texts[i]);
        }
    }

    /// <summary>アニメーションを再生する </summary>
    void TextAnim(TMP_Text text, Action onComplete = default)
    {
        text.color = new Color(1, 1, 1, 0);
        var seq = DOTween.Sequence();

        seq.Append(text.DOFade(1f, _duration))
           .AppendInterval(_waitTime)
           .Append(text.DOFade(0f, _duration))
           .AppendInterval(1f)
           .AppendCallback(() => onComplete?.Invoke());
    }
}
