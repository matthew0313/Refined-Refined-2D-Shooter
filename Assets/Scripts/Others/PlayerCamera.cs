using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Transform player;
    [SerializeField] float lerpRate = 1.0f;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, player.position, lerpRate * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
    }
}
