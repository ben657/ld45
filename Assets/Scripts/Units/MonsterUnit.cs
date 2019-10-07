using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUnit : Unit
{
    public StatType primaryStat = StatType.STR;
    public int rarity = 1;

    protected override void Start()
    {
        base.Start();
        SetStats(StatHelper.GenerateStats(rarity, primaryStat));
    }

    protected override bool ShouldTarget(Unit unit)
    {
        if (targettingAlignment == TargettingAlignment.Enemy)
            return unit.GetComponent<HeroUnit>() != null;
        else if (targettingAlignment == TargettingAlignment.Friendly)
            return unit.GetComponent<MonsterUnit>() != null;
        else return false;
    }
}
