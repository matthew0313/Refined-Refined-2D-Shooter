using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMovements_TopLayer_Disabled : State<PlayerMovements>
{
    public PlayerMovements_TopLayer_Disabled(Layer<PlayerMovements> parentLayer, PlayerMovements origin) : base(parentLayer, origin)
    {

    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if(origin.canMove == true && origin.origin.stunned == false)
        {
            parentLayer.SwitchState("Moving");
        }
    }
}
