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

    public int rarities = 4;
    public int maxStats = 25;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("Min: " + minStat.ToString() + " Max: " + maxStat.ToString());
        int str = Random.Range(minStat, maxStat);
        int inte = Random.Range(minStat, maxStat);
        int dex = Random.Range(minStat, maxStat);

        HeroUnit hero = Instantiate(heroPrefab);
        hero.name = "Some random name";
        hero.rarity = rarity;
        hero.cost = (str + inte + dex) * 10;
        hero.SetStats(str, inte, dex);
        //Ability[] abilites = { };
        //if (str > inte && str > dex) abilites = strAbilities;
        //else if (inte > str && inte > dex) abilites = Random.Range(0.0f, 1.0f) > 0.5f ? intDmgAbilities : intSupAbilities;
        //else if (dex > str && dex > inte) abilites = dexAbilities;
        //else
        //{
        //    int choice = Random.Range(0, 4);
        //    if (choice == 0) abilites = strAbilities;
        //    else if (choice == 1) abilites = intDmgAbilities;
        //    else if (choice == 2) abilites = intSupAbilities;
        //    else if (choice == 3) abilites = dexAbilities;
        //}
        //UnitAbilityController abilityRoot = hero.GetAbilityController();
        //foreach(Ability abilityPrefab in abilites)
        //{
        //    Ability ability = Instantiate(abilityPrefab);
        //    ability.transform.parent = abilityRoot.transform;
        //    ability.transform.localPosition = Vector3.zero;
        //}
        //abilityRoot.UpdateAbilityList();
        heroList.AddHero(hero);
        return hero;
    }

    public void LoadDungeon()
    {
        PartyManager.it.LoadDungeon();
    }
}
