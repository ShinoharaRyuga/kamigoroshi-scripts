using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{

    /// <summary>
    /// ����������C�x���g�̓��e���L�q
    /// </summary>
    public UniTask ReleaseEvent();
}
