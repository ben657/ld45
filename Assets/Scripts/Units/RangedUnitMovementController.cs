using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnitMovementController : MeleeUnitMovementController
{
    public float minDistance = 5.0f;
    public float maxDistance = 20.0f;

    float minDist2 = 0.0f;
    float maxDist2 = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        minDist2 = minDistance * minDistance;
        maxDist2 = maxDistance * maxDistance;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Unit target = unit.GetTarget();
        if (target)
        {
            Vector3 targetPos = target.GetPointOnBounds(transform.position);
            Vector3 between = transform.position - targetPos;
            Vector3 towardsSelf = between.normalized;
            if (between.sqrMagnitude < maxDist2 && between.sqrMagnitude > minDist2)
            {
                StopMoving();
                transform.forward = -towardsSelf;
                return;
            }

            float targetDistance = (minDistance + maxDistance) * 0.5f;
            targetPos += towardsSelf * targetDistance;
            Move(targetPos);
        }
    }
}
