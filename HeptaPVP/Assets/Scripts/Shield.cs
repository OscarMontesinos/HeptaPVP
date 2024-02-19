using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Buff
{
    public static float shieldAmount;
    public float singularShieldAmount;

    public virtual float ChangeShieldAmount(float value)
    {
        if (value > -singularShieldAmount)
        {
            singularShieldAmount += value;
            shieldAmount += value;
            value = 0;
        }
        else
        {
            value += singularShieldAmount;
            singularShieldAmount = 0;
            shieldAmount += value;
        }

        if(shieldAmount < 0)
        {
            shieldAmount = 0;
        }

        return -value;
    }
}
