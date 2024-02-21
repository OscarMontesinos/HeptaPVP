using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class IABase : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public PjBase character;
    static float viewDistance = 40f;
    public List<PjBase> enemiesOnSight = new List<PjBase>();
    public List<PjBase> alliesOnSight = new List<PjBase>();
    public PjBase lowestEnemy;
    public PjBase closestEnemy;
    public float minWeight;
    public float maxWeight;
    public float minHeight;
    public float maxHeight;

    public Playstyle playstyle;

    public enum Playstyle
    {
        none, aggresive, neutral, defensive, pursuing
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        character = GetComponent<PjBase>();
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.speed = character.stats.spd;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = false;
        agent.angularSpeed = 300;
        agent.acceleration = character.stats.spd * 5;
        Destroy(character.UIManager.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (character.dashing)
        {
            agent.destination = transform.position;
        }

        foreach (PjBase unit in GameManager.Instance.pjList)
        {
            if (unit != null)
            {
                var dir = unit.transform.position - transform.position;
                if (dir.magnitude <= viewDistance && !Physics2D.Raycast(transform.position, dir, dir.magnitude, GameManager.Instance.wallLayer))
                {
                    if (unit.team != character.team)
                    {
                        if (!enemiesOnSight.Contains(unit))
                        {
                            enemiesOnSight.Add(unit);
                        }
                        if (lowestEnemy == null || lowestEnemy.stats.hp > unit.stats.hp)
                        {
                            lowestEnemy = unit;
                        }

                        if (closestEnemy == null)
                        {
                            closestEnemy = unit;
                        }
                        else
                        {
                            Vector3 dist = unit.transform.position - transform.position;
                            Vector3 closestDist = closestEnemy.transform.position - transform.position;
                            if(dist.magnitude < closestDist.magnitude)
                            {
                                closestEnemy = unit;
                            }
                        }
                    }
                    else
                    {
                        if (!alliesOnSight.Contains(unit))
                        {
                            alliesOnSight.Add(unit);
                        }
                    }
                }
                else
                {
                    if (unit.team != character.team)
                    {
                        if (enemiesOnSight.Contains(unit))
                        {
                            if(lowestEnemy == unit)
                            {
                                StartCoroutine(OnLowestLost());
                            }
                            if(closestEnemy == unit)
                            {
                                closestEnemy = null;
                            }

                            enemiesOnSight.Remove(unit);
                        }
                    }
                    else
                    {
                        if (alliesOnSight.Contains(unit))
                        {
                            alliesOnSight.Remove(unit);
                        }
                    }
                }
            }
        }

        float enemiesHp = 0;
        float enemiesFullHp = 0;
        float alliesHp = 0;

        foreach (PjBase unit in enemiesOnSight)
        {
            if (unit != null)
            {
                enemiesHp += unit.stats.hp;
                enemiesFullHp += unit.stats.mHp;
            }
        }

        foreach (PjBase unit in alliesOnSight)
        {
            if (unit != null)
            {
                alliesHp += unit.stats.hp;
            }
        }

        if (enemiesHp > 0)
        {
            if (enemiesHp - 20 > alliesHp)
            {
                if (playstyle != Playstyle.defensive)
                {
                    agent.destination = transform.position;
                }
                playstyle = Playstyle.defensive;
            }
            else if (enemiesHp + 20 < alliesHp || enemiesFullHp * 0.35f >= enemiesHp)
            {
                if (playstyle != Playstyle.aggresive)
                {
                    agent.destination = transform.position;
                }
                playstyle = Playstyle.aggresive;
            }
            else
            {
                if (playstyle != Playstyle.neutral)
                {
                    agent.destination = transform.position;
                }
                playstyle = Playstyle.neutral;
            }
        }
        else
        {
            playstyle = Playstyle.none;
        }

    }

    public virtual void Look(Vector3 position)
    {
        position = position - transform.position;
        character.pointer.transform.up = position;
    }
    public virtual void LookReverse(Vector3 position)
    {
        position =  transform.position - position;
        character.pointer.transform.up = position;
    }
    public virtual bool InRange(GameObject target, float range)
    {
        if (target != null)
        {
            Vector3 dist = target.transform.position - transform.position;

            if (dist.magnitude > range)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else { return false; }
    }

    public virtual void IA()
    {
        if (enemiesOnSight.Count >1)
        {
            if (lowestEnemy == null || closestEnemy == null)
            {
                foreach (PjBase unit in enemiesOnSight)
                {
                    if (unit != null)
                    {
                        if (lowestEnemy == null || lowestEnemy.stats.hp > unit.stats.hp)
                        {
                            lowestEnemy = unit;
                        }
                        if (closestEnemy == null)
                        {
                            closestEnemy = unit;
                        }
                        else
                        {
                            Vector3 dist = unit.transform.position - transform.position;
                            Vector3 closestDist = closestEnemy.transform.position - transform.position;
                            if (dist.magnitude < closestDist.magnitude)
                            {
                                closestEnemy = unit;
                            }
                        }
                    }
                }
            }
        }
    }

    public virtual IEnumerator RestartIA()
    {
        yield return null;
        IA();
    }

    public virtual void PivotBackwards()
    {
        Vector3 point = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), transform.position.z);
        point += transform.position;

        Vector3 dist = closestEnemy.transform.position - transform.position;
        Vector3 pivotDist = closestEnemy.transform.position - point;

        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, 100, 1);
        agent.destination = point;

        while (dist.magnitude > pivotDist.magnitude+1 && agent.remainingDistance > 5)
        {
            point = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), transform.position.z);
            point += transform.position;

            dist = closestEnemy.transform.position - transform.position;
            pivotDist = closestEnemy.transform.position - point;

            NavMesh.SamplePosition(point, out hit, 100, 1);
            agent.destination = point;
        }

        NavMesh.SamplePosition(point, out hit, 100, 1);
        agent.destination = point;
    }
    public virtual void PivotForwards()
    {
        Vector3 point = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), transform.position.z);
        point += transform.position;

        Vector3 dist = closestEnemy.transform.position - transform.position;
        Vector3 pivotDist = closestEnemy.transform.position - point;

        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, 100, 1);
        agent.destination = point;

        while (dist.magnitude < pivotDist.magnitude -1 && agent.remainingDistance > 5)
        {
            point = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), transform.position.z);
            point += transform.position;

            dist = closestEnemy.transform.position - transform.position;
            pivotDist = closestEnemy.transform.position - point;

            NavMesh.SamplePosition(point, out hit, 100, 1);
            agent.destination = point;
        }

        NavMesh.SamplePosition(point, out hit, 100, 1);
        agent.destination = point;
    }

    public virtual IEnumerator OnLowestLost()
    {
        if (playstyle == Playstyle.aggresive || playstyle == Playstyle.neutral && enemiesOnSight.Count <1)
        {
            playstyle = Playstyle.pursuing;
            yield return null;
            agent.destination = lowestEnemy.transform.position;
        }
        lowestEnemy = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
