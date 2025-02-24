using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public abstract class Weapon : PooledPrefab
{
    [Header("Weapon")]
    [SerializeField] string m_weaponName;
    [SerializeField] Sprite m_image;
    [SerializeField] float m_damage;
    [SerializeField] float m_fireRate, m_knockback;
    [SerializeField] bool m_auto = false;
    public string weaponName { get { return m_weaponName; } }
    public Sprite image { get { return m_image; } }
    public float damage { get { return m_damage; } }
    public float fireRate { get { return m_fireRate; } }
    public float knockback { get { return m_knockback; } }
    public bool auto { get { return m_auto; } }
    protected float counter = 0.0f;
    protected MonoBehaviour user;
    public bool enemy { get; private set; } = false;

    protected Entity entity;
    protected WeaponController weapons;
    protected MovementController movements;

    List<AudioSource> m_audio = new List<AudioSource>();

    const float playerVolume = 1.0f;
    const float enemyVolume = 0.5f;
    protected AudioSource audio { get
        {
            foreach(var i in m_audio)
            {
                if (i.isPlaying == false) return i;
            }
            AudioSource tmp = transform.AddComponent<AudioSource>();
            tmp.volume = (enemy) ? enemyVolume : playerVolume;
            tmp.playOnAwake = false;
            m_audio.Add(tmp);
            return tmp;
        } }
    public virtual void Set(MonoBehaviour user, bool enemy)
    {
        this.user = user;
        this.enemy = enemy;
        movements = user.GetComponent<MovementController>();
        weapons = user.GetComponent<WeaponController>();
        entity = user.GetComponent<Entity>();
        counter = 0.0f;
        user.StartCoroutine(OnUpdate());
    }
    protected virtual IEnumerator OnUpdate()
    {
        while (true)
        {
            if (counter < m_fireRate) counter += Time.deltaTime;
            if (hasAbility1 && a1_counter < a1_cooldown) a1_counter += Time.deltaTime;
            if (hasAbility2 && a2_counter < a2_cooldown) a2_counter += Time.deltaTime;
            yield return null;
        }
    }
    /*public virtual void OnUpdate()
    {
        if (auto && Input.GetMouseButton(0) || !auto && Input.GetMouseButtonDown(0))
        {
            if(counter >= m_fireRate)
            {
                counter = 0.0f;
                Fire();
            }
        }
    }*/
    public bool hasAbility1 { get; protected set; } = false;
    public string ability1Name { get; protected set; }
    public float a1_cooldown { get; protected set; }
    public float a1_counter { get; private set; }
    public bool hasAbility2 { get; protected set; } = false;
    public string ability2Name { get; protected set; }
    public float a2_cooldown { get; protected set; }
    public float a2_counter { get; private set; }
    public virtual float gaugeFill { get; }
    public virtual void AttemptAbility1()
    {
        if(hasAbility1 && a1_counter >= a1_cooldown)
        {
            a1_counter = 0.0f;
            Ability1();
        }
    }
    public virtual void Ability1()
    {

    }
    public virtual void AttemptAbility2()
    {
        if(hasAbility2 && a2_counter >= a2_cooldown)
        {
            a2_counter = 0.0f;
            Ability2();
        }
    }
    public virtual void Ability2()
    {

    }
    public virtual void AttemptFire()
    {
        
    }
    public virtual void Fire()
    {

    }
    public virtual void AttemptReload()
    {

    }
    public virtual string Describe()
    {
        return "Damage: " + damage + 
            "\n" + "FireRate: " + fireRate + 
            ((knockback > 0.0f) ? "\nKnockback: " + knockback : "");
    }
    public virtual string AmmoDesc()
    {
        return "--/--";
    }
}
