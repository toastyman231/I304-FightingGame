using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected readonly PlayerStateMachine stateMachine;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void CalculateMoveDirection()
    {
        Vector3 moveDirection = Vector3.forward * stateMachine.input.actions["Movement"].ReadValue<float>();

        stateMachine.Velocity.z = moveDirection.z * stateMachine.MovementSpeed;
    }

    protected void ApplyGravity()
    {
        if (stateMachine.Velocity.y > Physics.gravity.y)
        {
            stateMachine.Velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    protected void Move()
    {
        stateMachine.controller.Move(stateMachine.Velocity * Time.deltaTime);
    }
}
