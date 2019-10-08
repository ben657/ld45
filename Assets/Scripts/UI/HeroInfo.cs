using System.Collections;
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
    public Button actionButton;

    public Sprite strPanel;
    public Sprite intPanel;
    public Sprite dexPanel;

    public Transform abilitiesList;

    public void SetHero(HeroUnit hero)
    {
        this.hero = hero;
        nameText.text = hero.name;
        if (!InParty)
            buttonText.text = "Hire (" + hero.cost + " Gold)";
        else
            buttonText.text = "Remove from party";

        if (!InParty && hero.cost > PartyManager.it.GetGold())
            actionButton.interactable = false;

        levelText.text = "Rarity " + hero.rarity;
        UnitStats stats = hero.GetStats();
        strStatImage.transform.localScale = new Vector3(stats.strength / 25.0f, 1.0f, 1.0f);
        intStatImage.transform.localScale = new Vector3(stats.intelligence / 25.0f, 1.0f, 1.0f);
        dexStatImage.transform.localScale = new Vector3(stats.dexterity / 25.0f, 1.0f, 1.0f);

        switch(stats.GetPrimaryStat())
        {
            case StatType.STR:
                GetComponent<Image>().sprite = strPanel;
                break;
            case StatType.INT:
                GetComponent<Image>().sprite = intPanel;
                break;
            case StatType.DEX:
                GetComponent<Image>().sprite = dexPanel;
                break;
        }

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
        {
            PartyManager.it.RemoveMember(hero);
            GetComponentInParent<InnUIManager>().forHireList.AddHero(hero);
        }
        else
        {
            PartyManager.it.AddMember(hero);
            PartyManager.it.TakeGold(hero.cost);
            GetComponentInParent<InnUIManager>().partyList.AddHero(hero);
            hero.GetAnimator().SetTrigger("Attack2");
        }
        GetComponentInParent<HeroList>().RemoveHero(hero);
    }
}
