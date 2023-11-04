using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// Sequencer（シーケンサー）
    /// 概要: 子ノードを順番に実行し、失敗するか最後まで実行した場合に成功するシーケンサーノードを表すクラスです。CompositeNode クラスを継承しています。
    /// 機能: 子ノードを順番に実行します。子ノードの実行結果が「実行中」の場合は「実行中」となり、実行を継続します。子ノードの実行結果が「失敗」の場合は「失敗」となり、Sequencer ノードも「失敗」となります。子ノードの実行結果が「成功」の場合は次の子ノードを実行します。すべての子ノードが「成功」した場合、Sequencer ノードも「成功」となります。
    /// </summary>
    [System.Serializable]
    public class Sequencer : CompositeNode
    {
        protected int current;

        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            for (int i = current; i < children.Count; ++i)
            {
                current = i;
                var child = children[current];

                switch (child.Update())
                {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        return State.Failure;
                    case State.Success:
                        continue;
                }
            }

            return State.Success;
        }
    }
}