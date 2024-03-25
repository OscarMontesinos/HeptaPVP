using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZafirBuff : Buff
{
    Zafir zafir;
    public float potAmount;
    public float regenPerSecond;
    public float duration;
    GameObject buffFx;
    GameObject burstFx;
    public void SetUp(Zafir user, PjBase target, float potAmount, float regenPerSecond, float duration, GameObject buffFx, GameObject burstFx)
    {
        this.user = user;
        this.zafir = user;
        this.target = target;
        if (target == user)
        {
            Die();
        }
        this.potAmount += potAmount;
        this.regenPerSecond += regenPerSecond;
        time = duration;
        this.duration = duration;
        if (target.team == user.team)
        {
            target.stats.pot += potAmount;
        }
        if (potAmount != 0)
        {
            this.buffFx = Instantiate(buffFx, transform);
            this.burstFx = burstFx;
        }
    }

    public void Burst()
    {
        Vector2 dist = user.transform.position - transform.position;
        if (dist.magnitude <= zafir.h3Range)
        {
            if (target.team == user.team)
            {
                target.Heal(user, zafir.CalculateControl(zafir.h3Heal), HitData.Element.desert);
            }
            else
            {
                target.GetComponent<TakeDamage>().TakeDamage(user, user.CalculateSinergy(zafir.h3Dmg), HitData.Element.desert, PjBase.AttackType.Magical);
                user.DamageDealed(user, target, zafir.CalculateSinergy(zafir.h3Dmg), HitData.Element.desert, HitData.AttackType.aoe, HitData.HabType.hability);
            }
            Instantiate(burstFx, transform.position, transform.rotation);
            time = duration;
        }
    }

    public override void Update()
    {
        if (target.team != user.team)
        {
            user.Heal(user,regenPerSecond * Time.deltaTime,HitData.Element.desert);
        }
        base.Update();
    }

    public override void Die()
    {
        Destroy(buffFx);
        if (user != null && target.team == user.team)
        {
            target.stats.pot -= potAmount;
        }
        zafir.zafirBuffs.Remove(this);
        base.Die();
    }
}
