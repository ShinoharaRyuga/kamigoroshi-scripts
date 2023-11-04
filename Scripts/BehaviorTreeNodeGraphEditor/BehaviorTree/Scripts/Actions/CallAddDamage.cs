using UnityEngine;
using BehaviorTreeNodeGraphEditor;
using UnityEngine.Serialization;

/// <summary>
/// 目的: 敵コントローラーのAddDamage()メソッドを呼び出すアクションノードです。
/// </summary>
[System.Serializable]
public class CallAddDamage : ActionNode
{
    public EnemyController _enemyController;
    public int _attackPower;

    protected override void OnStart()
    {
        if (context != null)
        {
            _enemyController = context.enemyController;
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (_enemyController == null)
        {
            return State.Failure;
        }

        // AddDamage()メソッドを呼び出す
        //_enemyController.OnAttack(_attackPower);

        return State.Success;
    }
}