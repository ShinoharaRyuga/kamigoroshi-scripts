
using Unity.Mathematics;

namespace PlayerState
{
    public class DeadState : State
    {
        public DeadState(PlayerManager manager, PlayerStateMachineController playerStateMachine) : base(manager, playerStateMachine) { }

        public override void OnEnter()
        {
            Player.AnimationController.StartDeadAnim();
            Player.ExecuteGameOverProcess.GameOverByTimeoutOrDied();
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {

        }
    }
}

