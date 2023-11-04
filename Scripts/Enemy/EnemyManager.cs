using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyController[] _enemyControllers;

    private Subject<Unit> _enemyDeactivationSubject = new Subject<Unit>();

    public IObservable<Unit> OnEnemyDeactivation => _enemyDeactivationSubject;

    private void Start()
    {
        foreach (var enemyController in _enemyControllers)
        {
            enemyController.OnDeactivation
                .TakeUntilDestroy(this) 
                .Subscribe(_ => CheckEnemyActive())
                .AddTo(enemyController);
        }

        CheckEnemyActive();
    }

    private void CheckEnemyActive()
    {
        if (_enemyControllers.All(e => !e.gameObject.activeSelf))
        {
            _enemyDeactivationSubject.OnNext(Unit.Default);
            Debug.Log("‘S–Å");
        }
    }

    public void StartQuest()
    {
        _enemyControllers.ToList().ForEach(e => e?.gameObject.SetActive(true));
    }
}