using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>クエスト選択シーンの管理クラス </summary>
public class QuestSelectManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager _playerManager = default;
    [SerializeField]
    FadeController _fadeController = default;
    [SerializeField, Tooltip("会話イベント01のデータ")]
    EventData _talk1Data = default;

    private void Start()
    {
        CameraManager.Instance.SetFlag();

        PauseManager.Instance.ExecutePause(); // 開始時に会話が始まるまで動けないようにする

        _fadeController.FadeIn(() =>
        {
            PauseManager.Instance.ExecutePause();
            EventManager.Instance.EventStart(_talk1Data);
        });
    }
}
