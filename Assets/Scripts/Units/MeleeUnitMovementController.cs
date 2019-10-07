using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnitMovementController : UnitMovementController
{
    public float targetDistance = 0.5f;
    public float stopOffset = 0.2f;

    float stopDist2 = 0.0f;

    void Start()
    {
        //targetDistance = unit.GetBounds().size.magnitude;
        float stopDist = targetDistance + stopOffset;
        stopDist2 = stopDist * stopDist;
    }

    protected override void Update()
    {
        base.Update();

        Unit target = unit.GetTarget();
        if (!target && leader) target = leader;
        if(target)
        {
            Vector3 targetPos = target.GetPointOnBounds(transform.position);
            Vector3 between = transform.position - targetPos;
            if (between.sqrMagnitude < stopDist2)
            {
                StopMoving();
                return;
            }
            Vector3 towardsSelf = between.normalized;
            targetPos += towardsSelf * targetDistance;
            Move(targetPos);
        }
    }
}
