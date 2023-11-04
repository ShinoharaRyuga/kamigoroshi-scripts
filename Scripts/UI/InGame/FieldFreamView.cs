using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;

public class FieldFreamView : MonoBehaviour
{
    [SerializeField] DotParameter _moveParam;
    [SerializeField] DotParameter _fadeParam;
    [SerializeField] DotParameter _nameFadeParam;
    [SerializeField] float _fadeInterval = 2f;
    [Space(10)]
    [SerializeField] Image _mainImage;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] RectTransform _maskTransform;
    [SerializeField] CanvasGroup _nameGroup;

    Sequence _currentSequence;

    private void Start()
    {
        var g = GameManager.Instance;
        var isDebug = g ? false : true;

        if (isDebug) return;
        GameManager.Instance.FieldInfo.Subscribe(x => SetSprite(x.FieldName)).AddTo(this);
        gameObject.SetActive(true);
    }

    void SetSprite(Sprite sprite)
    {
        _mainImage.sprite = sprite;
    }

    private void OnEnable()
    {
        var g = GameManager.Instance;
        var isDebug = g ? false : true;

        if (isDebug) return;
        g.OnEnterSceneTransitionComplete += Play;
    }

    private void OnDisable()
    {
        var g = GameManager.Instance;
        var isDebug = g ? false : true;

        if (isDebug) return;
        g.OnEnterSceneTransitionComplete -= Play;
    }

    public void Play()
    {
        if(_currentSequence != null)
        {
            _currentSequence.Kill();
        }

        var seq = DOTween.Sequence();
        
        seq
            .Append(_canvasGroup.DOFade(1f, 0f))
            .Join(_nameGroup.DOFade(0f, 0f))
            .Append(_maskTransform.DOAnchorMax(new Vector2(0f, 1f), _moveParam.Speed).From().SetEase(_moveParam.Ease))
            .Append(_nameGroup.DOFade(1f, _nameFadeParam.Speed))
            .AppendInterval(_fadeInterval)
            .Append(_canvasGroup.DOFade(0f, _fadeParam.Speed).SetEase(_fadeParam.Ease));

        seq.Play();
        _currentSequence = seq;
    }
}
