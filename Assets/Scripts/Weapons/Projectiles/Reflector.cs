using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
public class Reflector : MonoBehaviour
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }
    [SerializeField] bool enemy = false;
    public void Set(bool enemy)
    {
        this.enemy = enemy;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) collision.GetComponent<Bullet>().Reflect(enemy);
    }
}
