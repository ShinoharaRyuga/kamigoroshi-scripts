using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Icon("Assets/Graphics/DataIcon/Event_Data.png")]
[CreateAssetMenu(fileName = "New EventData", menuName = "ScriptableObjects/EventData")]
public class EventData : ScriptableObject
{
    [SerializeField, Header("イベントの種類")] EventType _eventType;
    [SerializeField, Header("開始時遷移の種類")] EventTransitionType _startTransitionType = EventTransitionType.Fade;
    [SerializeField, Header("終了時遷移の種類")] EventTransitionType _finishTransitionType = EventTransitionType.Fade;
    [Space(10)]
    [SerializeReference, SubclassSelector] IEventAction[] _events;

    public IEventAction[] Events => _events;

    public EventType EventType => _eventType;
    public EventTransitionType StartTransitionType => _startTransitionType;
    public EventTransitionType FinishTransitionType => _finishTransitionType;
}

[System.Serializable]
public class SelectEvent
{
    [SerializeField] string _selectionName = "New Selection";
    [SerializeReference, SubclassSelector] IEventAction[] _selectEvents;

    public string SelectionName => _selectionName;
    public IEventAction[] SelectEvents => _selectEvents;
}

/// <summary>イベントの種類</summary>
public enum EventType
{
    /// <summary>会話イベント</summary>
    Talk,
    /// <summary>オブジェクトを調べるイベント</summary>
    Interact
}

public enum EventTransitionType
{
    Fade,       //フェードアウトして開始
    Seamless,   //特に何もせずに開始
}