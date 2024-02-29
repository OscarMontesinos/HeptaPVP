using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoanaSlow : Buff
{
    float spd;

    public void SetUp(Loana user, float time, float spd)
    {
        this.user = user;
        if (spd != 0)
        {
            this.spd = spd;
        }
        this.time = time;
        target = GetComponent<PjBase>();
        target.stats.spd -= spd;
    }

    public override void Die()
    {
        target.stats.spd += spd;
        base.Die();
    }
}
