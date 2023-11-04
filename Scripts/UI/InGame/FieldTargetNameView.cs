using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UnityEngine.UI; 

[RequireComponent(typeof(CanvasGroup))]
public class FieldTargetNameView : MonoBehaviour
{
    [SerializeField] RectTransform _fieldRect;
    [SerializeField] Image _sprite; 
    [Space(10)]
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _offset = new Vector2(0, 0.5f);
    [Space(10)]
    [SerializeField] DotParameter _param;

    Camera _mainCam;
    CanvasGroup _canvasGroup;
    Tween _tween;
    bool _active = false;

    private void Reset()
    {
        if (TryGetComponent(out _fieldRect))
        {
            var child = _fieldRect.GetChild(0);

            if (child)
            {
                child.TryGetComponent(out _sprite); 
            }
        }
    }

    private void Awake()
    {
        if (!_fieldRect || !_sprite) 
        {
            Reset();
        }

        _mainCam = Camera.main;
        TryGetComponent(out _canvasGroup);
        _canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        var e = EventManager.Instance;

        if (e)
        {
            e.NamePlateSetEvent.Subscribe(OnSwitchActive).AddTo(this);
        }

        var i = ItemManager.Instance;

        if (i)
        {
            i.NamePlateSetEvent.Subscribe(OnSwitchActive).AddTo(this);
        }
    }

    public void Initialize(Sprite sprite) 
    {
        _sprite.sprite = sprite;
    }

    private void LateUpdate()
    {
        if (!_target) return;
        if (!_active) return;
        if (_canvasGroup.alpha <= 0f) return;

        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_mainCam, _target.position + _offset);
        _fieldRect.position = screenPoint;
    }

    public void OnShow()
    {
        _active = true;

        SetAnimation(1f);
    }

    private void SetAnimation(float duration)
    {
        if (_tween != null)
        {
            _tween.Kill();
            _tween = null;
        }

        _tween = _canvasGroup.DOFade(duration, _param.Speed).SetEase(_param.Ease);
    }

    public void OnHide()
    {
        _active = false;

        SetAnimation(0f);
    }

    void OnSwitchActive(bool flag)
    {
        if (!flag)
        {
            if (_active)
            {
                OnShow();
            }
        }
        else
        {
            SetAnimation(0f);
        }
    }
}
