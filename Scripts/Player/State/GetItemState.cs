
namespace PlayerState
{
    public class GetItemState : State
    {
        public GetItemState(PlayerManager manager, PlayerStateMachineController playerStateMachine) : base(manager, playerStateMachine)
        {
        }

        public override void OnEnter()
        {
            Player.PlayerGetItem.OnFuncCall();
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {

        }
    }

}

