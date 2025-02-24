using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyMovements : MovementController
{
    Player player;
    [SerializeField] float moveSpeed, backStepSpeed, minDistance, maxDistance;
    public float dist { get; private set; }
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        originalScaleY = rotator.localScale.y;
    }
    float originalScaleY;
    public void OnUpdate()
    {
        float dir = Mathf.Atan2(rotator.position.y - player.transform.position.y, rotator.position.x - player.transform.position.x) * Mathf.Rad2Deg;
        rotator.rotation = Quaternion.Euler(0, 0, dir);
        if (dir >= 90.0f || dir <= -90.0f) rotator.localScale = new Vector2(rotator.localScale.x, -originalScaleY);
        else rotator.localScale = new Vector2(rotator.localScale.x, originalScaleY);
        dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist < minDistance)
        {
            transform.Translate(rotator.right * backStepSpeed * Time.deltaTime);
        }
        else if(dist > maxDistance)
        {
            transform.Translate(rotator.right * -1.0f * moveSpeed * Time.deltaTime);
        }
    }
}
