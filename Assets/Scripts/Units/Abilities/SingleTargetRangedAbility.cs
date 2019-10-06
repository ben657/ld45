using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetRangedAbility : Ability
{
    public Projectile projectilePrefab;

    public string animationTrigger = "Attack";
    public float knockback = 10.0f;
    public float damageMultiplier = 0.0f;

    public override void Activate()
    {
        base.Activate();
        unit.GetAnimator().SetTrigger(animationTrigger);
    }

    public void AnimationDamageEvent()
    {
        if (!unit.GetTarget()) return;
        Projectile projectile = Instantiate(projectilePrefab);
        projectile.transform.position = transform.position + Vector3.up * 1.0f;
        projectile.SetTarget(unit.GetTarget());
        projectile.GetComponent<DamageDealer>().owner = unit;
    }

    public void AnimationAttackEndEvent()
    {
        active = false;
    }
}
