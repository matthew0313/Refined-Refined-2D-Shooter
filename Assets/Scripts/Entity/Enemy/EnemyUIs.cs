using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIs : MonoBehaviour
{
    Enemy origin;
    [SerializeField] Transform healthBar;
    private void Awake()
    {
        origin = GetComponent<Enemy>();
    }
    public void OnUpdate()
    {
        healthBar.localScale = new Vector2(origin.hp / origin.maxHp, healthBar.localScale.y);
    }
}
