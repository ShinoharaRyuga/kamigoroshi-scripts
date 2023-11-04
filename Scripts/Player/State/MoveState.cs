using UnityEngine;

namespace PlayerState
{
    public class MoveState : State
    {
        Vector3 _moveDir = Vector3.zero;

        public MoveState(PlayerManager manager, PlayerStateMachineController playerStateMachine) : base(manager, playerStateMachine) { }
        public override void OnUpdate()
        {
            _moveDir = Vector3.forward * Player._vertical + Vector3.right * Player._horizontal;

            if (_moveDir == Vector3.zero)
            {
                Player.Rb.velocity = new Vector3(0f, Player.Rb.velocity.y, 0f);
            }
            else
            {
                Vector3 vector = Player.PlayerCamera.transform.TransformDirection(_moveDir);
                vector.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(vector);
                Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, targetRotation, Time.deltaTime * Player.RotationValue);

                Vector3 velo = vector.normalized * Player.MoveSpeed;
                velo.y = Player.Rb.velocity.y;

                Player.Rb.velocity = new Vector3(velo.x, Player.Rb.velocity.y, velo.z);
            }

            Player.AnimationController.MoveAnim(Player.Rb.velocity.magnitude);
            Player.EffectController.PlaySmokeEffect();
        }

        public override void OnEnter()
        {
            AudioManager.Instance.PlaySound(SoundType.SE, "SE_Run_Ground", 0.7f);
        }

        public override void OnExit()
        {
            Player.Rb.velocity = Vector3.zero;
            Player.AnimationController.MoveAnim(Player.Rb.velocity.magnitude);
            AudioManager.Instance.StopSound(SoundType.SE, "SE_Run_Ground");
        }
    }
}


