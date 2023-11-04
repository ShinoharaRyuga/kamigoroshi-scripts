using InputControl;

/// <summary>ステート遷移の条件 </summary>
public struct TriggerConditions
{
    /// <summary>どの入力がされたか </summary>
    ActionType _triggerActionType;

    /// <summary>トリガーフラグ </summary>
    bool _trigger;

    #region プロパティ
    /// <summary>入力 </summary>
    ActionType TriggerActionType { get => _triggerActionType; }

    /// <summary>トリガーフラグ </summary>
    bool Trigger { get => _trigger; }
    #endregion

    public TriggerConditions(ActionType triggerAction, bool Trigger)
    {
        _triggerActionType = triggerAction;
        _trigger = Trigger;
    }

    /// <summary>条件を調べる </summary>
    public bool IsChack(ActionType Type, bool trigger)
    {
        if (Type == _triggerActionType && _trigger)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}