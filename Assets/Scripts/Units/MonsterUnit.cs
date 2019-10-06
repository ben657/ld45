using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUnit : Unit
{
    public StatType primaryStat = StatType.STR;
    public int rarity = 1;

    int rarities = 4;
    int maxStats = 25;

    protected override void Start()
    {
        base.Start();

        int secondaryRarity = Mathf.Clamp(rarity - 1, 1, rarity);
        float rarityPercentage = (float)secondaryRarity / (float)rarities;
        int minStat = (int)((maxStats - (maxStats / (rarities + 1))) * rarityPercentage);
        int maxStat = minStat + (maxStats / (rarities + 1)) + 1;
        
        SetStats(Random.Range(minStat, maxStat), Random.Range(minStat, maxStat), Random.Range(minStat, maxStat));

        rarityPercentage = (float)rarity / (float)rarities;
        minStat = (int)((maxStats - (maxStats / (rarities + 1))) * rarityPercentage);
        maxStat = minStat + (maxStats / (rarities + 1)) + 1;
        GetStats().SetStat(primaryStat, Random.Range(minStat, maxStat));
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
