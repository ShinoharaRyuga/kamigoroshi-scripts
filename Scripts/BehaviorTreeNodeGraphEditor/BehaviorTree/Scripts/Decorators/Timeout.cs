using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// Timeout（タイムアウト）
    /// 概要: 実行時間の制限を持つデコレーターノードを表すクラスです。
    /// 機能: 子ノードを実行し、指定された duration の時間が経過した場合は「失敗」を返し、それ以外の場合は子ノードの実行結果を返します。OnStart メソッドでは開始時刻を記録し、OnUpdate メソッドで経過時間をチェックしています。
    /// </summary>
    [System.Serializable]
    public class Timeout : DecoratorNode
    {
        public float duration = 1.0f;
        float startTime;

        protected override void OnStart()
        {
            startTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Time.time - startTime > duration)
            {
                return State.Failure;
            }

            return child.Update();
        }
    }
}