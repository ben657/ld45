using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUnit : Unit
{
    public Coin coinPrefab;
    public StatType primaryStat = StatType.STR;
    public int rarity = 1;

    protected override void Start()
    {
        base.Start();
        UnitStats stats = StatHelper.GenerateStats(rarity, primaryStat);
        stats.UpdateMaxHealth(true, 0.5f);
        SetStats(stats);
    }

    protected override bool ShouldTarget(Unit unit)
    {
        if (targettingAlignment == TargettingAlignment.Enemy)
            return unit.GetComponent<HeroUnit>() != null;
        else if (targettingAlignment == TargettingAlignment.Friendly)
            return unit.GetComponent<MonsterUnit>() != null;
        else return false;
    }

    public override IEnumerator Kill()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        UnitStats stats = GetStats();
        int reward = (stats.strength + stats.intelligence + stats.dexterity);
        PartyManager.it.AddGold(reward);
        return base.Kill();
    }
}
