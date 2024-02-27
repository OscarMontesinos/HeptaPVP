using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class Barrier : MonoBehaviour, TakeDamage
{
    public PjBase user;
    public float hp;
    float mHp;
    public float duration;
    public Slider hpBar;
    public bool deniesVision;


    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            GetComponent<TakeDamage>().Die(null);
        }

        if (hpBar != null)
        {
            hpBar.maxValue = mHp;
            hpBar.value = hp;
        }
    }
    public virtual void SetUp(PjBase user, float hp, float duration, bool deniesVision)
    {
        this.user = user;
        this.hp = hp;
        this.duration = duration;
        mHp = hp;
        this.deniesVision = deniesVision;
    }
    void TakeDamage.Die(PjBase killer)
    {
        Destroy(gameObject);
    }

    void TakeDamage.Stunn(float stunnTime)
    {

    }

    void TakeDamage.TakeDamage(PjBase user, float value, HitData.Element element, PjBase.AttackType type)
    {
        hp -= value;
        if (hp <= 0)
        {
            GetComponent<TakeDamage>().Die(user);
        }
    }
}
