using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;

public class IaNamir : IABase
{
    [HideInInspector]
    public Namir namir;

    

    public override void Start()
    {
        base.Start();
        namir = GetComponent<Namir>();
        IA();
    }

    public override void IA()
    {
        base.IA();

        if ((closestEnemy == null || lowestEnemy == null) && playstyle != Playstyle.none && playstyle != Playstyle.pursuing)
        {
            playstyle = Playstyle.none;
            StartCoroutine(RestartIA());
            return;
        }
        if(namir.stunTime > 0)
        {
            agent.speed = 0;
            StartCoroutine(RestartIA());
            return;
        }

        if (namir.currentHab3Cd <= 0)
        {
            namir.Hab3();
        }

        if (playstyle == Playstyle.aggresive)
        {
            Look(lowestEnemy.transform.position);

            if(InRange(lowestEnemy.gameObject, namir.aArea + 4))
            {
                namir.Hab2();
            }

            if (InRange(lowestEnemy.gameObject, namir.aArea + 2f))
            {
                if (lowestEnemy.stats.hp < namir.CalculateStrength(namir.h1Dmg3) - 5 && namir.h1AttacksCounter == 2)
                {

                    namir.Hab1();
                }
                else
                {
                    namir.MainAttack();
                }
            }
            else if(InRange(lowestEnemy.gameObject, namir.h1Range3) && namir.h1AttacksCounter == 2 && !namir.h1Dashing && (!InRange(lowestEnemy.gameObject, namir.aArea + 1.5f) || namir.h3AttacksCounter <= 0))
            {
                namir.Hab1();
            }
            else if (InRange(lowestEnemy.gameObject, namir.h1Range2) && (namir.h1AttacksCounter == 1 || namir.h1AttacksCounter <= 0 && namir.currentHab1Cd <= 0) && !namir.h1Dashing)
            {
                namir.Hab1();
            }
            else if (InRange(lowestEnemy.gameObject, namir.h1Range1 * 1.7f) && namir.currentHab1Cd <= 0 && namir.h1AttacksCounter <= 0 && !namir.h1Dashing)
            {
                namir.Hab1();
            }

            if (GetRemainingDistance() < 1f || !InRange(lowestEnemy.gameObject, namir.aArea + 1))
            {
                if (InRange(closestEnemy.gameObject, namir.aArea))
                {
                    PivotBackwards();
                }
                else
                {
                        agent.SetDestination(lowestEnemy.transform.position);
                }
            }

            StartCoroutine(RestartIA());
        }
        else if (playstyle == Playstyle.neutral)
        {
            Look(lowestEnemy.transform.position);

            if (InRange(lowestEnemy.gameObject, 20))
            {
                namir.Hab2();
            }

            if ((namir.currentHab1Cd <= 0 || namir.h1AttacksCounter > 0) || InRange(lowestEnemy.gameObject, namir.aArea + 4f))
            {
                if (namir.h2ActiveCloud != null && namir.h2ActiveCloud.duration < namir.h2ActiveCloud.duration - namir.h2BuffDuration)
                {
                    if (InRange(lowestEnemy.gameObject, namir.aArea + 2f))
                    {
                        if (lowestEnemy.stats.hp < namir.CalculateStrength(namir.h1Dmg3) - 5 && namir.h1AttacksCounter == 2)
                        {

                            namir.Hab1();
                        }
                        else
                        {
                            namir.MainAttack();
                        }
                    }
                }
                else
                {
                    if (InRange(lowestEnemy.gameObject, namir.aArea + 2f))
                    {
                        if (lowestEnemy.stats.hp < namir.CalculateStrength(namir.h1Dmg3) - 5 && namir.h1AttacksCounter == 2)
                        {

                            namir.Hab1();
                        }
                        else
                        {
                            namir.MainAttack();
                        }
                    }
                    else if (InRange(lowestEnemy.gameObject, namir.h1Range3) && namir.h1AttacksCounter == 2 && !namir.h1Dashing && (!InRange(lowestEnemy.gameObject, namir.aArea + 1.5f) || namir.h3AttacksCounter <= 0))
                    {
                        namir.Hab1();
                    }
                    else if (InRange(lowestEnemy.gameObject, namir.h1Range2) && (namir.h1AttacksCounter == 1 || namir.h1AttacksCounter <= 0 && namir.currentHab1Cd <= 0) && !namir.h1Dashing)
                    {
                        namir.Hab1();
                    }
                    else if (InRange(lowestEnemy.gameObject, namir.h1Range1 * 1.7f) && namir.currentHab1Cd <= 0 && namir.h1AttacksCounter <= 0 && !namir.h1Dashing)
                    {
                        namir.Hab1();
                    }
                }


                if (GetRemainingDistance() < 1f || !InRange(closestEnemy.gameObject, namir.aArea))
                {
                    if (InRange(closestEnemy.gameObject, namir.aArea))
                    {
                        PivotBackwards();
                    }
                    else
                    {
                        agent.SetDestination(lowestEnemy.transform.position);
                    }
                }
            }
            else
            {
                if (GetRemainingDistance() < 1f)
                {
                    if (InRange(closestEnemy.gameObject, namir.h1Range1 * 2))
                    {
                        PivotBackwards();
                    }
                    else
                    {
                        PivotForwards();
                    }
                }
            }

            

            StartCoroutine(RestartIA());
        }
        else if (playstyle == Playstyle.defensive)
        {
            if (namir.stats.mHp * 0.35f > namir.stats.hp)
            {
                LookReverse(lowestEnemy.transform.position);

                if (namir.currentHab2Cd <= 0)
                {
                    namir.Hab2();
                }

                if (namir.currentHab1Cd <= 0 || namir.h1AttacksCounter > 0)
                {
                    namir.Hab1();
                }

                if (GetRemainingDistance() < 1f)
                {
                    RunAway();
                }
            }
            else
            {
                Look(lowestEnemy.transform.position);

                if (InRange(lowestEnemy.gameObject, 20))
                {
                    namir.Hab2();
                }

                if ((namir.currentHab1Cd <= 0 || namir.h1AttacksCounter > 0) || InRange(lowestEnemy.gameObject, namir.aArea + 4f))
                {
                    if (namir.h2ActiveCloud != null && namir.h2ActiveCloud.duration < namir.h2ActiveCloud.duration - namir.h2BuffDuration)
                    {
                        if (InRange(lowestEnemy.gameObject, namir.aArea + 2f))
                        {
                            if (lowestEnemy.stats.hp < namir.CalculateStrength(namir.h1Dmg3) - 5 && namir.h1AttacksCounter == 2)
                            {

                                namir.Hab1();
                            }
                            else
                            {
                                namir.MainAttack();
                            }
                        }
                    }
                    else
                    {
                        if (InRange(lowestEnemy.gameObject, namir.aArea + 2f))
                        {
                            if (lowestEnemy.stats.hp < namir.CalculateStrength(namir.h1Dmg3) - 5 && namir.h1AttacksCounter == 2)
                            {

                                namir.Hab1();
                            }
                            else
                            {
                                namir.MainAttack();
                            }
                        }
                        else if (InRange(lowestEnemy.gameObject, namir.h1Range3) && namir.h1AttacksCounter == 2 && !namir.h1Dashing && (!InRange(lowestEnemy.gameObject, namir.aArea + 1.5f) || namir.h3AttacksCounter <= 0))
                        {
                            namir.Hab1();
                        }
                        else if (InRange(lowestEnemy.gameObject, namir.h1Range2) && (namir.h1AttacksCounter == 1 || namir.h1AttacksCounter <= 0 && namir.currentHab1Cd <= 0) && !namir.h1Dashing)
                        {
                            namir.Hab1();
                        }
                        else if (InRange(lowestEnemy.gameObject, namir.h1Range1 * 1.7f) && namir.currentHab1Cd <= 0 && namir.h1AttacksCounter <= 0 && !namir.h1Dashing)
                        {
                            namir.Hab1();
                        }
                    }


                    if (GetRemainingDistance() < 1f || !InRange(closestEnemy.gameObject, namir.aArea))
                    {
                        if (InRange(closestEnemy.gameObject, namir.aArea))
                        {
                            PivotBackwards();
                        }
                        else
                        {
                            agent.SetDestination(lowestEnemy.transform.position);
                        }
                    }
                }
                else
                {
                    if (GetRemainingDistance() < 1f)
                    {
                        if (InRange(closestEnemy.gameObject, namir.h1Range1 * 2))
                        {
                            PivotBackwards();
                        }
                        else
                        {
                            PivotForwards();
                        }
                    }
                }
            }

                StartCoroutine(RestartIA());
        }
        else
        {
            if (GetRemainingDistance() < 1f || agent.velocity.magnitude <= 0.2f)
            {
                float randomWeight = Random.Range(minWeight, maxWeight);
                float randomHeight = Random.Range(minHeight, maxHeight);
                NavMeshHit hit;
                NavMesh.SamplePosition(new Vector3(randomWeight, randomHeight, transform.position.z ), out hit, 100, 1);
                agent.destination = hit.position;
            }
            StartCoroutine(RestartIA());
        }
        
    }

    IEnumerator DashBackward()
    {
        if (!character.IsCasting() && !character.IsDashing())
        {
            LookReverse(closestEnemy.transform.position);
            yield return null;
            namir.Hab3();
            yield return null;
            if (lowestEnemy != null)
            {
                Look(lowestEnemy.transform.position);
                namir.MainAttack();
            }
            else if(closestEnemy != null)
            {
                Look(closestEnemy.transform.position);
                namir.MainAttack();
            }
        }
    }
    IEnumerator DashForward()
    {
        if (!character.IsCasting() && !character.IsDashing())
        {
            Look(lowestEnemy.transform.position);
            yield return null;
            namir.Hab3();
            yield return null;
            if (lowestEnemy != null)
            {
                Look(lowestEnemy.transform.position);
                namir.MainAttack();
            }
            else if (closestEnemy != null)
            {
                Look(closestEnemy.transform.position);
                namir.MainAttack();
            }
        }
    }
}
