using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Gun : Weapon 
{
    [Header("Gun")]
    [SerializeField] protected int m_magSize;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float range, bulletSpeed, spread;
    [SerializeField] protected bool bulletPierce = false, reflectable = true;
    [SerializeField] protected float reloadTime;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform[] firePoints;
    [SerializeField] Animator fireAnim;
    [SerializeField] AudioClip fireSound, reloadSound;
    public int magSize { get { return m_magSize; } }
    public int currentMag { get; private set; }
    public int currentAmmo { get; private set; }
    public override void Set(MonoBehaviour user, bool enemy)
    {
        base.Set(user, enemy);
        currentMag = magSize;
        currentAmmo = maxAmmo;
    }
    public override void AttemptFire()
    {
        base.AttemptFire();
        if (counter > fireRate && currentMag > 0 && reloading == null)
        {
            counter = 0.0f;
            Fire();
        }
    }
    public override void Fire()
    {
        if (fireAnim != null) fireAnim.SetTrigger("Fire");
        currentMag--;
        AudioSource tmp = audio;
        tmp.clip = fireSound;
        tmp.Play();
        FireBullet();
        base.Fire();
    }
    protected virtual void FireBullet()
    {
        foreach (var i in firePoints)
        {
            float dir = Mathf.Atan2(i.right.y, i.right.x) * Mathf.Rad2Deg;
            Bullet bul = bullet.PoolCreate(i.position, Quaternion.Euler(0, 0, dir + Random.Range(-spread, spread))).GetComponent<Bullet>();
            bul.Set(damage, range, bulletSpeed, knockback, enemy, bulletPierce, reflectable);
        }
    }
    IEnumerator reloading = null;
    float reloadCounter = 0.0f;
    public override float gaugeFill { get { return reloadCounter / reloadTime; } }
    public override void AttemptReload()
    {
        base.AttemptReload();
        if(currentMag < magSize && currentAmmo > 0 && reloading == null)
        {
            Reload();
        }
    }
    protected void Reload()
    {
        AudioSource tmp = audio;
        tmp.clip = reloadSound;
        tmp.Play();
        reloadCounter = reloadTime;
        reloading = Reloading();
        reloading = Reloading();
        user.StartCoroutine(reloading);
    }
    IEnumerator Reloading()
    {
        while(reloadCounter > 0.0f)
        {
            reloadCounter = Mathf.Max(0, reloadCounter - Time.deltaTime);
            yield return null;
        }
        if (enemy) currentMag = magSize;
        else
        {
            int amount = Mathf.Min(currentAmmo, magSize - currentMag);
            currentAmmo -= amount;
            currentMag += amount;
        }
        reloading = null;
    }
    public override string AmmoDesc()
    {
        return currentMag + "/" + currentAmmo;
    }
    public override string Describe()
    {
        return base.Describe() + "\n" +
            "Mag size: " + magSize + "\n" + 
            "Ammo: " + maxAmmo + "\n" + 
            "Spread: " + spread;
    }
}
