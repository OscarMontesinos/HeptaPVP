using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Namir : PjBase
{
    Animator animator;
    public GameObject aPoint;
    public float aArea;
    public float aDmg;

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
            animator.Play("NamirAttack1");
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
                enemy.GetComponent<TakeDamage>().TakeDamage(this, CalculateStrength(aDmg), HitData.Element.desert, AttackType.Physical);
                DamageDealed(this, enemy, CalculateStrength(aDmg), HitData.Element.desert, HitData.AttackType.melee, HitData.HabType.basic);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(aPoint.transform.position, aArea);
    }

}
