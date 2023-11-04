using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// Repeat（繰り返し）
    /// 概要: 子ノードを繰り返し実行するデコレーターノードを表すクラスです。
    /// 機能: 子ノードを実行し、子ノードの実行結果が「実行中」の場合は「実行中」を返し、子ノードの実行結果が「失敗」の場合は「失敗」を返します。子ノードの実行結果が「成功」の場合は、restartOnSuccess フラグが true の場合は子ノードを再起動し、「実行中」を返し、restartOnSuccess フラグが false の場合は「成功」を返します。また、restartOnFailure フラグが true の場合は子ノードを再起動し、「実行中」を返し、restartOnFailure フラグが false の場合は「失敗」を返します。
    /// </summary>
    [System.Serializable]
    public class Repeat : DecoratorNode
    {
        // 成功時に子ノードを再起動するかどうかを示すフラグです。
        public bool restartOnSuccess = true;

        //失敗時に子ノードを再起動するかどうかを示すフラグ
        public bool restartOnFailure = false;

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
                    break;
                case State.Failure:
                    if (restartOnFailure)
                    {
                        return State.Running;
                    }
                    else
                    {
                        return State.Failure;
                    }
                case State.Success:
                    if (restartOnSuccess)
                    {
                        return State.Running;
                    }
                    else
                    {
                        return State.Success;
                    }
            }

            return State.Running;
        }
    }
}