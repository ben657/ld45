using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : Unit
{
    public int rarity = 0;
    public int cost = 0;

    protected override bool ShouldTarget(Unit unit)
    {
        if (targettingAlignment == TargettingAlignment.Enemy)
            return unit.GetComponent<MonsterUnit>() != null;
        else if (targettingAlignment == TargettingAlignment.Friendly)
            return unit.GetComponent<HeroUnit>() != null;
        else return false;
    }
}
