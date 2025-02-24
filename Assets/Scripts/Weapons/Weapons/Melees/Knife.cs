using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Knife : Melee
{
    [Header("Knife")]
    [SerializeField] Transform firePoint;
    [SerializeField] protected GameObject flying_slash_projectile;
    [SerializeField] float flying_slash_damage, flying_slash_speed, flying_slash_range, flying_slash_cooldown;

    public Knife()
    {
        hasAbility1 = true;
        ability1Name = "Flying Slash";
    }
    protected override void Awake()
    {
        base.Awake();
        a1_cooldown = flying_slash_cooldown;
    }
    public override void Ability1()
    {
        base.Ability1();
        counter = 0.0f;
        anim.SetTrigger("Slash");
        AudioSource tmp2 = audio;
        tmp2.clip = slashSound;
        tmp2.Play();
        Bullet tmp = flying_slash_projectile.PoolCreate(firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        tmp.Set(flying_slash_damage, flying_slash_range, flying_slash_speed, 0.0f, enemy, true, false);
    }
    public override string Describe()
    {
        return base.Describe() + "\n" + 
            "Ab1: Flying Slash";
    }
}
