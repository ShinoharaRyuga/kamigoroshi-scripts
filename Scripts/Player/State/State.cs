
namespace PlayerState
{
    public abstract class State
    {
        PlayerManager _playerManager = default;

        PlayerStateMachineController _stateController = default;

        protected PlayerManager Player => _playerManager;

        protected PlayerStateMachineController StateController => _stateController;

        public State(PlayerManager manager, PlayerStateMachineController playerStateMachine)
        {
            _playerManager = manager;
            _stateController = playerStateMachine;
        }

        public void Execute(Event evt)
        {
            if (_playerManager == null) { return; }

            switch (evt)
            {
                case Event.Enter:
                    OnEnter();
                    break;
                case Event.Update:
                    OnUpdate();
                    break;
                case Event.Exit:
                    OnExit();
                    break;
            }
        }

        public void SetStateController(PlayerManager player)
        {
            _playerManager = player;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();
    }

    public enum Event
    {
        Enter,
        Update,
        Exit,
    }
}


