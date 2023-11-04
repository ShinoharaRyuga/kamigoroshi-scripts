using InputControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//日本語対応
public class ExecuteGameOverProcess : MonoBehaviour
{
    [Header("時間切れまたは死亡によるゲームオーバー")]
    [SerializeField]
    private EventData _gameOverByTimeoutOrDiedData = default;

    public void GameOverByTimeoutOrDied()
    {
        GameManager.Instance.CurrentEndingType = EndingType.Bad;
        EventManager.Instance.EventStart(_gameOverByTimeoutOrDiedData);
    }
}
