using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapons : WeaponController
{
    Player player;
    EnemyMovements movements;
    [SerializeField] Transform weaponAnchor;
    [SerializeField] List<WeaponDistPair> weapons;

    public Weapon currentWeapon { get { return weapons[weaponIndex].weapon; } }
    int weaponIndex = 0;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        movements = GetComponent<EnemyMovements>();
        foreach(WeaponDistPair i in weapons)
        {
            i.weapon = i.weaponPrefab.PoolCreate(weaponAnchor.position, weaponAnchor.rotation, weaponAnchor).GetComponent<Weapon>();
            i.weapon.Set(this, true);
            i.weapon.gameObject.SetActive(false);
            i.weapon.transform.localScale = Vector2.one;
        }
        if (weapons.Count > 0) weapons[0].weapon.gameObject.SetActive(true);
    }
    public void SwitchWeapon(int index)
    {
        if (!canSwap || index < 0 || index >= weapons.Count) return;
        if (weaponIndex == index) return;
        if(currentWeapon is Gun)
        {
            if ((currentWeapon as Gun).currentMag <= (currentWeapon as Gun).magSize / 2) currentWeapon.AttemptReload();
        }
        currentWeapon.gameObject.SetActive(false);
        weaponIndex = index;
        currentWeapon.gameObject.SetActive(true);
    }
    public void OnUpdate()
    {
        for(int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].equipDist >= movements.dist)
            {
                SwitchWeapon(i);
                break;
            }
        }
        if (weapons[weaponIndex].lmbDist >= movements.dist && canFire) currentWeapon.AttemptFire();
        if (weapons[weaponIndex].ability1Dist >= movements.dist && canUseAbility1) currentWeapon.AttemptAbility1();
        if (weapons[weaponIndex].ability2Dist >= movements.dist && canUseAbility2) currentWeapon.AttemptAbility2();
        if(currentWeapon is Gun)
        {
            if ((currentWeapon as Gun).currentMag == 0) currentWeapon.AttemptReload();
        }
        /*
        bool equipped = false;
        for(int i = 0; i <  weapons.Count; i++)
        {
            if (weapons[i].useDist >= movements.dist)
            {
                if(weaponIndex != i) SwitchWeapon(i);
                equipped = true;
                break;
            }
        }
        if (!equipped) return;
        if(canFire) currentWeapon.AttemptFire();
        if (weapons[weaponIndex].ability1UseDist >= movements.dist && canUseAbility1) currentWeapon.AttemptAbility1();
        if (weapons[weaponIndex].ability2UseDist >= movements.dist && canUseAbility2) currentWeapon.AttemptAbility2();
        if (currentWeapon is Gun)
        {
            if ((currentWeapon as Gun).currentMag == 0)
            {
                currentWeapon.AttemptReload();
            }
        }
        */
    }
}
[System.Serializable]
public class WeaponDistPair
{
    public GameObject weaponPrefab;
    [HideInInspector] public Weapon weapon;
    public float equipDist;
    public float lmbDist, ability1Dist, ability2Dist;
}