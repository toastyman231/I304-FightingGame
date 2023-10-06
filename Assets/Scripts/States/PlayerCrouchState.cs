using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Velocity.y = Physics.gravity.y;
        stateMachine.anim.SetBool("IsCrouching", true);
    }

    public override void Tick()
    {
        if (!stateMachine.controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        CalculateMoveDirection();
        Move();
        stateMachine.anim.SetFloat("WalkDirection",
            stateMachine.input.actions["Movement"].ReadValue<float>() * stateMachine.FacingSign);
    }

    public override void Exit()
    {
        //stateMachine.anim.SetBool("IsCrouching", false);
    }
}
