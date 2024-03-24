using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZafirShield : Shield
{
    public void SetUp(PjBase user,PjBase target, float amount, float duration)
    {
        this.target = target;
        this.user = user;
        ChangeShieldAmount(amount);
        this.time = duration;
    }
}
