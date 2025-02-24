using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMovements_TopLayer_Dashing : State<PlayerMovements>
{
    public PlayerMovements_TopLayer_Dashing(Layer<PlayerMovements> parentLayer, PlayerMovements origin) : base(parentLayer, origin)
    {
        
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        origin.origin.Immunity("Dash");
        counter = 0.0f;
        effectCounter = 0.0f;
    }
    float counter = 0.0f;
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if(counter * origin.dashSpeed < origin.dashRange)
        {
            origin.transform.Translate(origin.dashDir * origin.dashSpeed * Time.deltaTime);
            counter += Time.deltaTime;
        }
        else
        {
            parentLayer.SwitchState("Moving");
        }
    }
    float effectCounter = 0.0f;
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        if (effectCounter < origin.dashEffectRate) effectCounter += Time.fixedDeltaTime;
        else
        {
            effectCounter = 0.0f;
            origin.dashEffect.PoolCreate(origin.rotator.position, origin.rotator.rotation);
        }
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        origin.origin.UnImmunity("Dash");
    }
}
