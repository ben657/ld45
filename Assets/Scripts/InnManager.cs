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
        int rarity = Random.Range(1, rarities + 1);
        float rarityPercentage = (float)rarity / (float)rarities;
        int minStat = (int)((maxStats - (maxStats / (rarities + 1))) * rarityPercentage);
        int maxStat = minStat + (maxStats / (rarities + 1)) + 1;

        int str = Random.Range(minStat, maxStat);
        int inte = Random.Range(minStat, maxStat);
        int dex = Random.Range(minStat, maxStat);

        HeroUnit hero = Instantiate(heroPrefab);
        hero.name = "Some random name";
        hero.rarity = rarity;
        hero.cost = (str + inte + dex) * 10;
        hero.SetStats(str, inte, dex);
        Ability[] abilites = { };
        if (str > inte && str > dex) abilites = strAbilities;
        else if (inte > str && inte > dex) abilites = Random.Range(0.0f, 1.0f) > 0.5f ? intDmgAbilities : intSupAbilities;
        else if (dex > str && dex > inte) abilites = dexAbilities;
        else
        {
            int choice = Random.Range(0, 4);
            if (choice == 0) abilites = strAbilities;
            else if (choice == 1) abilites = intDmgAbilities;
            else if (choice == 2) abilites = intSupAbilities;
            else if (choice == 3) abilites = dexAbilities;
        }
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
        movementController.enabled = false;

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
