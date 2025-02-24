using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Bullet : PooledPrefab
{
    protected float damage, range, speed, knockback;
    protected bool enemy, pierce, reflectable;
    float counter = 0.0f;
    TrailRenderer trail;
    Reflector reflector;
    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        reflector = GetComponent<Reflector>();
    }
    public void Set(float damage, float range, float speed, float knockback, bool enemy, bool pierce, bool reflectable)
    {
        this.damage = damage;
        this.range = range; 
        this.speed = speed;
        this.knockback = knockback;
        this.enemy = enemy;
        this.pierce = pierce;
        this.reflectable = reflectable;
        counter = 0.0f;
        reflected = false;
        if (trail != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
            trail.Clear();
        }
        if(reflector != null)
        {
            reflector.Set(enemy);
        }
    }
    private void Update()
    {
        if (counter * speed < range)
        {
            transform.Translate(transform.right * (reflected ? -1.0f : 1.0f) * speed * Time.deltaTime, Space.World);
            counter += Time.deltaTime;
        }
        else
        {
            gameObject.PoolDestroy();
        }
    }
    bool reflected = false;
    public void Reflect(bool enemy)
    {
        if (this.enemy == enemy) return;
        if (!reflectable) return;
        this.enemy = !this.enemy;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        reflected = !reflected;
        counter = 0.0f;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("BulletIgnore")) return;
        if (collision.CompareTag("Map"))
        {
            gameObject.PoolDestroy();
            return;
        }
        if (!enemy)
        {
            if (collision.CompareTag("Player")) return;
            if (collision.CompareTag("Enemy"))
            {
                Entity tmp = collision.GetComponent<Entity>();
                tmp.GiveDamage(damage);
                if (knockback > 0.0f) tmp.GiveKnockback(transform.right.normalized * knockback);
            }
        }
        else
        {
            if (collision.CompareTag("Enemy")) return;
            if (collision.CompareTag("Player"))
            {
                Entity tmp = collision.GetComponent<Entity>();
                tmp.GiveDamage(damage);
                if (knockback > 0.0f) tmp.GiveKnockback(transform.right.normalized * knockback);
            }
        }
        if (!pierce) gameObject.PoolDestroy();
    }
}
