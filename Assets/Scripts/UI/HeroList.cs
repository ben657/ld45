using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroList : MonoBehaviour
{
    public HeroInfo infoPrefab;
    public bool isPartyList = false;

    public Dictionary<HeroUnit, HeroInfo> heroInfoMap = new Dictionary<HeroUnit, HeroInfo>();

    public void Clear()
    {
        heroInfoMap.Clear();
        List<GameObject> toDestroy = new List<GameObject>();
        for(int i = 0; i < transform.childCount; i++)
        {
            toDestroy.Add(transform.GetChild(i).gameObject);
        }
        toDestroy.ForEach((o) => Destroy(o));
    }

    public void AddHero(HeroUnit hero)
    {
        HeroInfo info = Instantiate(infoPrefab);
        info.InParty = isPartyList;
        info.SetHero(hero);
        info.transform.SetParent(transform, false);
        heroInfoMap.Add(hero, info);
    }

    public void RemoveHero(HeroUnit hero)
    {
        if(heroInfoMap.ContainsKey(hero))
        {
            HeroInfo info = heroInfoMap[hero];
            heroInfoMap.Remove(hero);
            Destroy(info.gameObject);
        }
    }
}
