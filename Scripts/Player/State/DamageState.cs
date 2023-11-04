
namespace PlayerState
{
    public class DamageState : State
    {
        public DamageState(PlayerManager manager, PlayerStateMachineController playerStateMachine) : base(manager, playerStateMachine) { }

        public override void OnUpdate()
        {

        }

        public override void OnEnter()
        {
            Player.AnimationController.StartDamageAnim();
        }

        public override void OnExit()
        {

        }
    }
}
