using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Buff
{
    public float shieldAmount;

    public virtual float ChangeShieldAmount(float value)
    {
        if (value >= -shieldAmount)
        {
            shieldAmount += value;
            target.stats.shield += value;
            value = 0;
        }
        else
        {
            value += shieldAmount;
            shieldAmount = 0;
            target.stats.shield += value;
        }

        if(target.stats.shield < 0)
        {
            target.stats.shield = 0;
        }

        return -value;
    }

    public override void Die()
    {
        ChangeShieldAmount(-shieldAmount);
        base.Die();
    }
}
