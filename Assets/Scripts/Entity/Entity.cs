using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] float m_maxHp;
    [SerializeField] float kbResist = 1.0f;
    public float maxHp { get { return m_maxHp; } }
    [SerializeField] float m_hp;
    public float hp { get { return m_hp; } }
    public bool stunned { get; private set; } = false;
    protected bool invincible = false;
    protected bool kbImmune = false;
    protected virtual float kbStop { get; } = 2.0f;
    Rigidbody2D rb;
    private void OnEnable()
    {
        m_hp = maxHp;
    }
    protected virtual void Awake()
    {
        m_hp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0.0f;
            rb.mass = 0.0f;
            rb.angularDrag = 0.0f;
            rb.drag = kbResist;
            rb.freezeRotation = true;
        }
    }
    public virtual void GiveDamage(float damage)
    {
        if (hp == 0 || invincible) return;
        m_hp = Mathf.Max(0, m_hp - damage);
        if (hp == 0) Death();
    }
    public virtual void Death()
    {

    }
    public virtual void Heal(float healAmount)
    {
        if (hp == 0) return;
        m_hp = Mathf.Min(maxHp, hp + healAmount);
    }
    IEnumerator knocking = null;
    public virtual void GiveKnockback(Vector2 knockback)
    {
        if (invincible || kbImmune || rb==null || hp==0) return;
        rb.velocity = knockback;
        if (knocking == null)
        {
            knocking = Knocking();
            StartCoroutine(knocking);
        }
    }
    IEnumerator Knocking()
    {
        stunned = true;
        while(rb.velocity.magnitude > kbStop)
        {
            yield return null;
        }
        stunned = false;
        knocking = null;
    }
    List<string> immuneCausers = new List<string>();
    public void Immunity(string causer)
    {
        invincible = true;
        immuneCausers.Add(causer);
    }
    public void UnImmunity(string causer)
    {
        if (!immuneCausers.Contains(causer))
        {
            Debug.LogError(causer + " did not cause immunity");
            return;
        }
        immuneCausers.Remove(causer);
        if (immuneCausers.Count == 0) invincible = false;
    }
}
