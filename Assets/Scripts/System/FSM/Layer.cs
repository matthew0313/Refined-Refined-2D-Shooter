using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Layer<T> : State<T> where T : Component
{
    protected State<T> defaultState;
    protected State<T> currentState;
    Dictionary<string, State<T>> states = new Dictionary<string, State<T>>();
    public Layer(Layer<T> parentLayer, T origin) : base(parentLayer, origin)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        currentState = defaultState;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        currentState.OnStateUpdate();
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        currentState.OnStateFixedUpdate();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        currentState.OnStateExit();
    }
    public void SwitchState(string stateName)
    {
        if (!states.ContainsKey(stateName))
        {
            Debug.LogError(stateName + " does not exist");
            return;
        }
        currentState.OnStateExit();
        currentState = states[stateName];
        currentState.OnStateEnter();
    }
    public void SwitchState(State<T> state)
    {
        if (currentState == state)
        {
            Debug.LogWarning(state + " is already the state.");
        }
        currentState.OnStateExit();
        currentState = state;
        currentState.OnStateEnter();
    }
    protected void AddState(string stateName, State<T> state)
    {
        states.Add(stateName, state);
    }
}
