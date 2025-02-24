using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Sword : Melee
{
    [Header("Sword")]
    [SerializeField] Transform firePoint;
    [SerializeField] protected GameObject flying_slash_projectile;
    [SerializeField] float flying_slash_damage, flying_slash_speed, flying_slash_range, flying_slash_cooldown;
    [SerializeField] Collider2D lungeHitbox;
    [SerializeField] float lunge_damage, lunge_speed, lunge_range, lunge_knockback, lunge_cooldown;

    public Sword()
    {
        hasAbility1 = true;
        ability1Name = "Flying Slash";
        hasAbility2 = true;
        ability2Name = "Lunge";
    }
    protected override void Awake()
    {
        base.Awake();
        lungeHitbox.enabled = false;
        lungeHitbox.isTrigger = true;
        a1_cooldown = flying_slash_cooldown;
        a2_cooldown = lunge_cooldown;
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
    bool lunging = false;
    Vector2 lungeDir = Vector2.zero;
    public override void Ability2()
    {
        base.Ability2();
        movements.DisableMovement();
        movements.DisableRotation();
        weapons.DisableSwap();
        weapons.DisableFire();
        lunging = true;
        hitbox.enabled = false;
        lungeHitbox.enabled = true;
        lungeDir = movements.rotator.transform.right.normalized * -1.0f;
        anim.SetBool("Lunging", true);
        entity.Immunity("Lunging");
        weapons.DisableAbility1();
        lungeCounter = 0.0f;
        AudioSource tmp = audio;
        tmp.clip = slashSound;
        tmp.Play();
        StartCoroutine(LungeDash());
    }
    float lungeCounter = 0.0f;
    IEnumerator LungeDash()
    {
        while(lungeCounter * lunge_speed < lunge_range)
        {
            movements.transform.Translate(lungeDir * lunge_speed * Time.deltaTime, Space.World);
            lungeCounter += Time.deltaTime;
            yield return null;
        }
        anim.SetBool("Lunging", false);
        lunging = false;
        lungeHitbox.enabled = false;
        movements.EnableMovement();
        movements.EnableRotation();
        weapons.EnableFire();
        weapons.EnableSwap();
        weapons.EnableAbility1();
        entity.UnImmunity("Lunging");
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!lunging)
        {
            base.OnTriggerEnter2D(collision);
            return;
        }
        if (CheckReflection(collision)) return;
        if (!enemy && collision.CompareTag("Enemy") || enemy && collision.CompareTag("Player"))
        {
            Entity tmp = collision.GetComponent<Entity>();
            tmp.GiveDamage(lunge_damage);
            if (lunge_knockback > 0.0f) tmp.GiveKnockback(knockbackDirPoint.right.normalized * knockback);
        }
    }
    public override string Describe()
    {
        return base.Describe() + "\n" +
            "Ab1: Flying Slash\nAb2: Lunge";
    }
}
