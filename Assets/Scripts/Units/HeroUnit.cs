using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : Unit
{
    public int rarity = 0;
    public int cost = 0;

    public MeshRenderer Chest;
    public MeshRenderer Hat;

    protected override void Start() 
    {
        base.Start();

        switch(GetStats().GetPrimaryStat())
        {
            case StatType.STR:
                Chest.material.SetColor("_BaseColor", Color.red);
                Hat.material.SetColor("_BaseColor", Color.red);
                break;
            case StatType.INT:         
                Chest.material.SetColor("_BaseColor", Color.blue);
                Hat.material.SetColor("_BaseColor", Color.blue);
                break;
            case StatType.DEX:
                Chest.material.SetColor("_BaseColor", Color.green);
                Hat.material.SetColor("_BaseColor", Color.green);
                break;
        }
    }

    protected override bool ShouldTarget(Unit unit)
    {
        if (targettingAlignment == TargettingAlignment.Enemy)
            return unit.GetComponent<MonsterUnit>() != null;
        else if (targettingAlignment == TargettingAlignment.Friendly)
            return unit.GetComponent<HeroUnit>() != null;
        else return false;
    }
}
