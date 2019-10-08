using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnManager : MonoBehaviour
{
    public HeroUnit heroPrefab;
    public HeroList heroList;

    public Ability[] strAbilities;
    public Ability[] intDmgAbilities;
    public Ability[] intSupAbilities;
    public Ability[] dexAbilities;

    public Transform[] tables;
    List<Transform> seats = new List<Transform>();

    public int minHeroes = 3;
    public int maxHeroes = 5;
    public int rarities = 4;
    public int maxStats = 25;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform table in tables)
        {
            Transform seatsRoot = table.GetChild(0);
            for(int i = 0; i < seatsRoot.childCount; i++)
            {
                seats.Add(seatsRoot.GetChild(i));
            }
        }

        GenerateHero(1, StatType.STR, 0);
        int heroes = Random.Range(minHeroes, maxHeroes + 1);
        for(int i = 0; i < heroes; i++)
            GenerateHero();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public HeroUnit GenerateHero()
    {
        StatType primaryStat = (StatType)Random.Range(0, 3);
        int rarity = Random.Range(1, rarities + 1);
        return GenerateHero(rarity, primaryStat);
    }

    public HeroUnit GenerateHero(int rarity, StatType primaryStat, int cost = -1)
    {
        UnitStats stats = StatHelper.GenerateStats(rarity, primaryStat);
        stats.UpdateMaxHealth(true, 2.0f);
        int heroCost = cost < 0 ? (stats.strength + stats.intelligence + stats.dexterity) * 10 : cost;
        return GenerateHero(stats, rarity, primaryStat, heroCost);
    }

    public HeroUnit GenerateHero(UnitStats stats, int rarity, StatType primaryStat, int cost)
    {
        HeroUnit hero = Instantiate(heroPrefab);
        hero.name = StatHelper.MakeName();
        hero.rarity = rarity;
        hero.SetStats(stats);
        hero.cost = cost;
        Ability[] abilites = { };
        if (primaryStat == StatType.STR) abilites = strAbilities;
        else if (primaryStat == StatType.INT) abilites = Random.Range(0.0f, 1.0f) > 0.5f ? intDmgAbilities : intSupAbilities;
        else if (primaryStat == StatType.DEX) abilites = dexAbilities;
        UnitAbilityController abilityRoot = hero.GetAbilityController();
        foreach (Ability abilityPrefab in abilites)
        {
            Ability ability = Instantiate(abilityPrefab);
            ability.transform.parent = abilityRoot.transform;
            ability.transform.localPosition = Vector3.zero;
        }
        abilityRoot.UpdateAbilityList();
        abilityRoot.enabled = false;

        if (abilites == intSupAbilities) hero.targettingAlignment = TargettingAlignment.Friendly;

        UnitMovementController movementController;
        if (abilites == strAbilities)
            movementController = hero.gameObject.AddComponent<MeleeUnitMovementController>();
        else
            movementController = hero.gameObject.AddComponent<RangedUnitMovementController>();
        movementController.Disable();

        int seatIndex = Random.Range(0, seats.Count);
        Transform seat = seats[seatIndex];
        seats.RemoveAt(seatIndex);
        hero.transform.position = seat.position;

        heroList.AddHero(hero);
        return hero;
    }

    public void LoadDungeon()
    {
        PartyManager.it.LoadDungeon();
    }
}
