using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNodeGraphEditor;
using UnityEngine.Serialization;

[System.Serializable]
public class MoveToPlayerPosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    
    public PlayerManager _playerManager;
    protected override void OnStart() 
    {
        _playerManager = context.playerManager;
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = context.enemyController.Speed;

        Vector3 offset = (_playerManager.transform.position - context.agent.transform.position).normalized * 0.5f; // プレイヤーから少し外れた位置を計算
        context.agent.destination = _playerManager.transform.position + offset;

        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            blackboard.moveToPlayerPosition = _playerManager.transform.position;
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Running;
    }
}
