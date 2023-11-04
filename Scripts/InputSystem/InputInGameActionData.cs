using System;
using UnityEngine.InputSystem;

namespace InputControl
{
    public struct InputInGameActionData
    {
        /// <summary>登録時のid 削除時使用 </summary>
        int _id;

        /// <summary>どの処理(ボタン)を実行するか </summary>
        ActionType _actionType;

        /// <summary>実行タイミング </summary>
        InputActionPhase _actionPhase;

        /// <summary>実行される処理 </summary>
        Action _action;

        #region プロパティ
        /// <summary>登録時のid 削除時使用 </summary>
        public int ID => _id;

        /// <summary>どの処理(ボタン)を実行するか </summary>
        public ActionType ActionType { get => _actionType; }

        /// <summary>実行タイミング </summary>
        public InputActionPhase ActionPhase { get => _actionPhase; }

        /// <summary>実行される処理 </summary>
        public Action ExecuteAction { get => _action; }
        #endregion

        public InputInGameActionData(int id, ActionType uiActionType, InputActionPhase actionPhase, Action action)
        {
            _id = id;
            _actionType = uiActionType;
            _actionPhase = actionPhase;
            _action = action;
        }

        /// <summary>引数と比べて同じかどうか調べる </summary>
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