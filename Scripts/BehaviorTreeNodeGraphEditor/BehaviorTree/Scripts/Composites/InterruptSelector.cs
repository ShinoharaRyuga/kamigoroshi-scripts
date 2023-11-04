using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNodeGraphEditor
{
    /// <summary>
    /// InterruptSelector（割り込みセレクター）
    /// 概要: 割り込みを処理するセレクターノードを表すクラスです。Selector クラスを継承しています。
    /// 機能: 子ノードを実行する前に、直前に実行していた子ノードが実行中だった場合に中断します。割り込みが発生した場合、直前に実行していた子ノードを中断（Abort）します。
    /// </summary>
    [System.Serializable]
    public class InterruptSelector : Selector
    {
        protected override State OnUpdate()
        {
            int previousIndex = current;
            base.OnStart();
            var status = base.OnUpdate();
            if (previousIndex != current)
            {
                if (children[previousIndex].state == State.Running)
                {
                    children[previousIndex].Abort();
                }
            }

            return status;
        }
    }
}