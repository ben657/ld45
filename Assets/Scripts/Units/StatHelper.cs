﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatHelper
{
    public static int rarities = 4;
    static int maxStatValue = 25;

    public static int[] GenerateStatRange(int rarity)
    {
        float rarityPercentage = (float)rarity / (float)rarities;
        int minStat = (int)((maxStatValue - (maxStatValue / (rarities + 1))) * rarityPercentage);
        int maxStat = minStat + (maxStatValue / (rarities + 1)) + 1;
        return new int[] { minStat, maxStat };
    }

    public static UnitStats GenerateStats(int rarity, StatType primaryStat)
    {
        int secondaryRarity = Mathf.Clamp(rarity - 1, 0, rarity);
        int[] secondaryStatRange = GenerateStatRange(secondaryRarity);

        UnitStats stats = new UnitStats(
            Random.Range(secondaryStatRange[0], secondaryStatRange[1]),
            Random.Range(secondaryStatRange[0], secondaryStatRange[1]),
            Random.Range(secondaryStatRange[0], secondaryStatRange[1])
        );

        int[] primaryStatRange = GenerateStatRange(rarity);
        stats.SetStat(primaryStat, Random.Range(primaryStatRange[0], primaryStatRange[1]));

        return stats;
    }
}
