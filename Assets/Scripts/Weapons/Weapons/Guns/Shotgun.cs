using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [Header("Shotgun")]
    [SerializeField] int shotCount = 1;
    protected override void FireBullet()
    {
        for(int i = 0; i < shotCount; i++)
        {
            base.FireBullet();
        }
    }
    public override string Describe()
    {
        return base.Describe() + "\n" + 
            "Shot count: " + shotCount;
    }
}
