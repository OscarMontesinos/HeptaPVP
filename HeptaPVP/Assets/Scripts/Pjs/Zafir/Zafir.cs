using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Zafir : PjBase
{
    Animator animator;
    public Weapon weapon;
    int combo;
    public bool showARanges;
    public float aDmg;
    public GameObject aSpearPoint;
    public float aSpearWeight;
    public float aSpearHeight;
    public float aSpearHpPercentage;
    public float aSpearAtSpdMultiplier;
    public GameObject aShieldPoint;
    public float aShieldArea;
    public float aShieldResistancesBuff;
    public float aShieldAtSpdMultiplier;
    public GameObject aGaunletPoint;
    public float aGaunletAtSpdMultiplier;
    public float aGaunletArea;
    public float aGaunletSpd;

    public GameObject h1Spears;
    public bool showH1Ranges;
    public float h1Range;
    public float h1Angle;
    public float h1Dmg;

    public bool showH2Ranges;
    public float h2Range;
    public float h2Spd;
    public float h2Area;
    public float h2Dmg;
    public float h2StunnTime;
    public float h2AreaShield;
    public float h2ShieldAmount;
    public float h2ShieldDuration;

    public GameObject h3BuffFx;
    public GameObject h3BurstFx;
    public bool showH3Ranges;
    [HideInInspector]
    public List<ZafirBuff> zafirBuffs = new List<ZafirBuff>();
    public float h3Duration;
    public float h3Range;
    public float h3Dmg;
    public float h3Heal;
    public float h3PotAmount;
    public float h3RegenPerSecond;
    public enum Weapon
    {
        spear, shield, gaunlet
    }


    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    void ChangeWeapon(Weapon weapon)
    {
        switch (this.weapon)
        {
            case Weapon.spear:
                break;
            case Weapon.shield:
                stats.resist -= CalculateControl(aShieldResistancesBuff);
                break;
            case Weapon.gaunlet:
                stats.spd -= aGaunletSpd;
                break;

        }
        this.weapon = weapon;
        switch (this.weapon)
        {
            case Weapon.spear:
                break;
            case Weapon.shield:
                stats.resist += CalculateControl(aShieldResistancesBuff);
                break;
            case Weapon.gaunlet:
                stats.spd += aGaunletSpd;
                break;

        }
    }

    void IdleAnimation()
    {
        switch (weapon)
        {
            case Weapon.spear:
                animator.Play("SpearIdle");
                break;
            case Weapon.shield:
                animator.Play("ShieldIdle");
                break;
            case Weapon.gaunlet:
                animator.Play("GaunletsIdle");
                break;

        }
    }
    public override void MainAttack()
    {
        base.MainAttack();
        if (!IsCasting() && !IsSoftCasting() && !IsStunned() && !IsDashing())
        {
            switch (weapon)
            {
                case Weapon.spear:
                    StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd * aSpearAtSpdMultiplier)));
                    animator.Play("SpearAttack");
                    break;

                case Weapon.shield:
                    StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd * aShieldAtSpdMultiplier)));
                    animator.Play("ShieldAttack");
                    break;

                case Weapon.gaunlet:
                    StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd * aGaunletAtSpdMultiplier)));
                    if (combo == 0)
                    {
                        animator.Play("GaunletsAttack1");
                        combo++;
                    }
                    else
                    {

                        animator.Play("GaunletsAttack2");
                        combo = 0;
                    }
                    break;

            }

        }
    }

    public void AttackSpear()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(aSpearPoint.transform.position, new Vector2(aSpearWeight, aSpearHeight), pointer.transform.localEulerAngles.z, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>(); 
            if (enemy.team != team)
            {
                enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateStrength(aDmg) + (enemy.stats.mHp * (aSpearHpPercentage / 100)), HitData.Element.desert, AttackType.Physical);
                DamageDealed(this, enemy, CalculateStrength(aDmg) + (enemy.stats.mHp * (aSpearHpPercentage / 100)), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.basic);
                PlaceBuff(enemy);
            }
        }
    }

    public void AttackShield()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(aShieldPoint.transform.position, aShieldArea, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            if (enemy.team != team)
            {
                enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateSinergy(aDmg), HitData.Element.desert, AttackType.Physical);
                DamageDealed(this, enemy, CalculateSinergy(aDmg), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.basic);
                PlaceBuff(enemy);
            }
        }
    }
    public void AttackGaunlet()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(aGaunletPoint.transform.position, aGaunletArea, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            if (enemy.team != team)
            {
                enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateSinergy(aDmg), HitData.Element.desert, AttackType.Physical);
                DamageDealed(this, enemy, CalculateSinergy(aDmg), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.basic);
                PlaceBuff(enemy);
            }
        }
    }

    public override void Hab1()
    {
        base.Hab1();

        if (!IsCasting() && !IsStunned() && !IsDashing() && currentHab1Cd <= 0)
        {
            currentHab1Cd = CDR(hab1Cd);
            StartCoroutine(SoftCast(1));
            animator.Play("SpearHability");
            ChangeWeapon(Weapon.spear);
        }
    }

    public void SpearHability()
    {
        Instantiate(h1Spears, transform.position, spinObjects.transform.rotation);
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, h1Range, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            Transform target = enemyColl.transform;
            Vector2 dir = target.position - transform.position;

            if (Vector3.Angle(spinObjects.transform.up, dir.normalized) < h1Angle / 2)
            {
                enemy = enemyColl.GetComponent<PjBase>();
                if (enemy.team != team)
                {
                    enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateSinergy(h1Dmg), HitData.Element.desert, AttackType.Magical);
                    DamageDealed(this, enemy, CalculateSinergy(h1Dmg), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.hability);
                }
                PlaceBuff(enemy);
            }
        }

    }


    public override void Hab2()
    {
        base.Hab2();
        if (!IsCasting() && !IsStunned() && !IsDashing() && currentHab2Cd <= 0)
        {
            StartCoroutine(Cast(0.85f));
            animator.Play("ShieldHability");
            ChangeWeapon(Weapon.shield);
            currentHab2Cd = CDR(hab2Cd);
        }
    }
    public void ShieldShielding()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, h2AreaShield, GameManager.Instance.playerLayer);
        PjBase ally;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            ally = enemyColl.GetComponent<PjBase>();
            if (ally.team == team)
            {
                ally.AddComponent<ZafirShield>().SetUp(this,ally,CalculateControl(h2ShieldAmount),h2ShieldDuration);
                PlaceBuff(ally);
            }
        }
    }

    public void StartShieldDash()
    {
        StartCoroutine(ShieldDash());
    }

    IEnumerator ShieldDash()
    {
        AnimationCursorLock(1);
        StartCoroutine(Dash(pointer.transform.up, h2Spd, h2Range));
        yield return null;
        while (dashing)
        {
            ShieldStunn();
            yield return null;
        }
        AnimationCursorLock(0);
        IdleAnimation();
    }
    void ShieldStunn()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, h2Area, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            if (enemy.team != team)
            {
                enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateSinergy(h2Dmg), HitData.Element.desert, AttackType.Physical);
                DamageDealed(this, enemy, CalculateSinergy(h2Dmg), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.basic);
                Stunn(enemy,h2StunnTime);
                dashing = false;
                PlaceBuff(enemy);
            }
        }
    }


    public override void Hab3()
    {
        base.Hab3();
        if (!IsCasting() && !IsStunned() && !IsDashing() && currentHab3Cd <= 0)
        {
            zafirBuffs = zafirBuffs.Where(x => x != null).ToList();
            if (zafirBuffs.Count > 0)
            {
                StartCoroutine(Cast(0.40f));
                animator.Play("GaunletsHability");
                currentHab3Cd = CDR(hab3Cd);
                ChangeWeapon(Weapon.gaunlet);
            }
            else
            {
                currentHab3Cd = CDR(3);
                ChangeWeapon(Weapon.gaunlet);
                IdleAnimation();
            }
        }
    }

    void PlaceBuff(PjBase unit)
    {
        if (unit.GetComponent<ZafirBuff>())
        {
            unit.GetComponent<ZafirBuff>().SetUp(this, unit, 0, 0, h3Duration,h3BuffFx,h3BurstFx);
        }
        else if (unit != this)
        {
            ZafirBuff buff = unit.AddComponent<ZafirBuff>();
            buff.SetUp(this, unit, h3PotAmount, h3RegenPerSecond, h3Duration, h3BuffFx,h3BurstFx);
            zafirBuffs.Add(buff);
        }
        
    }

    public void GaunletAbility()
    {
        foreach(ZafirBuff buff in zafirBuffs)
        {
            if (buff != null)
            {
                buff.Burst();
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (showARanges)
        {
            Gizmos.DrawWireCube(aSpearPoint.transform.position, new Vector3(aSpearWeight, aSpearHeight, 1));
            Gizmos.DrawWireSphere(aShieldPoint.transform.position, aShieldArea);
            Gizmos.DrawWireSphere(aGaunletPoint.transform.position, aGaunletArea);
        }

        if (showH1Ranges)
        {
            Gizmos.DrawWireSphere(transform.position, h1Range);

            Vector3 rightAngle = GameManager.DirectionFromAngle(spinObjects.transform.eulerAngles.z, -h1Angle / 2);
            Vector3 leftAngle = GameManager.DirectionFromAngle(spinObjects.transform.eulerAngles.z, h1Angle / 2);

            Gizmos.DrawLine(transform.position, transform.position + leftAngle * h1Range);
            Gizmos.DrawLine(transform.position, transform.position + rightAngle * h1Range);
        }

        if (showH2Ranges)
        {
            Gizmos.DrawWireSphere(transform.position, h2Area);
            Gizmos.DrawWireSphere(transform.position, h2AreaShield);
        }

        if (showH3Ranges)
        {
            Gizmos.DrawWireSphere(transform.position, h3Range);
        }

    }
}
