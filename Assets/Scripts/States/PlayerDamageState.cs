using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageState : PlayerBaseState
{
    private DamageType _damageType;

    public PlayerDamageState(PlayerStateMachine stateMachine, DamageType damageType) : base(stateMachine)
    {
        _damageType = damageType;
    }

    public override void Enter()
    {
        switch (_damageType)
        {
            case DamageType.HEAD:
                stateMachine.anim.SetTrigger("HeadHit");
                break;
            case DamageType.BODY:
                stateMachine.anim.SetTrigger("BodyHit");
                break;
            case DamageType.LOWER_BODY:
                stateMachine.anim.SetTrigger("BodyHit");
                break;
            default:
                break;
        }
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
