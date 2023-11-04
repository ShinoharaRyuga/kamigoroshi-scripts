using System;
using UnityEngine.InputSystem;

namespace InputControl
{
    /// <summary>UI����ł̏����܂Ƃ߂��\���� </summary>
    public struct InputUIActionData
    {
        /// <summary>�o�^����id �폜���g�p </summary>
        int _id;
        /// <summary>UI�̎�� </summary>
        ActionMaps _actionMap;

        /// <summary>�ǂ̏���(�{�^��)�����s���邩 </summary>
        UIActionType _uiActionType;

        /// <summary>���s�^�C�~���O </summary>
        InputActionPhase _actionPhase;

        /// <summary>���s����鏈�� </summary>
        Action _action;

        #region �v���p�e�B

        /// <summary>�o�^����id �폜���g�p </summary>
        public int ID => _id; 

        /// <summary>UI�̎��</summary>
        public ActionMaps ActionMap { get => _actionMap; }

        /// <summary>�ǂ̏���(�{�^��)�����s���邩 </summary>
        public UIActionType ActionType { get => _uiActionType; }

        /// <summary>���s�^�C�~���O </summary>
        public InputActionPhase ActionPhase { get => _actionPhase; }

        /// <summary>���s����鏈�� </summary>
        public Action ExecuteAction { get => _action; }
        #endregion

        public InputUIActionData(int id, ActionMaps actionMap, UIActionType uiActionType, InputActionPhase actionPhase, Action action)
        {
            _id = id;
            _actionMap = actionMap;
            _uiActionType = uiActionType;
            _actionPhase = actionPhase;
            _action = action;
        }

        /// <summary>�����Ɣ�ׂē������ǂ������ׂ� </summary>
        public bool IsMatch(ActionMaps actionMap, UIActionType uiActionType, InputActionPhase actionPhase)
        {
            if (_actionMap == actionMap && _uiActionType == uiActionType && _actionPhase == actionPhase)
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