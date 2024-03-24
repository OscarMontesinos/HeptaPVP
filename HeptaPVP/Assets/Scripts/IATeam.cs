using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class IATeam : MonoBehaviour
{
    public int team;
    public List<PjBase> allies = new List<PjBase>();
    public List<PjBase> enemiesOnSight = new List<PjBase>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (PjBase unit in GameManager.Instance.pjList)
        {
            if (unit.team == team)
            {
                allies.Add(unit);
                if (unit.GetComponent<IABase>())
                {
                    if (unit.GetComponent<IABase>().team == null)
                    {
                        unit.GetComponent<IABase>().team = this;
                        unit.GetComponent<IABase>().IA();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    public void UpdateEnemiesOnSight(PjBase unit)
    {
        foreach(PjBase ally in allies)
        {
            if (ally != null && ally.GetComponent<IABase>())
            {
                IABase allyIA = ally.GetComponent<IABase>();
                foreach(PjBase enemy in allyIA.enemiesOnSight)
                {
                    if (enemiesOnSight.Contains(enemy))
                    {
                        return;
                    }
                }
            }
        }
        enemiesOnSight.Remove(unit);

        
    }
    public void AddEnemiesOnSight(PjBase unit)
    {
        if (!enemiesOnSight.Contains(unit))
        {
            enemiesOnSight.Add(unit);
        }
    }

}
