using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class Barrier : MonoBehaviour, TakeDamage
{
    PjBase user;
    public float hp;
    float mHp;
    public float duration;
    public Slider hpBar;

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            GetComponent<TakeDamage>().Die();
        }

        if (hpBar != null)
        {
            hpBar.maxValue = mHp;
            hpBar.value = hp;
        }
    }
    public virtual void SetUp(PjBase user, float hp, float duration)
    {
        this.user = user;
        this.hp = hp;
        this.duration = duration;
        mHp = hp;
    }
    void TakeDamage.Die()
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
            GetComponent<TakeDamage>().Die();
        }
    }
}
