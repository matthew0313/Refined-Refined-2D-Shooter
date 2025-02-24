using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D), typeof(PlayerMovements), typeof(PlayerWeapons))]
[RequireComponent(typeof(PlayerUIs))]
public class Player : Entity
{
    [SerializeField] float m_kbStop = 2.0f;
    protected override float kbStop { get { return m_kbStop; } }
    PlayerMovements movements;
    PlayerWeapons weapons;
    PlayerUIs UIs;

    public delegate void GameoverCallback();
    public GameoverCallback gameover;
    protected override void Awake()
    {
        base.Awake();
        movements = GetComponent<PlayerMovements>();
        weapons = GetComponent<PlayerWeapons>();
        UIs = GetComponent<PlayerUIs>();
    }
    private void Update()
    {
        if (GameManager.Instance.paused) return;
        movements.OnUpdate();
        weapons.OnUpdate();
        UIs.OnUpdate();
    }
    private void FixedUpdate()
    {
        movements.OnFixedUpdate();
    }
    public override void Death()
    {
        base.Death();
        gameover.Invoke();
        this.enabled = false;
    }
}
