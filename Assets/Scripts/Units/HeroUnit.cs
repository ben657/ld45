using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : Unit
{
    public int rarity = 0;
    public int cost = 0;
    protected override bool ShouldTarget(Unit unit)
    {
        return unit is MonsterUnit;
    }
}
