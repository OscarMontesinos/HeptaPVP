using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PjBase : MonoBehaviour, TakeDamage
{
    public UIManager UIManager;
    public GameManager manager;
    public PlayerController controller;
    public GameObject sprite;
    public string chName;
    public int team; 
    public GameObject pointer;
    public HitData.Element element;
    public GameObject spinObjects;
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    public Slider stunnBar;
    public Sprite hab1Image;
    public Sprite hab2Image;
    public Sprite hab3Image;
    [HideInInspector]
    public bool isActive;
    [HideInInspector]
    public float currentComboReset;
    [HideInInspector]
    public float currentHab1Cd;
    public float hab1Cd;
    [HideInInspector]
    public float currentHab2Cd;
    public float hab2Cd;
    [HideInInspector]
    public float currentHab3Cd;
    public float hab3Cd;
    [HideInInspector]
    public bool casting;
    [HideInInspector]
    public bool softCasting;
    [HideInInspector]
    public bool dashing;
    [HideInInspector]
    public float stunTime;
    [HideInInspector]
    public bool ignoreSoftCastDebuff;
    public Stats stats;
    public float damageTextOffset;
    [HideInInspector]
    public float dmgDealed;

    float healCount;
    public enum AttackType
    {
        Physical, Magical, None
    }

    public virtual void Awake()
    {
        controller =GetComponent<PlayerController>();
    }
    public virtual void Start()
    {
        stats.hp = stats.mHp;

        GameManager.Instance.pjList.Add(this);
        
    }
    public virtual void Update()
    {
        if (isActive)
        {
            if (stunTime > 0)
            {
                stunTime -= Time.deltaTime;
                if (stunnBar.maxValue < stunTime)
                {
                    stunnBar.maxValue = stunTime;
                }

                stunnBar.value = stunTime;
            }
            else
            {
                stunnBar.maxValue = 0.3f;
                stunnBar.value = 0;
            }
        }

        if (currentComboReset > 0)
        {
            currentComboReset -= Time.deltaTime;
        }

        if (currentHab1Cd > 0)
        {
            currentHab1Cd -= Time.deltaTime;
        }

        if (currentHab2Cd > 0)
        {
            currentHab2Cd -= Time.deltaTime;
        }

        if (currentHab3Cd > 0)
        {
            currentHab3Cd -= Time.deltaTime;
        }


        if (spinObjects != null)
        {
            spinObjects.transform.rotation = pointer.transform.rotation;
        }

    }

    public virtual void Activate(bool active)
    {
        GetComponent<Collider2D>().enabled = active;
        isActive = active;
        sprite.SetActive(active);
    }
    public virtual void MainAttack()
    {
        
    }

    public virtual void StrongAttack()
    {
        
    }

    public virtual void Hab1()
    {
        
    }

    public virtual void Hab2()
    {
        
    }

    public virtual void Hab3()
    {
        
    }

    public virtual void UsedBasicDash()
    { 
    
    }
    public virtual void EndedBasicDash()
    { 
    
    }
    public virtual void UsedBasicDashGlobal()
    { 
    
    }
    public virtual void EndedBasicDashGlobal()
    { 
    
    }

    public IEnumerator Cast(float time)
    {
        casting = true;
        yield return new WaitForSeconds(time);
        casting = false;
    }
    public virtual IEnumerator SoftCast(float time)
    {
        softCasting = true;
        yield return new WaitForSeconds(time);
        softCasting = false;
    }

    public bool IsCasting()
    {
        if(!casting && !softCasting)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsStunned()
    {
        if (stunTime > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RegisterDamage(float dmg)
    {
        if (controller != null)
        {
            dmgDealed += dmg;
            UIManager.UpdateDamageText();
        }
    }

    public virtual void DamageDealed(PjBase user, PjBase target, float amount, HitData.Element element, HitData.AttackType attackType, HitData.HabType habType)
    {
        List<HitInteract> hitList = new List<HitInteract>( target.gameObject.GetComponents<HitInteract>());
        foreach (HitInteract hit in hitList)
        {
            hit.Interact(user,target,amount,element,attackType,habType);
        }
        
    }

    void TakeDamage.TakeDamage(PjBase user,float value, HitData.Element element, AttackType type)
    {
        TakeDmg(user, value, element, type);
    }

    public virtual void TakeDmg(PjBase user,float value, HitData.Element element, AttackType type)
    {
        if (isActive)
        {
            float calculo = 0;
            DamageText dText = null;
            switch (element)
            {
                case HitData.Element.ice:
                    dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    dText.textColor = GameManager.Instance.iceColor;
                    break;
                case HitData.Element.fire:
                    dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    dText.textColor = GameManager.Instance.fireColor;
                    break;
                case HitData.Element.water:
                    dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    dText.textColor = GameManager.Instance.waterColor;
                    break;
                case HitData.Element.blood:
                    dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    dText.textColor = GameManager.Instance.bloodColor;
                    break;
                case HitData.Element.desert:
                    dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    dText.textColor = GameManager.Instance.desertColor;
                    break;
                case HitData.Element.wind:
                    dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    dText.textColor = GameManager.Instance.windColor;
                    break;
                case HitData.Element.nature:
                    dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    dText.textColor = GameManager.Instance.natureColor;
                    break;
                case HitData.Element.lightning:
                    dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    dText.textColor = GameManager.Instance.lightningColor;
                    break;
            }

            if (type == AttackType.Magical)
            {
                calculo = stats.mResist + stats.resist;
            }
            else
            {
                calculo = stats.fResist + stats.resist;
            }

            if (calculo < 0)
            {
                calculo = 0;
            }
            value -= ((value * ((calculo / (100 + calculo) * 100))) / 100);
            float originalValue = value;
            if (controller != null)
            {
                while (Shield.shieldAmount > 0 && value > 0)
                {
                    Shield chosenShield = null;
                    foreach (Shield shield in controller.GetComponents<Shield>())
                    {
                        if (chosenShield == null || shield.time < chosenShield.time && shield.singularShieldAmount > 0)
                        {
                            chosenShield = shield;
                        }
                    }
                    value = chosenShield.ChangeShieldAmount(-value);

                }

                /*if(value != originalValue)
                {
                    originalValue -= value;
                    DamageText sText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                    sText.textColor = Color.white;
                    sText.damageText.text = originalValue.ToString("F0");
                }*/
            }


            dText.damageText.text = value.ToString("F0");

            stats.hp -= value;
            user.RegisterDamage(value);
            if (stats.hp <= 0)
            {
                GetComponent<TakeDamage>().Die();
            }
            if (hpBar != null)
            {
                hpBar.maxValue = stats.mHp;
                hpBar.value = stats.hp;
                hpText.text = stats.hp.ToString("F0");
            }
        }
    }

    public virtual void Heal(PjBase user, float value, HitData.Element element)
    {
        if (stats.hp > 0)
        {
            stats.hp += value;
            if (stats.hp > stats.mHp)
            {
                stats.hp = stats.mHp;
            }

            if (value + healCount > 1)
            {
                value += healCount;
                healCount = 0;
                DamageText dText = null;
                switch (element)
                {
                    case HitData.Element.ice:
                        dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                        dText.textColor = GameManager.Instance.iceColor;
                        break;
                    case HitData.Element.fire:
                        dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                        dText.textColor = GameManager.Instance.fireColor;
                        break;
                    case HitData.Element.water:
                        dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                        dText.textColor = GameManager.Instance.waterColor;
                        break;
                    case HitData.Element.blood:
                        dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                        dText.textColor = GameManager.Instance.bloodColor;
                        break;
                    case HitData.Element.desert:
                        dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                        dText.textColor = GameManager.Instance.desertColor;
                        break;
                    case HitData.Element.wind:
                        dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                        dText.textColor = GameManager.Instance.windColor;
                        break;
                    case HitData.Element.nature:
                        dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                        dText.textColor = GameManager.Instance.natureColor;
                        break;
                    case HitData.Element.lightning:
                        dText = Instantiate(GameManager.Instance.damageText, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(damageTextOffset - 0.5f, damageTextOffset + 0.5f), 0), transform.rotation).GetComponent<DamageText>();
                        dText.textColor = GameManager.Instance.lightningColor;
                        break;
                }

                dText.damageText.text = "+" + value.ToString("F0");
            }
            else
            {
                healCount += value;
            }
        }

    }

    public virtual void Stunn(PjBase target, float value)
    {
        target.GetComponent<TakeDamage>().Stunn(value);
    }

    public virtual void OnGlobalStunn(PjBase target, float value)
    {

    }

    public virtual void OnGlobalDamageTaken()
    {

    }
    public virtual void OnDamageTaken()
    {

    }

    public virtual void Moving(float magnitude)
    {

    }

    public virtual void GlobalMoving(float magnitude, PjBase user)
    {

    }
    void TakeDamage.Stunn(float stunTime)
    {
        this.stunTime += stunTime;
    }
    void TakeDamage.Die()
    {
        Destroy(gameObject);
    }

    public virtual float CalculateSinergy(float calculo)
    {
        float value = stats.sinergy;
        value *= calculo / 100;
        //valor.text = value.ToString();
        return value;

    }

    public virtual float CalculateControl(float calculo)
    {
        float value = stats.control;
        value *= calculo / 100;
        //valor.text = value.ToString();
        return value;
    }
    public float CDR(float value)
    {
        value -= ((value * ((stats.cdr / (100 + stats.cdr)))));
        return value;
    }
    public float CalculateAtSpd(float value)
    {
        value = 1 / value;
        return value;
    }

    public virtual IEnumerator Dash(Vector2 direction, float speed, float range, bool isBasicDash)
    {
        yield return null;
        StartCoroutine(Dash(direction, speed, range, isBasicDash, false, true));
    }
    public virtual IEnumerator Dash(Vector2 direction, float speed, float range, bool isBasicDash, bool ignoreWalls, bool shutDownCollider)
    {
        if (isBasicDash)
        {
            speed += stats.spd;
        }
        if (!shutDownCollider)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
        }
        dashing = true;
        Vector2 destinyPoint = Physics2D.Raycast(transform.position, direction, range, GameManager.Instance.wallLayer).point;
        yield return null;
        if (destinyPoint == new Vector2(0, 0))
        {
            destinyPoint = new Vector2(transform.position.x, transform.position.y) + direction.normalized * range;
        }
        Vector2 distance = destinyPoint - new Vector2(transform.position.x, transform.position.y);
        yield return null;
        while (distance.magnitude > 1 && dashing && stunTime <= 0)
        {
            if (distance.magnitude > 0.7)
            {
                controller.rb.velocity = distance.normalized * speed;
            }
            else
            {
                controller.rb.velocity = distance * speed;
            }
            distance = destinyPoint - new Vector2(transform.position.x, transform.position.y);
            yield return null;
        }
        dashing = false;
        GetComponent<Collider2D>().enabled = true;
        controller.rb.velocity = new Vector2(0, 0);
        if (!shutDownCollider)
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    public virtual IEnumerator Dash(Vector2 direction, float speed, float range, bool isBasicDash, bool ignoreWalls)
    {
        if (isBasicDash)
        {
            speed += stats.spd;
        }
        GetComponent<Collider2D>().enabled = false;
        dashing = true;

        Vector2 destinyPoint = new Vector2(transform.position.x, transform.position.y) + direction.normalized * range;

        Vector2 distance = destinyPoint - new Vector2(transform.position.x, transform.position.y);
        yield return null;
        while (distance.magnitude > 1 && dashing && stunTime <= 0)
        {
            if (distance.magnitude > 0.7)
            {
                controller.rb.velocity = distance.normalized * speed;
            }
            else
            {
                controller.rb.velocity = distance * speed;
            }
            distance = destinyPoint - new Vector2(transform.position.x, transform.position.y);
            yield return null;
        }
        dashing = false;
        GetComponent<Collider2D>().enabled = true;
        controller.rb.velocity = new Vector2(0, 0);
    }

    public void AnimationCursorLock(int value)
    {
        if (value == 1)
        {
            controller.LockPointer(true);
        }
        else
        {
            controller.LockPointer(false);
        }
    }

    public virtual void OnKill(PjBase target)
    {

    }

    public virtual void Interact(PjBase user, PjBase target, float amount, HitData.Element element, HitData.AttackType attackType, HitData.HabType habType)
    {

    }
}