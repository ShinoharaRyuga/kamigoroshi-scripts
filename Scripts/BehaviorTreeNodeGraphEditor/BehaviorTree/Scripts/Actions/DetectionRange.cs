using UnityEngine;
using BehaviorTreeNodeGraphEditor;
using UnityEngine.Serialization;

/// <summary>
/// 目的: 範囲内に入ったときにプレイヤーコントローラーの位置をblackboard.moveToPositionに設定するアクションノードです。
/// </summary>
[System.Serializable]
public class DetectionRange : ActionNode
{
    public float _range = 5f;
    public PlayerManager _playerManager;

    /// <summary>
    /// ノードの開始時に呼ばれるメソッド
    /// </summary>
    protected override void OnStart()
    {
        _range = context.enemyController.TrackingRange;
        _playerManager = context.playerManager;
    }

    /// <summary>
    /// ノードの停止時に呼ばれるメソッド
    /// </summary>
    protected override void OnStop()
    {
    }

    /// <summary>
    /// ノードの更新時に呼ばれるメソッド
    /// </summary>
    /// <returns></returns>
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