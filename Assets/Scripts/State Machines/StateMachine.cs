using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State _currentState;

    public void SwitchState(State state)
    {
        _currentState?.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.Tick();
    }

    public State GetCurrentState()
    {
        return _currentState;
    }
}
