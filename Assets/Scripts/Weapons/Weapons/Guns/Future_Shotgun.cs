using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Future_Shotgun : Shotgun
{
    [Header("Future Shotgun")]
    [SerializeField] Transform drone1;
    [SerializeField] Transform drone2, firePoint1, firePoint2;
    [SerializeField] float assasult_drone_damage, assasult_drone_fireRate, assasult_drone_range, assasult_drone_speed, assasult_drone_knockback, assasult_drone_duration, assasult_drone_cooldown;
    [SerializeField] GameObject assasult_drone_projectile;
    Player player;
    public Future_Shotgun()
    {
        hasAbility1 = true;
        ability1Name = "Drone Assault";
    }
    private void Awake()
    {
        a1_cooldown = assasult_drone_cooldown;
        drone1.gameObject.SetActive(false);
        drone2.gameObject.SetActive(false);
    }
    private void Start()
    {
        if (enemy)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }
    public override void Ability1()
    {
        base.Ability1();
        weapons.DisableSwap();
        drone1.gameObject.SetActive(true);
        drone2.gameObject.SetActive(true);
        droneCounter = 0.0f;
        droneFireCounter = 0.0f;
        user.StartCoroutine(DroneAssasult());
    }
    float droneCounter = 0.0f;
    float droneFireCounter = 0.0f;
    IEnumerator DroneAssasult()
    {
        while (droneCounter < assasult_drone_duration)
        {
            droneCounter += Time.deltaTime;
            if (droneFireCounter < assasult_drone_fireRate) droneFireCounter += Time.deltaTime;
            else
            {
                droneFireCounter -= assasult_drone_fireRate;
                Vector2 pos;
                if (!enemy) pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                else pos = player.transform.position;
                drone1.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(pos.y - drone1.position.y, pos.x - drone1.position.x) * Mathf.Rad2Deg);
                drone2.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(pos.y - drone2.position.y, pos.x - drone2.position.x) * Mathf.Rad2Deg);
                assasult_drone_projectile.PoolCreate(firePoint1.position, firePoint1.rotation).GetComponent<Bullet>().Set(assasult_drone_damage, assasult_drone_range, assasult_drone_speed, assasult_drone_knockback, enemy, false, true);
                assasult_drone_projectile.PoolCreate(firePoint2.position, firePoint2.rotation).GetComponent<Bullet>().Set(assasult_drone_damage, assasult_drone_range, assasult_drone_speed, assasult_drone_knockback, enemy, false, true);
            }
            yield return null;
        }
        weapons.EnableSwap();
        drone1.gameObject.SetActive(false);
        drone2.gameObject.SetActive(false);
    }
    public override string Describe()
    {
        return base.Describe() + "\n" +
            "Ab1: Drone Assasult";
    }
}
