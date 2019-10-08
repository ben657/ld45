using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                SetColor(Color.red);
                break;
            case StatType.INT:
                if (targettingAlignment == TargettingAlignment.Friendly)
                    SetColor(Color.cyan);
                else
                    SetColor(Color.blue);
                break;
            case StatType.DEX:
                SetColor(Color.green);
                break;
        }
    }

    public void SetColor(Color color)
    {
        Chest.material.SetColor("_BaseColor", color);
        Hat.material.SetColor("_BaseColor", color);
    }

    protected override bool ShouldTarget(Unit unit)
    {
        if (targettingAlignment == TargettingAlignment.Enemy)
            return unit.GetComponent<MonsterUnit>() != null;
        else if (targettingAlignment == TargettingAlignment.Friendly)
            return unit.GetComponent<HeroUnit>() != null;
        else return false;
    }

    public override IEnumerator Kill()
    {
        PartyManager.it.RemoveMember(this);
        yield return base.Kill();
    }
}
