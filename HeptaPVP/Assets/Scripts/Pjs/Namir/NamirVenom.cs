using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NamirVenom : Buff
{

    List<Venom> venoms = new List<Venom>();

    public class Venom
    {
        public float dmg;
        public float duration;
    }


    public void SetUp(Namir user, float time, float dmg)
    {
        Venom venom = new Venom();
        venom.duration = time;
        venom.dmg = dmg;
        venoms.Add(venom);
        this.user = user;
        target = GetComponent<PjBase>();
        untimed = true;
        this.time = time;
    }

    public override void Update()
    {
        base.Update();

        if(user == null)
        {
            Die();
        }

        bool end = true;

        foreach (Venom venom in venoms)
        {
            if (venom.duration > 0)
            {
                end = false;
                GetComponent<TakeDamage>().TakeDamage(user, user.CalculateStrength(venom.dmg / time * Time.deltaTime), HitData.Element.desert, PjBase.AttackType.Magical);
                user.DamageDealed(user, target, user.CalculateStrength(venom.dmg / time * Time.deltaTime), HitData.Element.desert, HitData.AttackType.range, HitData.HabType.hability);
            }
            venom.duration -= Time.deltaTime;
        }

        if (end)
        {
            Die();
        }

    }
    public override void Die()
    {
        base.Die();
    }

}
