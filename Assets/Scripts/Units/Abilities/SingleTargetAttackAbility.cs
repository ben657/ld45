using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetAttackAbility : Ability
{
    public string animationTrigger = "Attack";
    public float knockback = 10.0f;
    public override void Activate()
    {
        base.Activate();
        unit.GetAnimator().SetTrigger(animationTrigger);
    }

    public void AnimationDamageEvent()
    {
        Debug.Log("Ran");
        unit.GetTarget().GetMovementController().GetAgent().velocity = transform.forward * knockback;
    }

    public void AnimationAttackEndEvent()
    {
        active = false;
    }
}
