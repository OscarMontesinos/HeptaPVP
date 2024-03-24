using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoanaSpeed : Buff
{
    float spd;

    public void SetUp(Loana user, float time, float spd)
    {
        this.user = user;
        this.spd = spd;
        this.time = time;
        user.stats.spd += spd;
    }

    public override void Die()
    {
        user.stats.spd -= spd;
        base.Die();
    }
}
