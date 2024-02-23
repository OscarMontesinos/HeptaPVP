using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Namir : PjBase
{
    Animator animator;
    public GameObject aPoint;
    public float aArea;
    public float aDmg;

    [HideInInspector]
    public bool h1Dashing;
    float h1DashActive;
    public float h1TimeToDash;
    [HideInInspector]
    public float h1AttacksCounter;
    public float h1Area;
    public float h1Dmg1;
    public float h1Spd1;
    public float h1Range1;
    public float h1Dmg2;
    public float h1Spd2;
    public float h1Range2;
    public float h1Stunn2;
    public float h1Dmg3;
    public float h1Spd3;
    public float h1Range3;

    public Barrier h2ActiveCloud;
    public GameObject h2Cloud;
    public float h2CloudDuration;
    public float h2BuffDuration;
    public float h2Buff;

    public float h3Attacks;
    [HideInInspector]
    public float h3AttacksCounter;
    public float h3AtSpdBuff;
    public float h3DmgModifier;
    public float h3DmgOverTime;
    public float h3Duration;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

        if(h1DashActive> 0)
        {
            h1DashActive -= Time.deltaTime;
        }
        else if (h1AttacksCounter>0)
        {
            h1AttacksCounter = 0;
            currentHab1Cd = CDR(hab1Cd);
        }
    }

    public override void MainAttack()
    {
        base.MainAttack();

        if (!IsCasting() && !IsSoftCasting() && !IsStunned() && !IsDashing())
        {
            if (h3AttacksCounter <= 0)
            {
                StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd)));
                animator.Play("NamirAttack1");
            }
            else if(h3AttacksCounter % 2 == 0)
            {
                StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd + h3AtSpdBuff)));
                animator.Play("NamirAttackVenom2");
            }
            else
            {
                StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd + h3AtSpdBuff)));
                animator.Play("NamirAttackVenom1");
            }
        }
    }

    public void MainAttackDmg()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(aPoint.transform.position, aArea, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            if (enemy.team != team)
            {
                if (h3AttacksCounter > 0)
                {
                    enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateStrength(aDmg * h3DmgModifier), HitData.Element.desert, AttackType.Physical);
                    DamageDealed(this, enemy, CalculateStrength(aDmg * h3DmgModifier), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.basic);

                    NamirVenom venom;

                    if (enemy.gameObject.GetComponent<NamirVenom>())
                    {
                        venom = enemy.gameObject.GetComponent<NamirVenom>();
                    }
                    else
                    {
                        venom = enemy.gameObject.AddComponent<NamirVenom>();
                    }

                    venom.SetUp(this, h3Duration, CalculateSinergy(h3DmgOverTime));
                }
                else
                {
                    enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateStrength(aDmg), HitData.Element.desert, AttackType.Physical);
                    DamageDealed(this, enemy, CalculateStrength(aDmg), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.basic);
                }
            }
        }
        if (h3AttacksCounter > 0)
        {
            h3AttacksCounter--;
            if (h3AttacksCounter <= 0)
            {
                currentHab3Cd = CDR(hab3Cd);
            }
        }
    }

    public override void Hab1()
    {
        base.Hab1(); 
        if (!IsCasting() && !IsDashing() && !IsStunned() && (currentHab1Cd <= 0 || h1AttacksCounter > 0) && !h1Dashing)
        {
            h1DashActive = h1TimeToDash;
            AnimationCursorLock(1);
            StartCoroutine(Cast(0.3f));
            if(h1AttacksCounter == 0)
            {
                StartCoroutine(H1Dash1());
                h1AttacksCounter++;
            }
            else if (h1AttacksCounter == 1)
            {
                StartCoroutine (H1Dash2());
                h1AttacksCounter++;
            }
            else if(h1AttacksCounter == 2)
            {
                StartCoroutine (H1Dash3());
                currentHab1Cd = CDR(hab1Cd);
                h1AttacksCounter = 0;
            }

        }
    }

    public IEnumerator H1Dash1()
    {
        animator.Play("NamirDash1");
        h1Dashing = true;
        StartCoroutine(Dash(pointer.transform.up, h1Spd1, h1Range1));
        yield return null;  
        List<PjBase> enemiesHitted = new List<PjBase>();
        while (dashing && h1Dashing)
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, h1Area, GameManager.Instance.playerLayer);
            PjBase enemy;
            foreach (Collider2D enemyColl in enemiesHit)
            {
                enemy = enemyColl.GetComponent<PjBase>();
                if (enemy.team != team)
                {
                    if (!enemiesHitted.Contains(enemy))
                    {
                        enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateStrength(h1Dmg1), HitData.Element.desert, AttackType.Physical);
                        DamageDealed(this, enemy, CalculateStrength(h1Dmg1), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.hability);
                        NamirVenom venom;

                        if (enemy.gameObject.GetComponent<NamirVenom>())
                        {
                            venom = enemy.gameObject.GetComponent<NamirVenom>();
                        }
                        else
                        {
                            venom = enemy.gameObject.AddComponent<NamirVenom>();
                        }

                        venom.SetUp(this, h3Duration, CalculateSinergy(h3DmgOverTime * 2));

                        enemiesHitted.Add(enemy);
                    }
                }
            }
            yield return null;
        }
        animator.Play("Idle");
        AnimationCursorLock(0);
        yield return null;
        h1Dashing = false;
    }

    public IEnumerator H1Dash2()
    {
        animator.Play("NamirDash2");
        h1Dashing = true;
        StartCoroutine(Dash(pointer.transform.up, h1Spd2, h1Range2));
        yield return null;
        List<PjBase> enemiesHitted = new List<PjBase>();
        while (dashing && h1Dashing)
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, h1Area, GameManager.Instance.playerLayer);
            PjBase enemy;
            foreach (Collider2D enemyColl in enemiesHit)
            {
                enemy = enemyColl.GetComponent<PjBase>();
                if (enemy.team != team)
                {
                    if (!enemiesHitted.Contains(enemy))
                    {
                        enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateStrength(h1Dmg2), HitData.Element.desert, AttackType.Physical);
                        DamageDealed(this, enemy, CalculateStrength(h1Dmg2), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.hability);
                        Stunn(enemy, h1Stunn2);

                        enemiesHitted.Add(enemy);
                    }
                }
            }
            yield return null;
        }

        animator.Play("Idle");
        AnimationCursorLock(0);
        yield return null;
        h1Dashing = false;
    }

    public IEnumerator H1Dash3()
    {
        animator.Play("NamirDash3");
        h1Dashing = true;
        StartCoroutine(Dash(pointer.transform.up, h1Spd3, h1Range3));
        yield return null;
        List<PjBase> enemiesHitted = new List<PjBase>();
        while (dashing && h1Dashing)
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, h1Area, GameManager.Instance.playerLayer);
            PjBase enemy;
            foreach (Collider2D enemyColl in enemiesHit)
            {
                enemy = enemyColl.GetComponent<PjBase>();
                if (enemy.team != team)
                {
                    if (!enemiesHitted.Contains(enemy))
                    {
                        enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateStrength(h1Dmg3), HitData.Element.desert, AttackType.Physical);
                        DamageDealed(this, enemy, CalculateStrength(h1Dmg3), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.hability);

                        enemiesHitted.Add(enemy);
                    }
                }
            }
            yield return null;
        }

        animator.Play("Idle");
        AnimationCursorLock(0);
        yield return null;
        h1Dashing = false;
    }



    public override void RechargeHab1()
    {
        if (h1AttacksCounter <= 0)
        {
            base.RechargeHab1();
        }
        else
        {
            currentHab1Cd = h1DashActive;
        }
    }



    public override void Hab2()
    {
        base.Hab2();
        if (!IsCasting() && !IsDashing() && !IsStunned() && currentHab2Cd <= 0)
        {
            Barrier cloud = Instantiate(h2Cloud, transform.position, transform.rotation).GetComponent<Barrier>();
            cloud.SetUp(this, 1, h2CloudDuration, true);
            h2ActiveCloud = cloud;
            NamirSpd spd = gameObject.AddComponent<NamirSpd>();
            spd.SetUp(this,h2BuffDuration,h2Buff);
            currentHab2Cd = CDR(hab2Cd);
        }
    }


    public override void Hab3()
    {
        base.Hab3();
        if (currentHab3Cd <= 0 && h3AttacksCounter == 0)
        {
            h3AttacksCounter = h3Attacks;
        }
    }

    public override void RechargeHab3()
    {
        if (h3AttacksCounter <= 0)
        {
            base.RechargeHab3();
        }
        else
        {
            currentHab3Cd = h3AttacksCounter;
        }
    }

    public override void OnKill(PjBase target)
    {
        base.OnKill(target);
        if(h2ActiveCloud != null)
        {
            h2ActiveCloud.SetUp(this, 1, h2CloudDuration, true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(aPoint.transform.position, aArea);
        Gizmos.DrawWireSphere(transform.position, h1Area);
    }

}
