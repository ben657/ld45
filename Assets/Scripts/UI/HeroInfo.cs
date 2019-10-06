﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfo : MonoBehaviour
{
    HeroUnit hero;

    public bool InParty = false;
    public Font font;

    public Text nameText;
    public Text levelText;
    public Image strStatImage;
    public Image intStatImage;
    public Image dexStatImage;
    public Text buttonText;

    public Transform abilitiesList;

    public void SetHero(HeroUnit hero)
    {
        this.hero = hero;
        nameText.text = hero.name;
        buttonText.text = "Hire (" + hero.cost + " Gold)";
        levelText.text = "Rarity " + hero.rarity;
        UnitStats stats = hero.GetStats();
        strStatImage.transform.localScale = new Vector3(stats.strength / 25.0f, 1.0f, 1.0f);
        intStatImage.transform.localScale = new Vector3(stats.intelligence / 25.0f, 1.0f, 1.0f);
        dexStatImage.transform.localScale = new Vector3(stats.dexterity / 25.0f, 1.0f, 1.0f);

        Ability[] abilities = hero.GetAbilityController().GetAbilities();
        foreach(Ability ability in abilities)
        {
            GameObject o = new GameObject();
            Text text = o.AddComponent<Text>();
            text.text = ability.name;
            text.resizeTextForBestFit = true;
            text.font = font;
            text.alignment = TextAnchor.MiddleCenter;
            o.transform.SetParent(abilitiesList, false);
        }
    }

    public void DoAction()
    {
        if (InParty)
            Debug.Log("Should remove from party");
        else
        {
            PartyManager.it.AddMember(hero);
            GetComponentInParent<InnUIManager>().partyList.AddHero(hero);
        }
        GetComponentInParent<HeroList>().RemoveHero(hero);
    }
}
