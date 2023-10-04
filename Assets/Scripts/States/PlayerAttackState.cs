using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Velocity.x = 0;
        stateMachine.anim.SetTrigger("Punch");
    }

    public override void Tick()
    {
        return;
    }

    public override void Exit()
    {
        return;
    }
}
