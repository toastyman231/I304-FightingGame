using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State _currentState;

    public abstract void OnAnyStateEnter();
    public abstract void OnAnyStateTick();
    public abstract void OnAnyStateExit();

    public void SwitchState(State state)
    {
        _currentState?.Exit();
        if (_currentState != null) OnAnyStateExit();
        _currentState = state;
        _currentState.Enter();
        OnAnyStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.Tick();
        OnAnyStateTick();
    }

    public State GetCurrentState()
    {
        return _currentState;
    }
}
