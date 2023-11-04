using InputControl;
using System.Collections.Generic;
using System.Linq;
using PlayerState;

public class PlayerStateMachine
{
    PlayerStateType _currentStateType = PlayerStateType.Idle;
    State _currentState;

    Dictionary<PlayerStateType, State> _stateTypes = new Dictionary<PlayerStateType, State>();

    /// <summary>ステートに遷移する条件を保持 </summary>
    Dictionary<PlayerStateType, List<Transition>> _transitionLists = new Dictionary<PlayerStateType, List<Transition>>();

    /// <summary>現在のステート </summary>
    public State CurrentState { get => _currentState;　}

    public PlayerStateMachine(PlayerStateType firstState, PlayerManager controller)
    {
        _stateTypes.Add(PlayerStateType.Idle, new IdleState(controller, controller.StateMachineController));
        _stateTypes.Add(PlayerStateType.Move, new MoveState(controller, controller.StateMachineController));
        _stateTypes.Add(PlayerStateType.Damage, new DamageState(controller, controller.StateMachineController));
        _stateTypes.Add(PlayerStateType.Attack, new AttackState(controller, controller.StateMachineController));
        _stateTypes.Add(PlayerStateType.Dead, new DeadState(controller, controller.StateMachineController));
        _stateTypes.Add(PlayerStateType.GetItem, new GetItemState(controller, controller.StateMachineController));
        _stateTypes.Add(PlayerStateType.DodgeAct, new DodgeActState(controller, controller.StateMachineController));

        InitState(firstState);
    }

    /// <summary>更新する </summary>
    public void StateUpdate()
    {
        _currentState.OnUpdate();
    }

    /// <summary> トリガーを実行する </summary>
    public void ExecuteTrigger(ActionType actionType, bool trigger)
    {
        var transitions = _transitionLists[_currentStateType];
        foreach (var transition in transitions)
        {
            if (transition.Trigger.IsChack(actionType, trigger))
            {
                ChangeState(transition.NextState);
                break;
            }
        }
    }

    /// <summary> </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="actionType"></param>
    /// <param name="trigger"></param>
    public void AddTransition(PlayerStateType from, PlayerStateType to, ActionType actionType, bool trigger)
    {
        if (!_transitionLists.ContainsKey(from))
        {
            _transitionLists.Add(from, new List<Transition>());
        }

        var transitions = _transitionLists[from];
        var transition = transitions.FirstOrDefault(x => x.NextState == to);
        var conditions = new TriggerConditions(actionType, trigger);

        if (transition == null)
        {
            transitions.Add(new Transition { NextState = to, Trigger = conditions }); //新規登録
        }
        else
        {
            transition.NextState = to;
            transition.Trigger = conditions;
        }
    }

    public void InitState(PlayerStateType stateType)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }

        _currentStateType = stateType;
        _currentState = _stateTypes[_currentStateType];
        _currentState.OnEnter();
    }

    /// <summary> Stateを直接変更する </summary>
    public void ChangeState(PlayerStateType stateType)
    {
        if (_currentStateType == stateType)
        {
            return;
        }
        if (_currentState != null)
        {
            _currentState.OnExit();
        }

        _currentStateType = stateType;
        _currentState = _stateTypes[_currentStateType];
        _currentState.OnEnter();
    }
}
