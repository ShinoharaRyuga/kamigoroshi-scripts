using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

/// <summary>���炷���̃e�L�X�g�𑀍삷��N���X</summary>
public class SynopsisController : MonoBehaviour
{
    [SerializeField, Header("���炷����\������e�L�X�g")]
    TMP_Text[] _texts = default;
    [SerializeField, Header("�e�L�X�g�\���ɂ����鎞��")]
    float _duration = 3f;
    [SerializeField, Header("�e�L�X�g��\�����Â��鎞��")]
    float _waitTime = 3f;
    [SerializeField, Header("���炷���\���܂ł̑ҋ@����")]
    float _delayTime = 2f;


#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void Start()
    {
        _waitTime = 1f;
    }
#endif

    /// <summary>�e�L�X�g�A�j���[�V�������Đ����� </summary>
    /// <param name="onComplete">�A�j���[�V�����I���̏���</param>
    public async void StartAnim(Action onComplete = default)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_delayTime));

        for (var i = 0; i < _texts.Length; i++)
        {
            if (i == _texts.Length - 1) //�Ō�̂݊�����������o�^����
            {
                TextAnim(_texts[i], onComplete);
                return;
            }

            TextAnim(_texts[i]);
        }
    }

    /// <summary>�A�j���[�V�������Đ����� </summary>
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
