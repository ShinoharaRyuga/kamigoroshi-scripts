using InputControl;
using UnityEngine;
using PlayerState;

/// <summary>ステートを操作するクラス </summary>
public class PlayerStateMachineController : MonoBehaviour
{
    [SerializeField, Header("ゲーム開始時の状態")]
    PlayerStateType _firstState = PlayerStateType.Idle;
    PlayerStateMachine _stateMachine;

    PlayerManager _playerManager = default;

    public PlayerStateMachine StateMachine { get => _stateMachine; }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (!_playerManager.IsPause)
        {
            if (PlayerInputs.Instance.GetInput(ActionType.Move))
            {
                if (!_playerManager.AnimationController.IsPlayAnim)
                {
                    _stateMachine.ChangeState(PlayerStateType.Move);
                }
                else
                {
                    _stateMachine.ExecuteTrigger(ActionType.Move, true);
                }
            }

            if (!PlayerInputs.Instance.IsInputting && !_playerManager.AnimationController.IsPlayAnim)
            {
                _stateMachine.ChangeState(PlayerStateType.Idle);
                AudioManager.Instance.StopSound(SoundType.SE, "SE_Run_Ground");
            }

            if (PlayerInputs.Instance.GetInputDown(ActionType.LightAttack) && !_playerManager.AnimationController.IsPlayAnim)
            {
                if (!_playerManager.AnimationController.IsCarryingKatana)
                {
                    return;
                }
                _stateMachine.ExecuteTrigger(ActionType.LightAttack, true);
            }

            if (PlayerInputs.Instance.GetInputDown(ActionType.Item))
            {
                if(_playerManager.PlayerGetItem.FieldItem == null)
                {
                    return;
                }
               _stateMachine.ExecuteTrigger(ActionType.Item, true);
            }

            if (PlayerInputs.Instance.GetInputDown(ActionType.DodgeAct))
            {
                if (!_playerManager.AnimationController.IsCarryingKatana)
                {
                    return;
                }
                _stateMachine.ExecuteTrigger(ActionType.DodgeAct, true);
            }

            _stateMachine.StateUpdate();
            // Debug.Log(_stateMachine.CurrentState);
        }
    }

    /// <summary>初期化処理 </summary>
    private void Initialize()
    {
        _playerManager = GetComponent<PlayerManager>();
        _stateMachine = new PlayerStateMachine(_firstState, _playerManager);

        _stateMachine.AddTransition(PlayerStateType.Idle, PlayerStateType.Move, ActionType.Move, true);
        _stateMachine.AddTransition(PlayerStateType.Move, PlayerStateType.Idle, ActionType.Move, false);

        _stateMachine.AddTransition(PlayerStateType.Move, PlayerStateType.Attack, ActionType.LightAttack, true);
        _stateMachine.AddTransition(PlayerStateType.Idle, PlayerStateType.Attack, ActionType.LightAttack, true);
        _stateMachine.AddTransition(PlayerStateType.Attack, PlayerStateType.Idle, ActionType.LightAttack, false);
        _stateMachine.AddTransition(PlayerStateType.Attack, PlayerStateType.Move, ActionType.LightAttack, false);

        _stateMachine.AddTransition(PlayerStateType.Idle, PlayerStateType.Damage, ActionType.None, true);
        _stateMachine.AddTransition(PlayerStateType.Damage, PlayerStateType.Idle, ActionType.None, false);
        _stateMachine.AddTransition(PlayerStateType.Damage, PlayerStateType.Attack, ActionType.LightAttack, true);

        _stateMachine.AddTransition(PlayerStateType.Idle, PlayerStateType.Dead, ActionType.None, false);
        _stateMachine.AddTransition(PlayerStateType.Damage, PlayerStateType.Dead, ActionType.None, false);

        _stateMachine.AddTransition(PlayerStateType.Idle, PlayerStateType.GetItem, ActionType.Item, true);
        _stateMachine.AddTransition(PlayerStateType.Move, PlayerStateType.GetItem, ActionType.Item, true);
        _stateMachine.AddTransition(PlayerStateType.GetItem, PlayerStateType.Move, ActionType.Move, true);
        _stateMachine.AddTransition(PlayerStateType.GetItem, PlayerStateType.Idle, ActionType.Item, false);

        _stateMachine.AddTransition(PlayerStateType.Move, PlayerStateType.DodgeAct, ActionType.DodgeAct, true);
        _stateMachine.AddTransition(PlayerStateType.DodgeAct, PlayerStateType.Move, ActionType.DodgeAct, false);
        _stateMachine.AddTransition(PlayerStateType.DodgeAct, PlayerStateType.Idle, ActionType.DodgeAct, false);
    }
}



