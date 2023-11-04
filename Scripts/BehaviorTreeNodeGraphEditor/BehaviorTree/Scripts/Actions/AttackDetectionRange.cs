using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNodeGraphEditor;

[System.Serializable]
public class AttackDetectionRange : ActionNode
{
    public float _range = 1f;
    public PlayerManager _playerManager;

    protected override void OnStart()
    {
        _range = context.enemyController.AttackRange;
        _playerManager = context.playerManager;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (_playerManager == null)
        {
            return State.Failure;
        }

        float playerDis = Vector3.Distance(context.transform.position, _playerManager.transform.position);

        if (playerDis <= _range)
        {
            return State.Success;
        }

        return State.Failure;
    }
}