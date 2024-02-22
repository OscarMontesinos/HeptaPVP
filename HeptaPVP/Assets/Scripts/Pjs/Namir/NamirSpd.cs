using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamirSpd : Buff
{
    float spd;

    public void SetUp(Namir user, float time, float spd)
    {
        this.user = user;
        this.spd = spd;
        user.stats.spd += spd;
        this.time = time;
        target = GetComponent<PjBase>();
    }

    public override void Die()
    {
        user.stats.spd -= spd;
        base.Die();
    }
}
