using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNodeGraphEditor;

[System.Serializable]
public class AudioStop : ActionNode
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

        if (_enemyController._isSearching)
        {
            _enemyController.OnUntraceable();
            _enemyController._isSearching = false;
        }

        return State.Success;
    }
}