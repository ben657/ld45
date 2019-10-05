using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMovementController : MonoBehaviour
{
    protected Unit unit;
    protected NavMeshAgent agent;

    public float minVelocity = 0.02f;

    bool lastMoving = false;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public virtual void Move(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public virtual void StopMoving()
    {
        agent.SetDestination(transform.position);
    }

    public bool IsMoving()
    {
        return agent.velocity.sqrMagnitude > minVelocity * minVelocity;
    }

    public virtual void OnTargetChanged()
    {

    }

    protected virtual void OnStartedMoving()
    {
        unit.GetAnimator().SetBool("Moving", true);
    }

    protected virtual void OnStoppedMoving()
    {
        agent.ResetPath();
        unit.GetAnimator().SetBool("Moving", false);
    }

    protected virtual void Update()
    {
        bool moving = IsMoving();
        if (moving && !lastMoving) OnStartedMoving();
        if (!moving && lastMoving) OnStoppedMoving();
        lastMoving = moving;
    }

    private void OnDrawGizmosSelected()
    {
        if(agent && IsMoving())
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(agent.destination, 0.5f);
        }
    }
}
