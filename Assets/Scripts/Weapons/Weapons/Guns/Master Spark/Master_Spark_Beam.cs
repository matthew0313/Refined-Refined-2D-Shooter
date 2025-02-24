using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Master_Spark_Beam : MonoBehaviour 
{
    [SerializeField] Master_Spark origin;
    [SerializeField] BoxCollider2D hitArea;
    [SerializeField] float scanRate;
    [SerializeField] float damage, knockback;
    float counter = 0.0f;
    List<Entity> hitObjects = new List<Entity>();
    private void Update()
    {
        if (counter < scanRate) counter += Time.deltaTime;
        else
        {
            counter = 0.0f;
            hitObjects.ForEach((Entity i) =>
            {
                if (i == null) return;
                if (knockback > 0.0f) i.GiveKnockback(transform.right.normalized * knockback);
                i.GiveDamage(damage);
            });
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(origin.enemy && collision.CompareTag("Player") || !origin.enemy && collision.CompareTag("Enemy"))
        {
            hitObjects.Add(collision.GetComponent<Entity>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (origin.enemy && collision.CompareTag("Player") || !origin.enemy && collision.CompareTag("Enemy"))
        {
            hitObjects.Remove(collision.GetComponent<Entity>());
        }
    }
}
