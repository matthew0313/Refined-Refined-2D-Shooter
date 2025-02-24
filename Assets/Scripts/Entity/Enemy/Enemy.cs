using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMovements), typeof(EnemyWeapons))]
public class Enemy : Entity
{
    [SerializeField] int moneyReward = 0;
    public readonly UnityEvent onDeath = new UnityEvent();
    EnemyMovements movements;
    EnemyWeapons weapons;
    EnemyUIs UIs;
    protected override void Awake()
    {
        base.Awake();
        movements = GetComponent<EnemyMovements>();
        weapons = GetComponent<EnemyWeapons>();
        UIs = GetComponent<EnemyUIs>();
    }
    protected virtual void Update()
    {
        if (stunned)
        {
            if (UIs != null) UIs.OnUpdate();
            return;
        }
        movements.OnUpdate();
        weapons.OnUpdate();
        UIs.OnUpdate();
    }
    public override void Death()
    {
        base.Death();
        onDeath.Invoke();
        GameManager.Instance.EarnMoney(moneyReward);
        Destroy(gameObject);
    }
}
