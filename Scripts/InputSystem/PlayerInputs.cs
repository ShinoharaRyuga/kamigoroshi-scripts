using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputControl
{
    /// <summary>
    /// 入力を管理するクラス 
    /// </summary>
    public class PlayerInputs : MonoBehaviour, GameInputs.IInGameActions, GameInputs.IUIActions, GameInputs.INoReceiveActions, GameInputs.IEventsActions, IManager
    {
        #region シングルトン
        static PlayerInputs _instance = null;

        public static PlayerInputs Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("PlayerInputs");
                    _instance = obj.AddComponent<PlayerInputs>();
                    DontDestroyOnLoad(obj);
                }

                return _instance;
            }
        }
        #endregion 

        /// <summary>左スティック 入力 
        /// <para>UI操作時はNavigateの値が入る</para>
        /// </summary>
        Vector2 _inputDirection = Vector2.zero;
        /// <summary>右スティック 入力 </summary>
        Vector2 _cameraInput = Vector2.zero;
        /// <summary>マウスポインターの位置 </summary>
        Vector2 _pointerPosition = Vector2.zero;
        /// <summary>マウスホイールの移動量</summary>
        Vector2 _scrollWheel = Vector2.zero;

        /// <summary>適用されているアクションマップ </summary>
        ActionMaps _currentActionMap = ActionMaps.InGame;

        /// <summary>入力されているかどうか </summary>
        bool _isInputting = false;

        /// <summary>最後に入力されたActionType</summary>
        ActionType _lastInput = ActionType.Submit;
        GameInputs _gameInputs = null;

        private int _currentInputID = 0;

        /// <summary>どのボタンが押されているか </summary>
        Dictionary<ActionType, bool> _enterInputs = new Dictionary<ActionType, bool>();

        List<ActionType> _frameInputs = new List<ActionType>();     
      
        /// <summary>インゲームの処理をまとめたリスト </summary>
        List<InputInGameActionData> _inGameInputs = new List<InputInGameActionData>();

        /// <summary>UIでの処理をまとめたリスト </summary>
        List<InputUIActionData> _uiInputs = new List<InputUIActionData>();

        #region プロパティ
        /// <summary>左スティック 入力 </summary>
        public Vector2 InputDirection
        {
            get
            {
                if (_instance == null)
                {
                    return Vector2.zero;
                }

                return _instance._inputDirection;
            }
        }

        /// <summary>右スティック 入力 </summary>
        public Vector2 CameraInput
        {
            get
            {
                if (_instance == null)
                {
                    return Vector2.zero;
                }

                return _instance._cameraInput;
            }
        }

        /// <summary>マウスポインターの位置 </summary>
        public Vector2 PointerPosition
        {
            get
            {
                if (_instance == null)
                {
                    return Vector2.zero;
                }

                return _instance._pointerPosition;
            }
        }

        /// <summary>マウスホイールの移動量</summary>
        public Vector2 ScrollWheel
        {
            get
            {
                if (_instance == null)
                {
                    return Vector2.zero;
                }

                return _instance._scrollWheel;
            }
        }

        public InputBinding? bindingMask { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ReadOnlyArray<InputDevice>? devices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReadOnlyArray<InputControlScheme> controlSchemes => throw new NotImplementedException();

        /// <summary>適用されているActionMap </summary>
        public ActionMaps CurrentActionMap { get => _currentActionMap; }

        /// <summary>最後の入力 </summary>
        public ActionType LastInput { get => _lastInput; }
        /// <summary>入力されているかどうか </summary>
        public bool IsInputting { get => _isInputting; set => _isInputting = value; }
        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (_inputDirection == Vector2.zero)
            {
                _enterInputs[ActionType.Move] = false;
            }
            else
            {
                _enterInputs[ActionType.Move] = true;
            }

            //if (_cameraInput == Vector2.zero)
            //{
            //    _enterInputs[ActionType.Camera] = false;
            //}
            //else
            //{
            //    _enterInputs[ActionType.Camera] = true;
            //}

            _isInputting = ChackInputting();
        }

        private void LateUpdate()
        {
            _frameInputs.Clear();
        }

        /// <summary>入力がされているボタンを取得する </summary>
        /// <returns>押されているボタンのリスト</returns>
        public List<ActionType> GetEnterInput()
        {
            var enterInput = new List<ActionType>();

            foreach (var input in _enterInputs)
            {
                if (input.Value)
                {
                    enterInput.Add(input.Key);
                }
            }

            return enterInput;
        }

        /// <summary>ボタンが押されているかどうか 長押しでも押されている判定になる </summary>
        public bool GetInput(ActionType actionType)
        {
            return _enterInputs[actionType];
        }

        public bool GetInputDown(ActionType actionType)
        {
            while (true)
            {
                if (_frameInputs.Contains(actionType))
                {
                    return true;
                }

                return false;
            };
        }


        /// <summary> アクションマップを切り替える  </summary>
        /// <param name="nextActionMap">次のActionMap</param>
        public void ChangeActionMap(ActionMaps nextActionMap)
        {
            switch (_currentActionMap)  //適用されているActionMapを無効にする
            {
                case ActionMaps.InGame:
                    _gameInputs.InGame.Disable();
                    break;
                case ActionMaps.UI:
                    _gameInputs.UI.Disable();
                    break;
                case ActionMaps.NoReceive:
                    _gameInputs.NoReceive.Disable();
                    break;
                case ActionMaps.Events:
                    _gameInputs.Events.Disable();
                    break;
            }

            _currentActionMap = nextActionMap;

            switch (nextActionMap)  //新しいActionMapを適用
            {
                case ActionMaps.InGame:
                    _gameInputs.InGame.Enable();
                    break;
                case ActionMaps.UI:
                    _gameInputs.UI.Enable();
                    break;
                case ActionMaps.NoReceive:
                    _gameInputs.NoReceive.Enable();
                    break;
                case ActionMaps.Events:
                    _gameInputs.Events.Enable();
                    break;
            }
        }

        #region 登録処理
        /// <summary>UI操作時の処理を追加する
        /// <para>ActionMapのUIに追加されます</para> 
        /// </summary>
        /// <param name="uiActionType">対応ボタン</param>
        /// <param name="actionPhase">実行タイミング</param>
        /// <param name="action">実行時処理</param>
        public int AddUIAction(UIActionType uiActionType, InputActionPhase actionPhase, Action action)
        {
            var id = _uiInputs.Count;
            var addUIAction = new InputUIActionData(id, ActionMaps.UI, uiActionType, actionPhase, action);
            _uiInputs.Add(addUIAction);

            return id;
        }

        /// <summary>
        /// UI操作時の処理を追加する 
        /// <para>UI以外のActionMapに追加したい時にこのメソッドを使用してください</para>
        /// <para>参照: <see cref="Reference.ReferenceInput"/></para>
        /// </summary>
        /// <param name="actionMap">どのActionMap</param>
        /// <param name="uiActionType">どの機能（ボタン）</param>
        /// <param name="actionPhase">実行されるタイミング</param>
        /// <param name="action">実行処理</param>
        public int AddUIAction(ActionMaps actionMap, UIActionType uiActionType, InputActionPhase actionPhase, Action action)
        {
            var id = _uiInputs.Count;
            var addUIAction = new InputUIActionData(id, actionMap, uiActionType, actionPhase, action);
            _uiInputs.Add(addUIAction);

            return id;
        }

        public int AddInGameAction(ActionType actionType, InputActionPhase actionPhase, Action action)
        {
            var id = _currentInputID;
            var addInGameAction = new InputInGameActionData(id, actionType, actionPhase, action);
            _inGameInputs.Add(addInGameAction);
            _currentInputID++;
         
            return id;
        }
        #endregion

        #region 登録解除処理
        /// <summary>登録した入力処理を破棄する </summary>
        /// <param name="id">登録時のid</param>
        public void RemoveUIAction(int id)
        {
            for (var i = 0; i < _uiInputs.Count; i++)
            {
                if (_uiInputs[i].ID == id)
                {
                    _uiInputs.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>登録した入力処理を破棄する </summary>
        /// <param name="id">登録時のid</param>
        public void RemoveInGameAction(int id)
        {
            for (var i = 0; i < _inGameInputs.Count; i++)
            {
                if (_inGameInputs[i].ID == id)
                {
                    _inGameInputs.RemoveAt(i);
                    break;
                }
            }
        }
        #endregion

        #region インゲームのメソッド
        public void OnMove(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
            _lastInput = ActionType.Move;
        }

        public void OnCamera(InputAction.CallbackContext context)
        {
            _cameraInput = context.ReadValue<Vector2>();
            _lastInput = ActionType.Camera;
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (_currentActionMap == ActionMaps.InGame)
            {
                ExecuteInGameInputAction(ActionType.Pause, context);
            }
            else
            {
                ExecuteUIAction(UIActionType.Pause, context);
            }

            _lastInput = ActionType.Pause;
        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {
            ExecuteInGameInputAction(ActionType.LightAttack, context);
            _lastInput = ActionType.LightAttack;
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            ExecuteInGameInputAction(ActionType.HeavyAttack, context);
            _lastInput = ActionType.HeavyAttack;
        }

        public void OnSpecialAttack01(InputAction.CallbackContext context)
        {
            ExecuteInGameInputAction(ActionType.SpecialAttack01, context);
            _lastInput = ActionType.SpecialAttack01;
        }

        public void OnSpecialAttack02(InputAction.CallbackContext context)
        {
            ExecuteInGameInputAction(ActionType.SpecialAttack02, context);
            _lastInput = ActionType.SpecialAttack02;
        }

        public void OnSpecialAttack03(InputAction.CallbackContext context)
        {
            ExecuteInGameInputAction(ActionType.SpecialAttack03, context);
            _lastInput = ActionType.SpecialAttack03;
        }

        public void OnDodgeAct(InputAction.CallbackContext context)
        {
            ExecuteInGameInputAction(ActionType.DodgeAct, context);
            _lastInput = ActionType.DodgeAct;
        }

        public void OnItem(InputAction.CallbackContext context)
        {
            ExecuteInGameInputAction(ActionType.Item, context);
            _lastInput = ActionType.Item;
        }

        public void OnLockOn(InputAction.CallbackContext context)
        {
            ExecuteInGameInputAction(ActionType.LockOn, context);
            _lastInput = ActionType.LockOn;
        }

        #endregion

        #region UIのメソッド
        public void OnNavigate(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            ExecuteUIAction(UIActionType.Cancel, context);
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (_currentActionMap == ActionMaps.InGame)
            {
                ExecuteInGameInputAction(ActionType.Submit, context);
            }
            else
            {
                ExecuteUIAction(UIActionType.Submit, context);
            }

            _lastInput = ActionType.Submit;
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            _pointerPosition = context.ReadValue<Vector2>();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            ExecuteUIAction(UIActionType.Click, context);
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            _scrollWheel = context.ReadValue<Vector2>();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            ExecuteUIAction(UIActionType.MiddleClick, context);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            ExecuteUIAction(UIActionType.RightClick, context);
        }

        #endregion

        /// <summary>入力処理を行う </summary>
        void ExecuteInGameInputAction(ActionType actionType, InputAction.CallbackContext context)
        {
            foreach (var input in _inGameInputs)
            {
                if (input.IsMatch(actionType, context.phase))
                {
                    input.ExecuteAction.Invoke();
                }
            }

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _enterInputs[actionType] = true;
                    _frameInputs.Add(actionType);
                    break;
                case InputActionPhase.Canceled:
                    _enterInputs[actionType] = false;
                    break;
            }
        }

        /// <summary><see cref="_uiInputs"/>に追加されていて条件にあったものを実行する </summary>
        /// <param name="uiActionType"></param>
        /// <param name="context"></param>
        void ExecuteUIAction(UIActionType uiActionType, InputAction.CallbackContext context)
        {
            foreach (var input in _uiInputs)
            {
                if (input.IsMatch(_currentActionMap, uiActionType, context.phase))
                {
                    input.ExecuteAction.Invoke();
                }
            }
        }

        /// <summary>初期化処理 </summary>
        void Initialize()
        {
            _gameInputs = new GameInputs();
            _gameInputs.InGame.SetCallbacks(this);
            _gameInputs.UI.SetCallbacks(this);
            _gameInputs.NoReceive.SetCallbacks(this);
            _gameInputs.Events.SetCallbacks(this);
            ChangeActionMap(_currentActionMap);

            for (var i = 0; i < Enum.GetValues(typeof(ActionType)).Length; i++)
            {
                _enterInputs.Add((ActionType)i, false);
            }
        }

        /// <summary>入力されているかどうか </summary>
        bool ChackInputting()
        {
            foreach (var input in _enterInputs)
            {
                if (input.Value)
                {
                    return true;
                }
            }

            return false;
        }

        public void DestroyObject()
        {
            _instance = null;
            Destroy(this.gameObject);
        }
    }
}

