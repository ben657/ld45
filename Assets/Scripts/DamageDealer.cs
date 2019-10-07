using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float knockback = 0.0f;
    public StatType primaryStat = StatType.STR;
    public float statModifier = 1.0f;
    public Unit owner;
    public bool friendlyFire = false;

    bool isHero = false;
    bool isMonster = false;

    private void Start()
    {
        if (owner)
        {
            isHero = owner.GetComponent<HeroUnit>() != null;
            isMonster = owner.GetComponent<MonsterUnit>() != null;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner.gameObject) return;
        if(!friendlyFire)
        {
            if (isHero && collision.gameObject.GetComponent<HeroUnit>()) return;
            if (isMonster && collision.gameObject.GetComponent<MonsterUnit>()) return;
        }

        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit)
        {
            float damage = statModifier;
            if (owner) damage *= owner.GetStats().GetStat(primaryStat);
            int damageActual = (int)damage;
            if (damage <= 0) damageActual = 1;
            unit.Damage(damageActual, transform.forward * knockback);
            Debug.Log("Dealt " + damageActual + " damage");
        }
        Destroy(gameObject);
    }
}
