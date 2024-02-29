using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanaWave : Projectile
{
    Loana user;
    float dmg;
    float slow;
    public List<PjBase> targetsHitted = new List<PjBase>();
    public void SetUp(Loana user, float range, float spd, float dmg, float slow)
    {
        this.user = user;
        this.speed = spd;
        this.range = range;
        this.dmg = dmg;
            this.slow = slow;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PjBase>())
        {
            PjBase target = collision.GetComponent<PjBase>();
            if (target.team != user.team && !targetsHitted.Contains(target))
            {
                target.GetComponent<TakeDamage>().TakeDamage(user, dmg, HitData.Element.water, PjBase.AttackType.Magical);
                user.DamageDealed(user, target, dmg, HitData.Element.water, HitData.AttackType.range, HitData.HabType.basic);
                if (target.GetComponent<LoanaSlow>())
                {
                    target.GetComponent<LoanaSlow>().SetUp(user, user.h3Time, 0);
                }
                else
                {
                    target.gameObject.AddComponent<LoanaSlow>().SetUp(user, user.h3Time, slow);
                }
                targetsHitted.Add(target);
            }
            else if (collision.GetComponent<Barrier>() && collision.GetComponent<Barrier>().user.team != user.team)
            {
                collision.GetComponent<TakeDamage>().TakeDamage(user, dmg, HitData.Element.water, PjBase.AttackType.Magical);
                Die();
            }
        }
    }
}
