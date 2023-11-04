namespace PlayerState
{
    /// <summary>�X�e�[�g��J�ڂ�����N���X </summary>
    public class Transition
    {
        /// <summary>���̑J�ڐ� </summary>
        PlayerStateType _nextState = PlayerStateType.Idle;

        /// <summary>�J�ڂ���g���K�[ </summary>
        TriggerConditions _trigger = default;

        #region �v���p�e�B
        /// <summary>���̑J�ڐ� </summary>
        public PlayerStateType NextState { get => _nextState; set => _nextState = value; }

        /// <summary>�J�ڂ���g���K�[ </summary>
        public TriggerConditions Trigger { get => _trigger; set => _trigger = value; }
        #endregion
    }
}


