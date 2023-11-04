using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{

    /// <summary>
    /// 解放したいイベントの内容を記述
    /// </summary>
    public UniTask ReleaseEvent();
}
