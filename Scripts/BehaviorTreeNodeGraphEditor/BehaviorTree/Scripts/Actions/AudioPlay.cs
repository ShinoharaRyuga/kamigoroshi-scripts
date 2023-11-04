using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNodeGraphEditor;

[System.Serializable]
public class AudioPlay : ActionNode
{
    public EnemyController _enemyController;
    public AudioManager _audioManager;

    protected override void OnStart()
    {
        _enemyController = context.enemyController;
        _audioManager = context.audioManager;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (_audioManager == null)
        {
            return State.Failure;
        }

        if (!_enemyController._isSearching)
        {
            _enemyController.OnFight();
            _enemyController._isSearching = true;
        }

        return State.Success;
    }
}