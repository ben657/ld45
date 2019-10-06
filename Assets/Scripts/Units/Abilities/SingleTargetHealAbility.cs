using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetHealAbility : Ability
{
    public string animationTrigger = "Attack2";
    public float healMultiplier = 0.0f;

    public override void Activate()
    {
        base.Activate();
        unit.GetAnimator().SetTrigger(animationTrigger);
    }

    public void AnimationDamageEvent()
    {
        if (!unit.GetTarget()) return;
        unit.GetTarget().Heal(unit.GetStats().intelligence * healMultiplier);
    }

    public void AnimationAttackEndEvent()
    {
        active = false;
    }
}
