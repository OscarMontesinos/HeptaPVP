using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanaBubble : Barrier
{
    public LoanaBubbleDebuff debuffGo;
    float debuff;


    public override void Update()
    {
        base.Update();
        transform.localEulerAngles = Vector3.zero;
    }
    public void SetUp(PjBase user, float hp, float duration, float debuff)
    {
        base.SetUp(user, hp, duration, false);
        this.debuff = debuff;
        debuffGo.SetUp(user, debuff);
        StartCoroutine(Place());
    }


    IEnumerator Place()
    {
        yield return new WaitForSeconds(0.55f);
        transform.parent = null;
    }

    public override void PreDie(PjBase killer)
    {
        debuffGo.Die();
        base.PreDie(killer);
    }
}
