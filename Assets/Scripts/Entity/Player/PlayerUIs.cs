using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIs : MonoBehaviour
{
    [SerializeField] Transform healthBar, dashBar;
    [SerializeField] Transform reloadBar;
    [SerializeField] Text weaponName, ammoDesc;

    [SerializeField] GameObject ability1;
    [SerializeField] Transform ability1Bar;
    [SerializeField] Text ability1Name;

    [SerializeField] GameObject ability2; 
    [SerializeField] Transform ability2Bar;
    [SerializeField] Text ability2Name;

    [SerializeField]
    Image[] weaponImages = new Image[5];
    [SerializeField]
    Text[] weaponNames = new Text[5];
    PlayerWeapons weapons;
    PlayerMovements movements;
    Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
        movements = GetComponent<PlayerMovements>();
        weapons = GetComponent<PlayerWeapons>();
        weapons.onSwap.AddListener(WeaponInfo);
        weapons.onLoadoutChange.AddListener(LoadoutInfo);
    }
    private void Start()
    {
        WeaponInfo();
    }
    public void OnUpdate()
    {
        Health_DashBar();
        WeaponGauge();
        BulletInfo();
    }
    void Health_DashBar()
    {
        healthBar.localScale = new Vector2(Mathf.Min(player.hp/player.maxHp, 1.0f), healthBar.localScale.y);
        dashBar.localScale = new Vector2(Mathf.Min(movements.dashCounter/movements.dashCooldown, 1.0f), dashBar.localScale.y);
    }
    void WeaponInfo()
    {
        weaponName.text = weapons.currentWeapon.weaponName;
        if (weapons.currentWeapon.hasAbility1)
        {
            ability1.SetActive(true);
            ability1Name.text = "[E]: " + weapons.currentWeapon.ability1Name;
        }
        else
        {
            ability1.SetActive(false);
        }
        if (weapons.currentWeapon.hasAbility2)
        {
            ability2.SetActive(true);
            ability2Name.text = "[F]: " + weapons.currentWeapon.ability2Name;
        }
        else
        {
            ability2.SetActive(false);
        }
    }
    void LoadoutInfo()
    {
        for(int i = 0; i < weapons.weapons.Length; i++)
        {
            if (weapons.weapons[i] == null)
            {
                weaponImages[i].color = new Color(1, 1, 1, 0);
                weaponNames[i].text = string.Empty;
            }
            else
            {
                if (weapons.weapons[i].image != null)
                {
                    weaponImages[i].color = new Color(1, 1, 1, 1);
                    weaponImages[i].sprite = weapons.weapons[i].image;
                }
                else
                {
                    weaponImages[i].color = new Color(1, 1, 1, 0);
                }
                weaponNames[i].text = weapons.weapons[i].weaponName;
            }
        }
    }
    void BulletInfo()
    {
        ammoDesc.text = weapons.currentWeapon.AmmoDesc();
    }
    void WeaponGauge()
    {
        reloadBar.localScale = new Vector2(Mathf.Max(weapons.currentWeapon.gaugeFill, 0.0f), reloadBar.localScale.y);
        if (weapons.currentWeapon.hasAbility1)
        {
            ability1Bar.localScale = new Vector2(Mathf.Min(weapons.currentWeapon.a1_counter / weapons.currentWeapon.a1_cooldown, 1.0f), ability1Bar.localScale.y);
        }
        if (weapons.currentWeapon.hasAbility2)
        {
            ability2Bar.localScale = new Vector2(Mathf.Min(weapons.currentWeapon.a2_counter / weapons.currentWeapon.a2_cooldown, 1.0f), ability2Bar.localScale.y);
        }
    }
}
