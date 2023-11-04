using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyHpPresenter : MonoBehaviour
{
    [SerializeField] EnemyHpView _view;
    [SerializeField] EnemyController _model;

    private void Awake()
    {
        var max = _model.MaxHP;
        _view.SetMaxValue(max);
        _model.CurrentHP.Subscribe(_view.SetValue).AddTo(this);
    }
}
