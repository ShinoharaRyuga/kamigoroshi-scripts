using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// Parallel（並列）
    /// 概要: 並列処理を行うコンポジットノードを表すクラスです。CompositeNode クラスを継承しています。
    /// 機能: 子ノードを並列に実行します。子ノードはすべて同時に実行されます。各子ノードの実行結果が「失敗」の場合、Parallel ノードも「失敗」となります。子ノードの実行結果が「実行中」の場合は、Parallel ノードも「実行中」となります。すべての子ノードが「成功」を返した場合にのみ、Parallel ノードも「成功」となります。
    /// </summary>
    [System.Serializable]
    public class Parallel : CompositeNode
    {
        private List<State> childrenLeftToExecute = new List<State>();

        protected override void OnStart()
        {
            childrenLeftToExecute.Clear();
            children.ForEach(child => { childrenLeftToExecute.Add(State.Running); });
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            bool stillRunning = false;
            for (int i = 0; i < childrenLeftToExecute.Count; ++i)
            {
                if (childrenLeftToExecute[i] == State.Running)
                {
                    var status = children[i].Update();
                    if (status == State.Failure)
                    {
                        AbortRunningChildren();
                        return State.Failure;
                    }

                    if (status == State.Running)
                    {
                        stillRunning = true;
                    }

                    childrenLeftToExecute[i] = status;
                }
            }

            return stillRunning ? State.Running : State.Success;
        }

        private void AbortRunningChildren()
        {
            for (int i = 0; i < childrenLeftToExecute.Count; ++i)
            {
                if (childrenLeftToExecute[i] == State.Running)
                {
                    children[i].Abort();
                }
            }
        }
    }
}