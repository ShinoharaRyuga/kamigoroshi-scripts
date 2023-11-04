using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputControl;

namespace PlayerState
{
    public class DodgeActState : State
    {
        private Vector3 _playerDir = Vector3.zero;
        private float _time = 0f;
        public DodgeActState(PlayerManager manager, PlayerStateMachineController playerStateMachine) : base(manager, playerStateMachine) { }

        public override void OnEnter()
        {
            _time = 0f;
            Player.IsInvicible = true;
            _playerDir = Vector3.forward * Player._vertical + Vector3.right * Player._horizontal;
            Player.AnimationController.StartDodgeActAnim(_playerDir.x);
        }
        public override void OnUpdate()
        {
            _time += Time.deltaTime;
            DodgeAct(_time);
            if (_time < Player.ParamData.InvincibleTime)
            {
                Player.IsInvicible = true;
            }
            else if(_time >= Player.ParamData.InvincibleTime && _time <= Player.ParamData.InvincibleTime * 1.1f)
            {
                Player.IsInvicible = false;
            }
            else
            {
                Player.Rb.velocity = Vector3.zero;
            }
        }

        public override void OnExit()
        {
            Player.IsInvicible = false;
            _time = 0f;
        }

        private void DodgeAct(float time)
        {
            Vector3 vector = Player.PlayerCamera.transform.TransformDirection(_playerDir);
            vector.y = 0;

            Vector3 velo = vector.normalized * Player.ParamData.DodgeSpeed / (time * Player.ParamData.VelocityCorrectionValue);
            Player.Rb.velocity = velo;
        }
        
    }
}

