using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TargettingType
{
    Nearest,
    Farthest,
    //Weakest,
    //Strongest,
    LeastHealth,
    MostHealth
}

[System.Serializable]
public struct UnitStats
{
    public float maxHealth;
    public float health;

    public int strength;
    public int intelligence;
    public int dexterity;

    public UnitStats(int str, int inte, int dex)
    {
        maxHealth = str * 1.5f;
        health = maxHealth;

        strength = str;
        intelligence = inte;
        dexterity = dex;
    }

    public float GetHealthPercentage()
    {
        return health / maxHealth;
    }
}

public class Unit : MonoBehaviour
{
    //Ability[] abilities;

    public TargettingType targettingType = TargettingType.Nearest;
    
    UnitMovementController movementController;
    UnitAbilityController abilityController;
    [SerializeField]
    Animator animator;
    [SerializeField]
    UnitStats stats;

    Unit target;
    HashSet<Unit> unitsInRange = new HashSet<Unit>();
    
    void Awake()
    {
        movementController = GetComponent<UnitMovementController>();
        abilityController = GetComponentInChildren<UnitAbilityController>();
    }

    public void SetStats(int str, int inte, int dex)
    {
        stats = new UnitStats(str, inte, dex);
    }

    void Start()
    {
        
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public UnitMovementController GetMovementController()
    {
        return movementController;
    }

    public UnitAbilityController GetAbilityController()
    {
        return abilityController;
    }

    public Unit GetTarget()
    {
        return target;
    }

    void UpdateTarget()
    {
        float bestMetric = 0;
        Unit bestUnit = null;
        if(targettingType == TargettingType.Nearest)
        {
            bestMetric = Mathf.Infinity;
            foreach(Unit unit in unitsInRange)
            {
                float dist2 = Vector3.SqrMagnitude(unit.transform.position - transform.position);
                if(dist2 < bestMetric)
                {
                    bestMetric = dist2;
                    bestUnit = unit;
                }
            }
        }
        else if (targettingType == TargettingType.Farthest)
        {
            bestMetric = 0.0f;
            foreach (Unit unit in unitsInRange)
            {
                float dist2 = Vector3.SqrMagnitude(unit.transform.position - transform.position);
                if (dist2 > bestMetric)
                {
                    bestMetric = dist2;
                    bestUnit = unit;
                }
            }
        }
        else if (targettingType == TargettingType.LeastHealth)
        {
            bestMetric = Mathf.Infinity;
            foreach (Unit unit in unitsInRange)
            {
                float health = unit.GetStats().GetHealthPercentage();
                if (health < bestMetric)
                {
                    bestMetric = health;
                    bestUnit = unit;
                }
            }
        }
        else if (targettingType == TargettingType.MostHealth)
        {
            bestMetric = 0.0f;
            foreach (Unit unit in unitsInRange)
            {
                float health = unit.GetStats().GetHealthPercentage();
                if (health > bestMetric)
                {
                    bestMetric = health;
                    bestUnit = unit;
                }
            }
        }

        target = bestUnit;

        if(movementController) movementController.OnTargetChanged();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateTarget();
    }

    public UnitStats GetStats()
    {
        return stats;
    }

    protected virtual bool ShouldTarget(Unit unit)
    {
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if(unit && unit != this)
        {
            unitsInRange.Add(unit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit && unitsInRange.Contains(unit))
            unitsInRange.Remove(unit);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach(Unit unit in unitsInRange)
        {
            Gizmos.DrawWireSphere(unit.transform.position + Vector3.up * 3.0f, 1.0f);
        }
    }
}
