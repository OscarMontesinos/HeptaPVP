using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anne : PjBase
{
    public GameObject shooterPoint;
    public GameObject aArrow;
    public float aDmg;
    public float aRange;
    public float aSpd;
    public GameObject h1Arrow;
    public float h1Dmg;
    public float h1Range;
    public float h1Spd;
    public float h1CastTime;
    public GameObject h2Arrow;
    public GameObject h2StormArrow;
    public float h2Attacks;
    [HideInInspector]
    public float h2AttacksCounter;
    public float h2AtSpdBuff;
    public float h2Dmg;
    public float h2Prerange;
    public float h2Prespd;
    public float h2Range;
    public float h2Spd;
    public float h3Range;
    public float h3Spd;
    bool h3Dashing;
    Animator animator;
    public Sprite unchargedWeapon;
    public Sprite chargedWeapon;
    public SpriteRenderer weaponRenderer;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        if (h2AttacksCounter > 0)
        {
            weaponRenderer.sprite = chargedWeapon;
        }
        else
        {
            weaponRenderer.sprite = unchargedWeapon;
        }
        base.Update();
    }

    public override void RechargeHab2()
    {
        if (h2AttacksCounter <= 0)
        {
            base.RechargeHab2();
        }
        else
        {
            currentHab2Cd = h2AttacksCounter;
        }
    }

    public override void MainAttack()
    {
        base.MainAttack();
        if (!IsCasting() && !IsSoftCasting() && !IsStunned() && !IsDashing())
        {
            if (h2AttacksCounter <= 0)
            {
                StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd)));
                animator.Play("ShootStandard");
            }
            else
            {
                StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd+h2AtSpdBuff)));
                animator.Play("ShootStorm");
            }
        }
        else if (h3Dashing && !IsStunned())
        {
            StartCoroutine(Cast(h1CastTime));
            animator.Play("ShootStrong");
            h3Dashing = false;
        }
    }
    public void ShootBasicArrow()
    {
        AnneBaseArrow arrow = Instantiate(aArrow, shooterPoint.transform.position, shooterPoint.transform.rotation).GetComponent<AnneBaseArrow>();
        arrow.SetUp(this, aSpd, aRange, CalculateSinergy(aDmg));
    }

    public override void Hab1()
    {
        base.Hab1();
        if (!IsCasting() && !IsDashing() && !IsStunned() && currentHab1Cd <=0)
        {
            StartCoroutine(Cast(h1CastTime));
            animator.Play("ShootStrong");
            currentHab1Cd = CDR(hab1Cd);
        }
    }
    public void ShootStrongArrow()
    {
        AnneBaseArrow arrow = Instantiate(h1Arrow, shooterPoint.transform.position, shooterPoint.transform.rotation).GetComponent<AnneBaseArrow>();
        arrow.SetUp(this, h1Spd, h1Range, CalculateSinergy(h1Dmg));
    }


    public override void Hab2()
    {
        base.Hab2();
        if (currentHab2Cd <= 0)
        {
            h2AttacksCounter = h2Attacks;
        }
        else if(h2AttacksCounter > 0)
        {
            currentHab2Cd = CDR(hab2Cd * (1 - (0.2f * h2AttacksCounter)));
            h2AttacksCounter = 0;
        }
    }


    public void ShootStormArrow()
    {
        AnneTinyArrow arrow = Instantiate(h2Arrow, shooterPoint.transform.position, shooterPoint.transform.rotation).GetComponent<AnneTinyArrow>();
        arrow.SetUp(this, h2StormArrow, h2Prespd, h2Prerange,h2Spd,h2Range, CalculateSinergy(h2Dmg));
        h2AttacksCounter--;
        if(h2AttacksCounter <= 0)
        {
            currentHab2Cd = CDR(hab2Cd);
        }
    }

    public override void Hab3()
    {
        base.Hab3();
        if (!IsCasting() && !IsStunned() && !IsDashing() && currentHab3Cd <= 0)
        {
            animator.Play("Idle");
            currentHab3Cd = CDR(hab3Cd);
            StartCoroutine(Dash(pointer.transform.up, h3Spd, h3Range));
            h3Dashing = true;
            StartCoroutine(AnneDash());
        }
    }

    IEnumerator AnneDash()
    {
        yield return null;
        while (IsDashing() && !IsStunned())
        {
            yield return null;
        }
        h3Dashing = false;
    }
}
