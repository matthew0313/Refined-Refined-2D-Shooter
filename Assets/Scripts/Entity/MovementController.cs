using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    public bool canRotate { get; private set; } = true;
    [SerializeField] Transform m_rotator;
    public Transform rotator { get { return m_rotator; } }
    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;
    public void EnableRotation() => canRotate = true;
    public void DisableRotation() => canRotate = false;
}
