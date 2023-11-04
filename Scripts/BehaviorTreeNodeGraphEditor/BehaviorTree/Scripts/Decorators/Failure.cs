using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// Failure（失敗）
    /// 概要: 失敗時に子ノードを実行するデコレーターノードを表すクラスです。
    /// 機能: 子ノードを実行し、その結果が成功（State.Success）だった場合は失敗（State.Failure）を返し、それ以外の場合は子ノードの実行結果を返します。
    /// </summary>
    [System.Serializable]
    public class Failure : DecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var state = child.Update();
            if (state == State.Success)
            {
                return State.Failure;
            }

            return state;
        }
    }
}