using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Master_Spark : Gun
{
    [Header("Master Spark")]
    [SerializeField] Animator master_spark_anim;
    [SerializeField] AudioClip master_spark_sound;
    [SerializeField] float master_spark_cooldown;
    public Master_Spark()
    {
        hasAbility1 = true;
        ability1Name = "Love Sign: Master Spark";
    }
    private void Awake()
    {
        a1_cooldown = master_spark_cooldown;
    }
    public override void Ability1()
    {
        base.Ability1();
        weapons.DisableSwap();
        weapons.DisableFire();
        AudioSource tmp = audio;
        tmp.clip = master_spark_sound;
        tmp.Play();
        master_spark_anim.SetTrigger("Master_Spark_Beam");
        StartCoroutine(Beaming());
    }
    IEnumerator Beaming()
    {
        yield return new WaitForSeconds(5.0f);
        Master_Spark_End();
    }
    public void Master_Spark_End()
    {
        weapons.EnableSwap();
        weapons.EnableFire();
    }
    public override string Describe()
    {
        return base.Describe() + "\n" + 
            "Ab1: Love sign: Master Spark";
    }
}
