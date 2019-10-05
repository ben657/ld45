using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartyManager : MonoBehaviour
{
    public static PartyManager it;

    public HashSet<HeroUnit> party = new HashSet<HeroUnit>();

    private void Awake()
    {
        if(!it)
        {
            it = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMember(HeroUnit hero)
    {
        party.Add(hero);
        DontDestroyOnLoad(hero.gameObject);
    }

    public void RemoveMember(HeroUnit hero)
    {
        if (party.Contains(hero))
            party.Remove(hero);
        Destroy(hero.gameObject);
    }

    public void LoadDungeon()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadInn()
    {
        SceneManager.LoadScene(0);
    }
}
