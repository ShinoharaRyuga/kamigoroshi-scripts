namespace PlayerState
{
    /// <summary>ステートを遷移させるクラス </summary>
    public class Transition
    {
        /// <summary>次の遷移先 </summary>
        PlayerStateType _nextState = PlayerStateType.Idle;

        /// <summary>遷移するトリガー </summary>
        TriggerConditions _trigger = default;

        #region プロパティ
        /// <summary>次の遷移先 </summary>
        public PlayerStateType NextState { get => _nextState; set => _nextState = value; }

        /// <summary>遷移するトリガー </summary>
        public TriggerConditions Trigger { get => _trigger; set => _trigger = value; }
        #endregion
    }
}


