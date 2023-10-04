using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Velocity.y = 0f;
        stateMachine.anim.SetBool("IsFalling", true);
    }

    public override void Tick()
    {
        ApplyGravity();
        Move();

        if (stateMachine.controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.anim.SetBool("IsFalling", false);
        stateMachine.anim.SetTrigger("Land");
    }
}
