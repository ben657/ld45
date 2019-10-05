using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventPassthrough : MonoBehaviour
{
    Unit unit;
    UnitAbilityController abilityController;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        abilityController = unit.GetComponentInChildren<UnitAbilityController>();
    }

    public void AnimationDamageEvent()
    {
        abilityController.PassthroughMessage("AnimationDamageEvent");
    }

    public void AnimationAttackEndEvent()
    {
        abilityController.PassthroughMessage("AnimationAttackEndEvent");
    }
}
