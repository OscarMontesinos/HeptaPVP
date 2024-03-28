using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ganya : PjBase
{
    Animator animator;

    public Sprite unchargedWeapon;
    public Sprite chargedWeapon;
    public SpriteRenderer weaponRenderer;

    public GameObject shooterPoint;
    public GameObject aBullet;
    public float aRange;
    public float aSpd;
    public float aDmg;
    public float aPassiveMultiplier;
    public float aPassiveActiveSeconds;
    [HideInInspector]
    public float aCurrentPassiveActiveSeconds;
    public int aPassiveShoots;

    public int h1MaxCharges;
    [HideInInspector]
    public int h1Charges;
    [HideInInspector]
    public float h1CurrentRealCd;
    public float h1Range;
    public float h1Spd;

    public GameObject h2Point;
    public GameObject h2Bomb;
    public float h2Area;
    public float h2Dmg;
    public float h2Stunn;
    public float h2CastTime;

    public GameObject h3Point;
    public float h3Dmg;
    public float h3Weight;
    public float h3Height;
    public float h3CastTime;
    bool h3Precast;
    public ParticleSystem h3Particle;


    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        h1Charges = h1MaxCharges;
        h1CurrentRealCd = CDR(hab1Cd);
        currentHab3Cd = CDR(hab3Cd);
    }

    public override void Update()
    {
        base.Update();
        if (aCurrentPassiveActiveSeconds > 0)
        {
            aCurrentPassiveActiveSeconds -= Time.deltaTime;
            weaponRenderer.sprite = chargedWeapon;
        }
        else
        {
            weaponRenderer.sprite = unchargedWeapon;
        }

        if(spinObjects.transform.localEulerAngles.z < 180 && spinObjects.transform.localEulerAngles.z > 0)
        {
            spinObjects.transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            spinObjects.transform.localScale = new Vector3(1,1,1);
        }
    }

    public override void MainAttack()
    {
        base.MainAttack();
        if (!IsCasting() && !IsSoftCasting() && !IsStunned() && !IsDashing())
        {
            h3Precast = false;
            if (aCurrentPassiveActiveSeconds > 0)
            {
                StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd)));
                StartCoroutine(RapidShoot());
            }
            else
            {
                StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd)));
                animator.Play("GanyaShoot");
            }
        }
    }

    public void Shoot()
    {
        GanyaShoot arrow = Instantiate(aBullet, shooterPoint.transform.position, pointer.transform.rotation).GetComponent<GanyaShoot>();
        arrow.SetUp(this, aSpd, aRange, CalculateStrength(aDmg));
    }
    public void Shoot2()
    {
        GanyaShoot arrow = Instantiate(aBullet, shooterPoint.transform.position, pointer.transform.rotation).GetComponent<GanyaShoot>();
        arrow.SetUp(this, aSpd, aRange, CalculateStrength(aDmg * aPassiveMultiplier));
    }

    public IEnumerator RapidShoot()
    {
        aCurrentPassiveActiveSeconds = 0;
        int shoots = aPassiveShoots;

        PjBase target = null; 
        Vector3 dist = Vector3.zero;
        if (Physics2D.CircleCast(shooterPoint.transform.position, 1, pointer.transform.up, aRange, GameManager.Instance.playerLayer))
        {
            target = Physics2D.CircleCast(shooterPoint.transform.position, 1, pointer.transform.up, aRange, GameManager.Instance.playerLayer).rigidbody.GetComponent<PjBase>();
            if (target.team == team)
            {
                target = null;
            }
            else
            {

                AnimationCursorLock(1);
            }
        }

        while (shoots > 0)
        {
            if(target != null)
            {
                dist = target.transform.position - transform.position;
                pointer.transform.up = dist;
            }
            animator.Play("Idle");
            yield return null;
            animator.Play("GanyaRapidShoot");
            shoots--;
            yield return new WaitForSeconds(CalculateAtSpd(stats.atSpd) / (aPassiveShoots * 2.5f));
        }
        AnimationCursorLock(0);
        if (h3Precast)
        {
            if (currentHab3Cd <= 0)
            {
                animator.Play("GanyaHability");
                StartCoroutine(Cast(h3CastTime));
                h3Precast = false;
            }
            else
            {
                h3Precast = false;
            }
        }
    }

    public override void RechargeHab1()
    {
        currentHab1Cd = h1Charges;
        if(h1Charges < h1MaxCharges)
        {
            if (h1CurrentRealCd > 0)
            {
                h1CurrentRealCd -= Time.deltaTime;
                if (h1Charges == 0)
                {
                    currentHab1Cd = h1CurrentRealCd;
                }
            }
            else
            {
                h1Charges++;
                h1CurrentRealCd = CDR(hab1Cd);
            }
        }
        else
        {
            h1CurrentRealCd = CDR(hab1Cd);
        }
    }

    public override void Hab1()
    {
        base.Hab1();
        if (!IsCasting() && !IsDashing() && !IsStunned() && h1Charges > 0)
        {
            aCurrentPassiveActiveSeconds = aPassiveActiveSeconds;
            StartCoroutine(Cast(0.2f));
            StartCoroutine(Hab1Dash());
            h1Charges--;
        }
    }

    IEnumerator Hab1Dash()
    {
        AnimationCursorLock(0);
        yield return null;
        StartCoroutine(Dash(pointer.transform.up, h1Spd, h1Range));
    }


    public override void Hab2()
    {
        base.Hab2();

        if (!IsCasting() && !IsDashing() && !IsStunned() && currentHab2Cd <= 0)
        {
            GanyaBomb bomb = Instantiate(h2Bomb, pointer.transform.position, pointer.transform.rotation).GetComponent<GanyaBomb>();
            bomb.SetUp(this);
            aCurrentPassiveActiveSeconds = aPassiveActiveSeconds;
            StartCoroutine(Cast(h2CastTime));
            currentHab2Cd = CDR(hab2Cd);
        }
    }

    public override void Hab3()
    {
        if (!IsCasting() && !IsDashing() && !IsStunned() && currentHab3Cd <= 0)
        {
            animator.Play("GanyaHability");
            StartCoroutine(Cast(h3CastTime));
            if (IsSoftCasting())
            {
                h3Precast = true;
            }
        }
            base.Hab3();
    }

    public void ShootHab3()
    {
        aCurrentPassiveActiveSeconds = aPassiveActiveSeconds;

        h3Particle.Play();

        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(h3Point.transform.position, new Vector2(h3Weight, h3Height), pointer.transform.localEulerAngles.z, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>(); if (enemy.team != team)
            {
                enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateStrength(h3Dmg), HitData.Element.fire, AttackType.Physical);
                DamageDealed(this, enemy, CalculateStrength(h3Dmg), HitData.Element.fire, HitData.AttackType.melee, HitData.HabType.hability);
            }
        }
        currentHab3Cd = CDR(hab3Cd);
    }

    public override void RechargeHab3()
    {

    }

    public override void DamageDealed(PjBase user, PjBase target, float amount, HitData.Element element, HitData.AttackType attackType, HitData.HabType habType)
    {
        base.DamageDealed(user, target, amount, element, attackType, habType);
        if(currentHab3Cd > 0)
        {
            currentHab3Cd -= amount;
        }
    }

    public override void OnKill(PjBase target)
    {
        base.OnKill(target);
        h1CurrentRealCd = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(h3Point.transform.position, new Vector3(h3Weight, h3Height, 1));
        Gizmos.DrawWireSphere(h2Point.transform.position, h2Area);
    }

}
