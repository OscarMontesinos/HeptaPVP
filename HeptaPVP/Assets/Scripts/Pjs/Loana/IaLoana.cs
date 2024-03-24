using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaLoana : IABase
{
    Loana loana;
    public override void Start()
    {
        base.Start();
        loana = GetComponent<Loana>();
    }

    public override void AgressiveBehaviour()
    {
        base.AgressiveBehaviour();

        Look(lowestEnemy.transform.position);
        if (loana.currentHab1Cd <=0 && InRange(lowestEnemy.gameObject, loana.h1Area * 1.5f))
        {
            loana.Hab1();
        }
        else if(loana.currentHab3Cd <= 0 && InRange(lowestEnemy.gameObject, loana.h3Range))
        {
            loana.Hab3();
        }
        else if(InRange(lowestEnemy.gameObject, loana.aArea * 1.5f))
        {
            loana.MainAttack();
        }

        if (GetRemainingDistance() < 1f || !InRange(lowestEnemy.gameObject, loana.aArea * 1.5f))
        {
            if (InRange(closestEnemy.gameObject, loana.aArea*1.5f))
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
        
        Look(closestEnemy.transform.position);
        if (loana.currentHab2Cd <= 0 && InRange(closestEnemy.gameObject, loana.h3Range) && !InRange(closestEnemy.gameObject, loana.h3Range * 0.25f))
        {
            loana.Hab2();
        }
        else if (loana.currentHab1Cd <= 0 && InRange(closestEnemy.gameObject, loana.h1Area * 1.5f))
        {
            loana.Hab1();
        }
        else if (loana.currentHab3Cd <= 0 && InRange(closestEnemy.gameObject, loana.h3Range))
        {
            loana.Hab3();
        }
        else if (InRange(closestEnemy.gameObject, loana.aArea * 1.5f))
        {
            loana.MainAttack();
        }

        if (GetRemainingDistance() < 1f)
        {
            if (lowestAlly == null)
            {
                if (InRange(closestEnemy.gameObject, loana.h1Area * 1.5f))
                {
                    if (InRange(closestEnemy.gameObject, loana.aArea * 1.5f))
                    {
                        PivotBackwards();
                    }
                    else
                    {
                        PivotForwards();
                    }
                }
                else
                {
                    if (InRange(closestEnemy.gameObject, loana.h3Range * 0.5f))
                    {
                        PivotBackwards();
                    }
                    else
                    {
                        PivotForwards();
                    }
                }
            }
            else
            {
                PivotAroundObject(lowestAlly.gameObject);
            }
        }

        StartCoroutine(RestartIA());
    }

    public override void DefensiveBehaviour()
    {
        base.DefensiveBehaviour();

        Look(closestEnemy.transform.position);
        if (loana.currentHab2Cd <= 0 && InRange(closestEnemy.gameObject, loana.h3Range) && !InRange(closestEnemy.gameObject, loana.h3Range * 0.25f))
        {
            loana.Hab2();
        }
        else if (loana.currentHab1Cd <= 0 && InRange(closestEnemy.gameObject, loana.h1Area * 1.5f))
        {
            loana.Hab1();
        }
        else if (loana.currentHab3Cd <= 0 && InRange(closestEnemy.gameObject, loana.h3Range))
        {
            loana.Hab3();
        }
        else if (InRange(closestEnemy.gameObject, loana.aArea * 1.5f))
        {
            loana.MainAttack();
        }

        if (GetRemainingDistance() < 1f)
        {
            PivotBackwards();
        }

        StartCoroutine(RestartIA());
    }
}
