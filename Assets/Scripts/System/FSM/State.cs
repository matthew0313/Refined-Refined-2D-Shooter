using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> where T : Component
{
    protected Layer<T> parentLayer;
    protected T origin;
    public State(Layer<T> parentLayer, T origin)
    {
        this.parentLayer = parentLayer;
        this.origin = origin;
    }
    public virtual void OnStateEnter()
    {

    }
    public virtual void OnStateUpdate()
    {

    }
    public virtual void OnStateFixedUpdate()
    {

    }
    public virtual void OnStateExit()
    {

    }
}
