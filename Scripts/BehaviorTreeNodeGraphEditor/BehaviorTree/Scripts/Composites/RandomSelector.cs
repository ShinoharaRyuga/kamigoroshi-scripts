using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// RandomSelector（ランダムセレクター）
    /// 概要: ランダムに子ノードを選択するセレクターノードを表すクラスです。CompositeNode クラスを継承しています。
    /// 機能: 子ノードのうち、ランダムに1つの子ノードを選択して実行します。選択された子ノードの実行結果が返されます。
    /// </summary>
    [System.Serializable]
    public class RandomSelector : CompositeNode
    {
        private int current;

        protected override void OnStart()
        {
            current = Random.Range(0, children.Count);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var child = children[current];
            return child.Update();
        }
    }
}