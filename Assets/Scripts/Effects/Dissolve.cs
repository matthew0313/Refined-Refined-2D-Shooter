using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Dissolve : PooledPrefab
{
    SpriteRenderer rend;
    [SerializeField] float dissolveTime = 1.0f;
    float counter = 0.0f;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        counter = 0.0f;
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1.0f);
    }
    private void Update()
    {
        if (counter < dissolveTime)
        {
            counter += Time.deltaTime;
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1.0f - counter / dissolveTime);
        }
        else gameObject.PoolDestroy();
    }

}
