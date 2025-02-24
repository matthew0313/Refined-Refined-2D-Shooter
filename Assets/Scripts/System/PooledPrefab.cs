using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PooledPrefab : MonoBehaviour
{
    public GameObject origin { get; private set; }
    public void SetOrigin(GameObject origin) => this.origin = origin;
}
