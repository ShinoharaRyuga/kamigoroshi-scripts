using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// Succeed（成功）
    /// 概要: 子ノードが失敗した場合に成功として扱うデコレーターノードを表すクラスです。
    /// 機能: 子ノードを実行し、子ノードの実行結果が「失敗」の場合は「成功」を返し、それ以外の場合は子ノードの実行結果を返します。
    /// </summary>
    [System.Serializable]
    public class Succeed : DecoratorNode
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
            if (state == State.Failure)
            {
                return State.Success;
            }

            return state;
        }
    }
}