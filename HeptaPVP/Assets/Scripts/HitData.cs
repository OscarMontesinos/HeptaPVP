using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitData
{
    public enum Element
    {
        ice,fire,water,desert,wind,nature,lightning,blood
    }

    public enum AttackType
    {
        range, melee, aoe, dot, pot, heal, passive
    }

    public enum HabType
    {
        basic, hability
    }
}
