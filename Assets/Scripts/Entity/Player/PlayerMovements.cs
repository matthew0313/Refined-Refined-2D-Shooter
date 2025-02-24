using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MovementController
{
    public Player origin { get; private set; }
    [SerializeField] GameObject m_dashEffect;
    public GameObject dashEffect { get { return m_dashEffect; } }
    [SerializeField] float m_moveSpeed;
    public float moveSpeed { get { return m_moveSpeed; } }
    [SerializeField] float m_dashSpeed, m_dashRange, m_dashEffectRate, m_dashCooldown;
    public float dashSpeed { get { return m_dashSpeed; } }
    public float dashRange { get { return m_dashRange; } }
    public float dashEffectRate { get { return m_dashEffectRate; } }
    public float dashCooldown { get { return m_dashCooldown; } }
    [HideInInspector] public float dashCounter = 0.0f;
    Layer<PlayerMovements> topLayer;
    [HideInInspector] public Vector2 dashDir { get; private set; }
    public void Awake()
    {
        topLayer = new PlayerMovements_TopLayer(this);
        origin = GetComponent<Player>();
    }
    private void Start()
    {
        topLayer.OnStateEnter();
    }
    public void OnUpdate()
    {
        if (dashCounter < dashCooldown) dashCounter += Time.deltaTime;
        topLayer.OnStateUpdate();
    }
    public void OnFixedUpdate()
    {
        topLayer.OnStateFixedUpdate();
    }
    public void SetDashDir(Vector2 dir)
    {
        dashDir = dir;
    }
}
