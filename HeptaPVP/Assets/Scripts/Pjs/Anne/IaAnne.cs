using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;

public class IaAnne : IABase
{
    [HideInInspector]
    public Anne anne;



    public override void Start()
    {
        base.Start();
        anne = GetComponent<Anne>();
    }





    public override void AgressiveBehaviour()
    {
        base.AgressiveBehaviour();
        Look(lowestEnemy.transform.position);
        if (anne.h2AttacksCounter <= 0 && anne.currentHab2Cd <= 0 && !InRange(lowestEnemy.gameObject, anne.h2Prerange + 5))
        {
            anne.Hab2();
        }
        else if (InRange(lowestEnemy.gameObject, anne.h2Prerange) && anne.h2AttacksCounter > 0)
        {
            anne.Hab2();
        }


        if (anne.currentHab3Cd <= 0 && !InRange(lowestEnemy.gameObject, anne.h3Range + 4) && IsPjAsolated(lowestEnemy))
        {
            StartCoroutine(DashForward());
        }
        else if (anne.h2AttacksCounter > 0 && InRange(lowestEnemy.gameObject, anne.h2Range + anne.h2Prerange))
        {
            anne.MainAttack();
        }
        else if (anne.currentHab1Cd <= 0 && InRange(lowestEnemy.gameObject, anne.h1Range) && !anne.IsSoftCasting())
        {
            anne.Hab1();
        }
        else if (InRange(lowestEnemy.gameObject, anne.aRange))
        {
            anne.MainAttack();
        }


        if (GetRemainingDistance() < 1f)
        {
            if (InRange(closestEnemy.gameObject, anne.h2Prerange + 3) && anne.h2AttacksCounter > 0)
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

    public override void NeutralBehaviour()
    {
        base.NeutralBehaviour();
        Look(closestEnemy.transform.position);
        if (anne.h2AttacksCounter <= 0 && anne.currentHab2Cd <= 0 && !InRange(closestEnemy.gameObject, anne.h2Prerange))
        {
            anne.Hab2();
        }
        else if (InRange(closestEnemy.gameObject, anne.h2Prerange) && anne.h2AttacksCounter > 0)
        {
            anne.Hab2();
        }

        if (anne.currentHab3Cd <= 0 && InRange(closestEnemy.gameObject, anne.h3Range - 2))
        {
            StartCoroutine(DashBackward());
        }
        else if (anne.currentHab1Cd <= 0 && InRange(closestEnemy.gameObject, anne.h1Range) && !anne.IsSoftCasting())
        {
            anne.Hab1();
        }
        else if (anne.h2AttacksCounter > 0 && InRange(closestEnemy.gameObject, anne.h2Range + anne.h2Prerange))
        {
            anne.MainAttack();
        }
        else
        {
            anne.MainAttack();
        }

        if (GetRemainingDistance() < 1f)
        {
            if (InRange(closestEnemy.gameObject, anne.h2Prerange + 3))
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
        if (anne.h2AttacksCounter <= 0 && anne.currentHab2Cd <= 0 && !InRange(closestEnemy.gameObject, anne.h2Prerange))
        {
            anne.Hab2();
        }
        else if (InRange(closestEnemy.gameObject, anne.h2Prerange) && anne.h2AttacksCounter > 0)
        {
            anne.Hab2();
        }

        if (anne.currentHab3Cd <= 0 && InRange(closestEnemy.gameObject, anne.h3Range - 2))
        {
            StartCoroutine(DashBackward());
        }
        else if (anne.currentHab1Cd <= 0 && InRange(closestEnemy.gameObject, anne.h1Range) && !anne.IsSoftCasting())
        {
            anne.Hab1();
        }
        else if (anne.h2AttacksCounter > 0 && InRange(closestEnemy.gameObject, anne.h2Range + anne.h2Prerange))
        {
            anne.MainAttack();
        }
        else if (InRange(closestEnemy.gameObject, anne.aRange))
        {
            anne.MainAttack();
        }


        if (GetRemainingDistance() < 1f)
        {
            PivotBackwards();
        }

        StartCoroutine(RestartIA());
    }

    IEnumerator DashBackward()
    {
        if (!character.IsCasting() && !character.IsDashing())
        {
            LookReverse(closestEnemy.transform.position);
            yield return null;
            anne.Hab3();
            yield return null;
            if (lowestEnemy != null)
            {
                Look(lowestEnemy.transform.position);
                anne.MainAttack();
            }
            else if (closestEnemy != null)
            {
                Look(closestEnemy.transform.position);
                anne.MainAttack();
            }
        }
    }
    IEnumerator DashForward()
    {
        if (!character.IsCasting() && !character.IsDashing())
        {
            Look(lowestEnemy.transform.position);
            yield return null;
            anne.Hab3();
            yield return null;
            if (lowestEnemy != null)
            {
                Look(lowestEnemy.transform.position);
                anne.MainAttack();
            }
            else if (closestEnemy != null)
            {
                Look(closestEnemy.transform.position);
                anne.MainAttack();
            }
        }
    }
}
