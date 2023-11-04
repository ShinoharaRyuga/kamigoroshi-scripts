using System;
using UnityEngine.InputSystem;

namespace InputControl
{
    public struct InputInGameActionData
    {
        /// <summary>�o�^����id �폜���g�p </summary>
        int _id;

        /// <summary>�ǂ̏���(�{�^��)�����s���邩 </summary>
        ActionType _actionType;

        /// <summary>���s�^�C�~���O </summary>
        InputActionPhase _actionPhase;

        /// <summary>���s����鏈�� </summary>
        Action _action;

        #region �v���p�e�B
        /// <summary>�o�^����id �폜���g�p </summary>
        public int ID => _id;

        /// <summary>�ǂ̏���(�{�^��)�����s���邩 </summary>
        public ActionType ActionType { get => _actionType; }

        /// <summary>���s�^�C�~���O </summary>
        public InputActionPhase ActionPhase { get => _actionPhase; }

        /// <summary>���s����鏈�� </summary>
        public Action ExecuteAction { get => _action; }
        #endregion

        public InputInGameActionData(int id, ActionType uiActionType, InputActionPhase actionPhase, Action action)
        {
            _id = id;
            _actionType = uiActionType;
            _actionPhase = actionPhase;
            _action = action;
        }

        /// <summary>�����Ɣ�ׂē������ǂ������ׂ� </summary>
        public bool IsMatch(ActionType uiActionType, InputActionPhase actionPhase)
        {
            if (_actionType == uiActionType && _actionPhase == actionPhase)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}