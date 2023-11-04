using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// Selector（セレクター）
    /// 概要: 子ノードを順番に実行し、成功するか最後まで実行した場合に失敗するセレクターノードを表すクラスです。CompositeNode クラスを継承しています。
    /// 機能: 子ノードを順番に実行します。子ノードの実行結果が「実行中」の場合は「実行中」となり、実行を継続します。子ノードの実行結果が「成功」の場合は「成功」となり、Selector ノードも「成功」となります。子ノードの実行結果が「失敗」の場合は次の子ノードを実行します。すべての子ノードが「失敗」した場合、Selector ノードも「失敗」となります。
    /// </summary>
    [System.Serializable]
    public class Selector : CompositeNode
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
                    case State.Success:
                        return State.Success;
                    case State.Failure:
                        continue;
                }
            }

            return State.Failure;
        }
    }
}