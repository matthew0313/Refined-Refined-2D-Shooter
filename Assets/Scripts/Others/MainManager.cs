using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public void Begin()
    {
        SceneChanger.Instance.SwitchScene("Game");
    }
}
