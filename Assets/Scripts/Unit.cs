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

public enum TargettingAlignment
{
    Friendly,
    Enemy
}

public enum StatType
{
    STR,
    INT,
    DEX
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
        maxHealth = str * 3.0f;
        health = maxHealth;

        strength = str;
        intelligence = inte;
        dexterity = dex;
    }

    public float GetHealthPercentage()
    {
        return health / maxHealth;
    }

    public float GetStat(StatType statType)
    {
        if (statType == StatType.STR) return strength;
        else if (statType == StatType.INT) return intelligence;
        else if (statType == StatType.DEX) return dexterity;
        return 0;
    }

    public StatType GetPrimaryStat()
    {
        int bestStat = Mathf.Max(strength, intelligence, dexterity);
        if (bestStat == strength)
            return StatType.STR;
        else if (bestStat == intelligence)
            return StatType.INT;
        else
            return StatType.DEX;

    }

    public void SetStat(StatType statType, int stat)
    {
        if (statType == StatType.STR)
        {
            strength = stat;
            maxHealth = strength * 3.0f;
        }
        else if (statType == StatType.INT) intelligence = stat;
        else if (statType == StatType.DEX) dexterity = stat;
    }
}

public class Unit : MonoBehaviour
{
    //Ability[] abilities;

    public TargettingType targettingType = TargettingType.Nearest;
    public TargettingAlignment targettingAlignment = TargettingAlignment.Enemy;
    
    UnitMovementController movementController;
    UnitAbilityController abilityController;
    [SerializeField]
    Animator animator;
    [SerializeField]
    UnitStats stats;
    [SerializeField]
    HealthBar healthBar;
    Collider boundsCollider;

    Unit target;
    HashSet<Unit> unitsInRange = new HashSet<Unit>();
    
    void Awake()
    {
        abilityController = GetComponentInChildren<UnitAbilityController>();
        abilityController.SetUnit(this);
    }

    public void SetStats(int str, int inte, int dex)
    {
        stats = new UnitStats(str, inte, dex);
    }

    public void SetStats(UnitStats stats)
    {
        this.stats = stats;
    }

    protected virtual void Start()
    {
        movementController = GetComponent<UnitMovementController>();
        Collider[] colliders = GetComponents<Collider>();
        foreach(Collider collider in colliders)
        {
            if(!collider.isTrigger)
            {
                boundsCollider = collider;
                break;
            }
        }
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

    public Bounds GetBounds()
    {
        return boundsCollider.bounds;
    }

    public Vector3 GetPointOnBounds(Vector3 target)
    {
        return boundsCollider.ClosestPoint(target);
    }

    public Unit GetTarget()
    {
        return target;
    }

    public void ClearUnitsInRange()
    {
        unitsInRange.Clear();
    }

    public void Damage(float amount, Vector3 knockback)
    {
        stats.health -= amount;
        healthBar.SetPercentage(stats.GetHealthPercentage());
        if(knockback.sqrMagnitude > 0.0f)
            movementController.GetAgent().velocity += knockback;

        if (stats.health <= 0.0f)
            Kill();
    }

    public void Heal(float amount)
    {
        stats.health = Mathf.Clamp(stats.health + amount, 0.0f, stats.maxHealth);
        healthBar.SetPercentage(stats.GetHealthPercentage());
    }

    public void Kill()
    {
        //TODO: something pretty
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return stats.health <= 0.0f;
    }

    void UpdateTarget()
    {
        List<Unit> deadUnits = new List<Unit>();
        foreach(Unit unit in unitsInRange)
        {
            if (!unit || unit.IsDead()) deadUnits.Add(unit);
        }
        deadUnits.ForEach(u => unitsInRange.Remove(u));

        float bestMetric = 0;
        Unit bestUnit = null;
        List<Unit> targets = new List<Unit>(unitsInRange);
        targets = targets.FindAll(u => ShouldTarget(u));
        if(targettingType == TargettingType.Nearest)
        {
            bestMetric = Mathf.Infinity;
            foreach(Unit unit in targets)
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
            foreach (Unit unit in targets)
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
            foreach (Unit unit in targets)
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
            foreach (Unit unit in targets)
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
