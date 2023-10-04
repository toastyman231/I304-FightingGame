using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Velocity = new Vector3(0, stateMachine.JumpForce, stateMachine.Velocity.z);
        stateMachine.anim.SetTrigger("Jump");
    }

    public override void Tick()
    {
        ApplyGravity();

        if (stateMachine.Velocity.y <= 0f)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        Move();
    }

    public override void Exit()
    {
        return;
    }
}
