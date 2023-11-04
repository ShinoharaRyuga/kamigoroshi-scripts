using System;
using UnityEngine.InputSystem;

namespace InputControl
{
    /// <summary>UI操作での処理まとめた構造体 </summary>
    public struct InputUIActionData
    {
        /// <summary>登録時のid 削除時使用 </summary>
        int _id;
        /// <summary>UIの種類 </summary>
        ActionMaps _actionMap;

        /// <summary>どの処理(ボタン)を実行するか </summary>
        UIActionType _uiActionType;

        /// <summary>実行タイミング </summary>
        InputActionPhase _actionPhase;

        /// <summary>実行される処理 </summary>
        Action _action;

        #region プロパティ

        /// <summary>登録時のid 削除時使用 </summary>
        public int ID => _id; 

        /// <summary>UIの種類</summary>
        public ActionMaps ActionMap { get => _actionMap; }

        /// <summary>どの処理(ボタン)を実行するか </summary>
        public UIActionType ActionType { get => _uiActionType; }

        /// <summary>実行タイミング </summary>
        public InputActionPhase ActionPhase { get => _actionPhase; }

        /// <summary>実行される処理 </summary>
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

        /// <summary>引数と比べて同じかどうか調べる </summary>
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