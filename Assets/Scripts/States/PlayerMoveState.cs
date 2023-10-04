using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Velocity.y = Physics.gravity.y;
        stateMachine.SwitchFacingDirection();
    }

    public override void Tick()
    {
        if (!stateMachine.controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        CalculateMoveDirection();
        Move();
        stateMachine.anim.SetFloat("WalkDirection", stateMachine.input.actions["Movement"].ReadValue<float>());
    }

    public override void Exit()
    {
        return;
    }
}
