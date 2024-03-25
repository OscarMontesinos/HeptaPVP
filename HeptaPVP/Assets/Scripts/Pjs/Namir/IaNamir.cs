using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.PlasticSCM.Editor.WebApi;
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
    }

    public override void IA()
    {
        if (namir.currentHab3Cd <= 0)
        {
            namir.Hab3();
        }

        base.IA();
    }

    public override void AgressiveBehaviour()
    {
        base.AgressiveBehaviour();
        Look(lowestEnemy.transform.position);

        if (InRange(lowestEnemy.gameObject, namir.aArea + 4))
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

    public override void NeutralBehaviour()
    {
        base.NeutralBehaviour();
        if (IsPjAsolated(lowestEnemy))
        {
            Look(lowestEnemy.transform.position);

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
        else if (IsPjAsolated(closestEnemy))
        {
            Look(closestEnemy.transform.position);

            if ((namir.currentHab1Cd <= 0 || namir.h1AttacksCounter > 0) || InRange(closestEnemy.gameObject, namir.aArea + 4f))
            {
                if (namir.h2ActiveCloud != null && namir.h2ActiveCloud.duration < namir.h2ActiveCloud.duration - namir.h2BuffDuration)
                {
                    if (InRange(closestEnemy.gameObject, namir.aArea + 2f))
                    {
                        if (closestEnemy.stats.hp < namir.CalculateStrength(namir.h1Dmg3) - 5 && namir.h1AttacksCounter == 2)
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
                    if (InRange(closestEnemy.gameObject, namir.aArea + 2f))
                    {
                        if (closestEnemy.stats.hp < namir.CalculateStrength(namir.h1Dmg3) - 5 && namir.h1AttacksCounter == 2)
                        {

                            namir.Hab1();
                        }
                        else
                        {
                            namir.MainAttack();
                        }
                    }
                    else if (InRange(closestEnemy.gameObject, namir.h1Range3) && namir.h1AttacksCounter == 2 && !namir.h1Dashing && (!InRange(closestEnemy.gameObject, namir.aArea + 1.5f) || namir.h3AttacksCounter <= 0))
                    {
                        namir.Hab1();
                    }
                    else if (InRange(closestEnemy.gameObject, namir.h1Range2) && (namir.h1AttacksCounter == 1 || namir.h1AttacksCounter <= 0 && namir.currentHab1Cd <= 0) && !namir.h1Dashing)
                    {
                        namir.Hab1();
                    }
                    else if (InRange(closestEnemy.gameObject, namir.h1Range1 * 1.7f) && namir.currentHab1Cd <= 0 && namir.h1AttacksCounter <= 0 && !namir.h1Dashing)
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
                        agent.SetDestination(closestEnemy.transform.position);
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
        else
        {
            if (namir.currentHab2Cd <=0 && InRange(closestEnemy.gameObject, namir.aArea * 2.5f))
            {
                namir.Hab2();
            }
            else if(InRange(closestEnemy.gameObject, namir.aArea * 1.5f))
            {
                namir.MainAttack();
            }
            if (GetRemainingDistance() < 1f || InRange(closestEnemy.gameObject, namir.h1Range1 * 3))
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

    public override void DefensiveBehaviour()
    {
        base.DefensiveBehaviour();
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
        

        StartCoroutine(RestartIA());
    }
}
