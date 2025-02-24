using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMovements_TopLayer_Moving : State<PlayerMovements>
{
    public PlayerMovements_TopLayer_Moving(Layer<PlayerMovements> parentLayer, PlayerMovements origin) : base(parentLayer, origin)
    {
        
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        origin.transform.Translate(move * origin.moveSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.LeftShift) && origin.dashCounter >= origin.dashCooldown)
        {
            origin.dashCounter = 0;
            origin.SetDashDir(move.normalized);
            parentLayer.SwitchState("Dashing");
        }
    }
}
