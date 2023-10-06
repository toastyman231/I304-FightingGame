using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private AttackInfo _attack;

    public PlayerAttackState(PlayerStateMachine stateMachine, AttackInfo attack) : base(stateMachine)
    {
        _attack = attack;
    }

    public override void Enter()
    {
        stateMachine.Velocity.x = 0;
        stateMachine.anim.SetTrigger(_attack.AttackTrigger);
    }

    public override void Tick()
    {
        return;
    }

    public override void Exit()
    {
        foreach (var playerCollisionDetection in stateMachine.GetComponentsInChildren<PlayerCollisionDetection>())
        {
            playerCollisionDetection.Reset();
        }

        return;
    }
}
