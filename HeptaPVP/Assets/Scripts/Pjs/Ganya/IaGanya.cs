using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaGanya : IABase
{

    Ganya ganya;

    public override void Start()
    {
        base.Start();
        ganya = GetComponent<Ganya>();
    }

    public override void AgressiveBehaviour()
    {
        base.AgressiveBehaviour();

        Look(lowestEnemy.transform.position);

        if (ganya.aCurrentPassiveActiveSeconds > 0 && InRange(lowestEnemy.gameObject, ganya.aRange))
        {
            ganya.MainAttack();
        }
        else if(ganya.h1Charges > 1 && InRange(lowestEnemy.gameObject, ganya.h1Range * 1.7f) && !ganya.IsSoftCasting())
        {
            if(InRange(lowestEnemy.gameObject, ganya.h1Range))
            {
                StartCoroutine(DashAround(lowestEnemy));
            }
            else
            {
                ganya.Hab1();
            }
        }
        else if(ganya.currentHab2Cd <= 0 && InRange(lowestEnemy.gameObject, ganya.h2Area * 2))
        {
            ganya.Hab2();
        }
        else if(ganya.aCurrentPassiveActiveSeconds <= 0 && ganya.currentHab3Cd <= 0 && InRange(lowestEnemy.gameObject, ganya.h3Height))
        {
            ganya.Hab3();
        }
        else if(ganya.h1Charges > 0 && InRange(lowestEnemy.gameObject, ganya.h1Range * 1.7f) && !ganya.IsSoftCasting())
        {

            if (InRange(lowestEnemy.gameObject, ganya.h1Range))
            {
                StartCoroutine(DashAround(lowestEnemy));
            }
            else
            {
                ganya.Hab1();
            }
        }
        else if (InRange(lowestEnemy.gameObject, ganya.aRange))
        {
            ganya.MainAttack();
        }


        if (GetRemainingDistance() < 1f || !InRange(lowestEnemy.gameObject, ganya.aRange - 3))
        {
            if (InRange(lowestEnemy.gameObject, ganya.aRange - 3))
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

        if (ganya.aCurrentPassiveActiveSeconds > 0 && InRange(closestEnemy.gameObject, ganya.aRange))
        {
            ganya.MainAttack();
        }
        else if (ganya.h1Charges > 1 && InRange(closestEnemy.gameObject, ganya.h1Range) && !ganya.IsSoftCasting() && IsPjAsolated(closestEnemy))
        {
            StartCoroutine(DashAround(closestEnemy));
        }
        else if (ganya.currentHab2Cd <= 0 && InRange(closestEnemy.gameObject, ganya.h2Area * 2))
        {
            ganya.Hab2();
        }
        else if (ganya.aCurrentPassiveActiveSeconds <= 0 && ganya.currentHab3Cd <= 0 && InRange(closestEnemy.gameObject, ganya.h3Height))
        {
            ganya.Hab3();
        }
        else if (ganya.h1Charges > 0 && InRange(closestEnemy.gameObject, ganya.h1Range * 0.5f) && !ganya.IsSoftCasting())
        {
            StartCoroutine(DashBackwards(closestEnemy));
        }
        else if (InRange(closestEnemy.gameObject, ganya.aRange))
        {
            ganya.MainAttack();
        }


        if (GetRemainingDistance() < 1f)
        {
            if (InRange(closestEnemy.gameObject, ganya.aRange - 2))
            {
                PivotBackwards();
            }
            else
            {
                PivotForwards();
            }
        }


            StartCoroutine(RestartIA());
    }

    public override void DefensiveBehaviour()
    {
        base.DefensiveBehaviour();
        Look(closestEnemy.transform.position);

        if (ganya.aCurrentPassiveActiveSeconds > 0 && InRange(closestEnemy.gameObject, ganya.aRange))
        {
            ganya.MainAttack();
        }
        else if (ganya.currentHab2Cd <= 0 && InRange(closestEnemy.gameObject, ganya.h2Area * 2))
        {
            ganya.Hab2();
        }
        else if (ganya.aCurrentPassiveActiveSeconds <= 0 && ganya.currentHab3Cd <= 0 && InRange(closestEnemy.gameObject, ganya.h3Height))
        {
            ganya.Hab3();
        }
        else if (ganya.h1Charges > 0 && InRange(closestEnemy.gameObject, ganya.h1Range * 1.7f) && !ganya.IsSoftCasting())
        {
            StartCoroutine(DashBackwards(closestEnemy));
        }
        else if (InRange(closestEnemy.gameObject, ganya.aRange))
        {
            ganya.MainAttack();
        }


        if (GetRemainingDistance() < 1f)
        {
            RunAway();
        }
        StartCoroutine(RestartIA());

    }

    public IEnumerator DashAround(PjBase target)
    {
        Vector3 dir = target.transform.position;
        int random = Random.Range(0, 2);
        if(random > 0)
        {
            dir = new Vector3(dir.x + ganya.h1Range * 0.6f, dir.y, 0);
        }
        else
        {
            dir = new Vector3(dir.x - ganya.h1Range * 0.6f, dir.y, 0);
        }
        random = Random.Range(0, 2);
        if(random > 0)
        {
            dir = new Vector3(dir.x , dir.y + ganya.h1Range * 0.6f, 0);
        }
        else
        {
            dir = new Vector3(dir.x , dir.y - ganya.h1Range * 0.6f, 0);
        }
        yield return null;
        
        ganya.Hab1();
    }
    public IEnumerator DashBackwards(PjBase target)
    {
        LookReverse(target.transform.position);
        yield return null;
        ganya.Hab1();
    }

}
