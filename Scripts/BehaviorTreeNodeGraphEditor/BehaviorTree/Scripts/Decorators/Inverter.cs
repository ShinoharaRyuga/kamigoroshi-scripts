using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// Inverter（反転）
    /// 概要: 子ノードの実行結果を反転するデコレーターノードを表すクラスです。
    /// 機能: 子ノードを実行し、子ノードの実行結果が「実行中」（State.Running）の場合は「実行中」を返し、子ノードの実行結果が「失敗」（State.Failure）の場合は「成功」（State.Success）を返し、子ノードの実行結果が「成功」の場合は「失敗」を返します。
    /// </summary>
    [System.Serializable]
    public class Inverter : DecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Success;
                case State.Success:
                    return State.Failure;
            }

            return State.Failure;
        }
    }
}