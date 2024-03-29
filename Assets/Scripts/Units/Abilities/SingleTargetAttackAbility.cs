﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetAttackAbility : Ability
{
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
        Unit target = unit.GetTarget();
        if (!target) return;
        target.Damage(unit.GetStats().strength * damageMultiplier, transform.forward * knockback);
    }

    public void AnimationAttackEndEvent()
    {
        active = false;
    }
}
