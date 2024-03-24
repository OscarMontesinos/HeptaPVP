using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Loana : PjBase
{
    Animator animator;

    int combo;
    public GameObject aPoint;
    public float aArea;
    public float aDmg;

    public GameObject h1Point;
    public float h1Area;
    public float h1Dmg;
    public float h1StunTime;
    public float h1AtSpdMultiplier;
    public float h1BuffSpd;
    public float h1BuffDuration;

    public GameObject h2Bubble;
    public float h2Exh;
    public float h2BubbleHp;
    public float h2BubbleDuration;
    
    public GameObject h3Wave;
    public float h3Range;
    public float h3Speed;
    public float h3Dmg;
    public float h3Slow;
    public float h3Time;
    public float h3CastTime;
    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void MainAttack()
    {
        base.MainAttack();
        if (!IsCasting() && !IsSoftCasting() && !IsStunned() && !IsDashing())
        {
            StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd)));
            if (combo == 0)
            {
                animator.Play("LoanaAttack1");
                combo++;
            }
            else
            {

                animator.Play("LoanaAttack2");
                combo = 0;
            }
        }
    }

    public void LoanaAttack()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(aPoint.transform.position, aArea, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            if (enemy.team != team)
            {
                enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateSinergy(aDmg), HitData.Element.water, AttackType.Magical);
                DamageDealed(this, enemy, CalculateSinergy(aDmg), HitData.Element.water, HitData.AttackType.melee, HitData.HabType.basic);
            }
        }
    }

    public override void Hab1()
    {
        base.Hab1(); if (!IsCasting() && !IsStunned() && currentHab1Cd <= 0)
        {
            gameObject.AddComponent<LoanaSpeed>().SetUp(this, h1BuffDuration, h1BuffSpd);
            StartCoroutine(SoftCast(CalculateAtSpd(stats.atSpd * h1AtSpdMultiplier)));
            animator.Play("LoanaStrongAttack");
            currentHab1Cd = CDR(hab1Cd) ; 
            combo = 1;
        }
    }
    public void LoanaHab1Attack()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(h1Point.transform.position, h1Area, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            if (enemy.team != team)
            {
                enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateSinergy(h1Dmg), HitData.Element.water, AttackType.Magical);
                DamageDealed(this, enemy, CalculateSinergy(h1Dmg), HitData.Element.water, HitData.AttackType.melee, HitData.HabType.basic);
                Stunn(enemy, h1StunTime);
            }
        }
    }


    public override void Hab2()
    {
        if (!IsCasting() && !IsStunned() && currentHab2Cd <= 0)
        {
            LoanaBubble bubble = Instantiate(h2Bubble, aPoint.transform).GetComponent<LoanaBubble>();
            bubble.SetUp(this, CalculateControl(h2BubbleHp), h2BubbleDuration, CalculateControl(h2Exh));
            currentHab2Cd = CDR(hab2Cd) + h2BubbleDuration;
        }
    }

    public override void Hab3()
    {
        base.Hab3();
        if (!IsCasting() && !IsStunned() && currentHab3Cd <= 0)
        {
            StartCoroutine(Cast(h3CastTime));
            animator.Play("LoanaWave");
            currentHab3Cd = CDR(hab3Cd) + h3CastTime;
            combo = 0;
        }
    }
    public void CreateWave()
    {
        LoanaWave wave = Instantiate(h3Wave, transform.position, pointer.transform.rotation).GetComponent<LoanaWave>();
        wave.SetUp(this, h3Range, h3Speed, CalculateSinergy(h3Dmg), h3Slow);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(aPoint.transform.position, aArea);
        Gizmos.DrawWireSphere(h1Point.transform.position, h1Area);
    }
}
