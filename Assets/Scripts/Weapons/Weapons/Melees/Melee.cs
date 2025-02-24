using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Melee : Weapon 
{
    [Header("Melee")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected Animator slashEffect;
    [SerializeField] protected Transform knockbackDirPoint;
    [SerializeField] protected Collider2D hitbox;
    [SerializeField] protected AudioClip slashSound;
    public override float gaugeFill { get { return 1.0f - counter / fireRate; } }
    protected virtual void Awake()
    {
        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }
    public override void AttemptFire()
    {
        base.AttemptFire();
        if(counter > fireRate)
        {
            counter = 0.0f;
            Fire();
        }
    }
    private void OnEnable()
    {
        anim.Play("Slash", -1, 1);
        if(slashEffect != null) slashEffect.Play("SlashEffect", -1, 1);
        hitbox.enabled = false;
    }
    public override void Fire()
    {
        base.Fire();
        anim.SetTrigger("Slash");
        hit.Clear();
        AudioSource tmp = audio;
        tmp.clip = slashSound;
        tmp.Play();
        if(slashEffect != null) slashEffect.SetTrigger("SlashEffect");
        hitbox.enabled = true;
    }
    public void EndSlash()
    {
        hitbox.enabled = false;
    }
    List<Collider2D> hit = new List<Collider2D>(); 
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit.Contains(collision)) return;
        hit.Add(collision);
        if (CheckReflection(collision)) return;
        if (enemy && collision.CompareTag("Player") || !enemy && collision.CompareTag("Enemy"))
        {
            Entity tmp = collision.GetComponent<Entity>();
            tmp.GiveDamage(damage);
            if (knockback > 0.0f) tmp.GiveKnockback(knockbackDirPoint.right.normalized * knockback);
        }
    }
    protected bool CheckReflection(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            collision.GetComponent<Bullet>().Reflect(enemy);
            return true;
        }
        return false;
    }
}
