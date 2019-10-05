using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUnit : Unit
{
    protected override bool ShouldTarget(Unit unit)
    {
        return unit is HeroUnit;
    }
}
