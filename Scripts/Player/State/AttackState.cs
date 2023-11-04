using UnityEngine;
using System;
using InputControl;

namespace PlayerState
{
    public class AttackState : State
    {
        public AttackState(PlayerManager manager, PlayerStateMachineController playerStateMachine) : base(manager, playerStateMachine) { }

        public override void OnUpdate()
        {
            if (PlayerInputs.Instance.GetInputDown(ActionType.LightAttack))
            {
                Player.AnimationController.StartLightAttackAnim();
            }
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {

        }
    }
}

