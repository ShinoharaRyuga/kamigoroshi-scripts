using InputControl;

/// <summary>�X�e�[�g�J�ڂ̏��� </summary>
public struct TriggerConditions
{
    /// <summary>�ǂ̓��͂����ꂽ�� </summary>
    ActionType _triggerActionType;

    /// <summary>�g���K�[�t���O </summary>
    bool _trigger;

    #region �v���p�e�B
    /// <summary>���� </summary>
    ActionType TriggerActionType { get => _triggerActionType; }

    /// <summary>�g���K�[�t���O </summary>
    bool Trigger { get => _trigger; }
    #endregion

    public TriggerConditions(ActionType triggerAction, bool Trigger)
    {
        _triggerActionType = triggerAction;
        _trigger = Trigger;
    }

    /// <summary>�����𒲂ׂ� </summary>
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