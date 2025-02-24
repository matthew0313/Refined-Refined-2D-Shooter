using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool canFire { get; private set; } = true;
    public bool canSwap { get; private set; } = true;
    public bool canUseAbility1 { get; private set; } = true;
    public bool canUseAbility2 { get; private set; } = true;
    public void EnableFire() => canFire = true;
    public void DisableFire() => canFire = false;
    public void EnableSwap() => canSwap = true;
    public void DisableSwap() => canSwap = false;
    public void EnableAbility1() => canUseAbility1 = true;
    public void DisableAbility1() => canUseAbility1 = false;
    public void EnableAbility2() => canUseAbility2 = true;
    public void DisableAbility2() => canUseAbility2 = false;
}
