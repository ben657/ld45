using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityActivationType
{
    Always,
    Range
}

public class Ability : MonoBehaviour
{
    protected Unit unit;

    public AbilityActivationType activationType = AbilityActivationType.Always;
    public float cooldown = 1.0f;
    public float minDexCDModifier = 1.0f;
    public float maxDexCDModifier = 0.5f;
    
    public float minRange = 0.0f;
    public float maxRange = 10.0f;

    float minRange2 = 0.0f;
    float maxRange2 = 0.0f;

    bool coolingDown = false;
    float sinceUsed = 0.0f;
    [SerializeField]
    float effectiveCD = 0.0f;

    protected bool active = false;

    private void Awake()
    {
        minRange2 = minRange * minRange;
        maxRange2 = maxRange * maxRange;
    }

    protected virtual void Start()
    {

    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
        float dexMod = unit.GetStats().GetStat(StatType.DEX) / StatHelper.maxStatValue;
        float range = minDexCDModifier - maxDexCDModifier;
        float dexCDMod = maxDexCDModifier + (range * dexMod);
        effectiveCD = cooldown * dexCDMod;
    }

    public virtual void Activate()
    {
        active = true;
        sinceUsed = 0.0f;
    }

    public bool IsCoolingDown()
    {
        return coolingDown;
    }

    public bool IsActive()
    {
        return active;
    }

    public bool CanActivate()
    {
        if (coolingDown || IsActive()) return false;
        if(activationType == AbilityActivationType.Range)
        {
            Unit target = unit.GetTarget();
            if (!target) return false;
            float dist2 = (target.GetPointOnBounds(transform.position) - transform.position).sqrMagnitude;
            return minRange2 < dist2 && dist2 < maxRange2;
        }
        return true;
    }

    protected virtual void Update()
    {
        if(coolingDown)
            sinceUsed += Time.deltaTime;
        coolingDown = sinceUsed < effectiveCD;
    }
}
