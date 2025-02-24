using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements_TopLayer : Layer<PlayerMovements>
{
    PlayerMovements_TopLayer_Disabled disabled;
    public PlayerMovements_TopLayer(PlayerMovements origin) : base(null, origin)
    {
        defaultState = new PlayerMovements_TopLayer_Moving(this, origin);
        AddState("Moving", defaultState);
        AddState("Dashing", new PlayerMovements_TopLayer_Dashing(this, origin));
        disabled = new PlayerMovements_TopLayer_Disabled(this, origin);
        AddState("Disabled", disabled);
        originalScaleY = origin.rotator.localScale.y;
    }
    float originalScaleY;
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if ((origin.canMove == false || origin.origin.stunned == true) && currentState != disabled)
        {
            SwitchState(disabled);
            return;
        }
        if (origin.canRotate)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dir = Mathf.Atan2(origin.rotator.position.y - pos.y, origin.rotator.position.x - pos.x) * Mathf.Rad2Deg;
            origin.rotator.rotation = Quaternion.Euler(0, 0, dir);
            if (dir >= 90.0f || dir <= -90.0f) origin.rotator.localScale = new Vector2(origin.rotator.localScale.x, -originalScaleY);
            else origin.rotator.localScale = new Vector2(origin.rotator.localScale.x, originalScaleY);
        }
    }
}
