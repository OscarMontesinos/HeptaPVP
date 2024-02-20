using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnneTinyArrow : Projectile
{
    PjBase user;
    GameObject geiser;
    float dmg;
    float speed2;
    float range2;
    public void SetUp(PjBase user, GameObject geiser, float speed, float range, float speed2, float range2, float dmg)
    {
        this.user = user;
        this.geiser = geiser;
        this.speed = speed;
        this.range = range;
        this.speed2 = speed2;
        this.range2 = range2;
        this.dmg = dmg;
    }

    public override void Die()
    {
        AnneBaseArrow arrow = Instantiate(geiser, transform.position, transform.rotation).GetComponent<AnneBaseArrow>();
        arrow.SetUp(user, speed2, range2, dmg);
        base.Die();
    }

}
