using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GanyaBomb : MonoBehaviour
{
    public GameObject point;
    Ganya user;
    public List<ParticleSystem> bombsParticles = new List<ParticleSystem>();

    public void SetUp(Ganya user)
    {
        this.user = user;
    }

    public void Explode()
    {
        foreach (ParticleSystem particle in bombsParticles)
        {
            particle.Play();
        }

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(point.transform.position, user.h2Area, GameManager.Instance.playerLayer);
        PjBase enemy;
        foreach (Collider2D enemyColl in enemiesHit)
        {
            enemy = enemyColl.GetComponent<PjBase>();
            if (enemy.team != user.team)
            {
                enemy.GetComponent<TakeDamage>().TakeDamage(user, user.CalculateStrength(user.h2Dmg), HitData.Element.fire, PjBase.AttackType.Physical);
                user.DamageDealed(user, enemy, user.CalculateStrength(user.aDmg), HitData.Element.fire, HitData.AttackType.melee, HitData.HabType.basic);

                user.Stunn(enemy, user.h2Stunn);

            }
        }

    }
}
