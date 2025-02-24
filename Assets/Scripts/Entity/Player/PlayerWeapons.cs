using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapons : WeaponController
{
    [SerializeField] Transform weaponAnchor;
    [SerializeField] GameObject[] startWeaponPrefabs = new GameObject[5];
    public Weapon[] weapons { get; private set; } = new Weapon[5];
    int weaponIndex = 0;
    public UnityEvent onSwap { get; } = new UnityEvent();
    public UnityEvent onLoadoutChange { get; } = new UnityEvent();
    public Weapon currentWeapon { get { return weapons[weaponIndex]; } }
    public Player origin { get; private set; }
    public bool canReload = true;
    private void Awake()
    {
        origin = GetComponent<Player>();
        for (int i = 0; i < 5; i++)
        {
            if (startWeaponPrefabs[i] != null) Wield(startWeaponPrefabs[i], i);
        }
        currentWeapon.gameObject.SetActive(true);
    }
    private void Start()
    {
        onLoadoutChange.Invoke();
    }
    public void Wield(GameObject weaponPrefab, int index)
    {
        if (weapons[index] != null) weapons[index].gameObject.PoolDestroy();
        weapons[index] = weaponPrefab.PoolCreate(weaponAnchor.position, weaponAnchor.rotation, weaponAnchor).GetComponent<Weapon>();
        weapons[index].transform.localScale = Vector2.one;
        weapons[index].Set(this, false);
        if (index != weaponIndex) weapons[index].gameObject.SetActive(false);
        else onSwap.Invoke();
        onLoadoutChange.Invoke();
    }
    public void SwitchWeapon(int index)
    {
        if (!canSwap || index < 0 || index > 4 || weapons[index] == null) return;
        currentWeapon.gameObject.SetActive(false);
        weaponIndex = index;
        currentWeapon.gameObject.SetActive(true);
        onSwap.Invoke();
    }
    public void OnUpdate()
    {
        if (origin.stunned) return;
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown((KeyCode)(49 + i))){
                Debug.Log("EE");
                SwitchWeapon(i);
            }
        }
        if (currentWeapon.auto)
        {
            if (canClick && canFire && Input.GetMouseButton(0)) currentWeapon.AttemptFire();
        }
        else if (!currentWeapon.auto)
        {
            if (canClick && canFire && Input.GetMouseButtonDown(0)) currentWeapon.AttemptFire();
        }
        if (canReload && Input.GetKeyDown(KeyCode.R)) currentWeapon.AttemptReload();
        if (canUseAbility1 && Input.GetKeyDown(KeyCode.E)) currentWeapon.AttemptAbility1();
        if (canUseAbility2 && Input.GetKeyDown(KeyCode.F)) currentWeapon.AttemptAbility2();
    }
    bool canClick = true;
    public void DisableClick() => canClick = false;
    public void EnableClick() => canClick = true;
}
