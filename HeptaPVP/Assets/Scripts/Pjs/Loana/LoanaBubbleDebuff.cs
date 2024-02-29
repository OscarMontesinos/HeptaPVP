using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanaBubbleDebuff : Spell
{
    public float debuff;
    public override void Update()
    {
        base.Update();
        transform.position = transform.parent.position;
    }
    public void SetUp(PjBase user, float debuff)
    {
        untimed = true;
        this.user = user;
        this.debuff = debuff;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PjBase>() && collision.GetComponent<PjBase>().team != user.team)
        {
            SpellEnter(collision.GetComponent<PjBase>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PjBase>() && collision.GetComponent<PjBase>().team != user.team)
        {
            SpellExit(collision.GetComponent<PjBase>());
        }
    }

    public override void SpellEnter(PjBase target)
    {
        base.SpellEnter(target);
        target.stats.sinergy -= debuff;
        target.stats.strength -= debuff;
        targets.Add(target);
    }

    public override void SpellExit(PjBase target)
    {
        base.SpellExit(target);
        target.stats.sinergy += debuff;
        target.stats.strength += debuff;
        targets.Remove(target);
    }

    public override void Die()
    {
        foreach (PjBase target in targets)
        {
            target.stats.sinergy += debuff;
            target.stats.strength += debuff;
        }
        base.Die();
    }
}
