using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartyManager : MonoBehaviour
{
    public static PartyManager it;

    public HashSet<HeroUnit> party = new HashSet<HeroUnit>();

    int gold = 0;

    private void Awake()
    {
        if(!it)
        {
            it = this;
            DontDestroyOnLoad(gameObject);

            gold = PlayerPrefs.GetInt("gold", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("gold", gold);
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
        SceneManager.MoveGameObjectToScene(hero.gameObject, SceneManager.GetActiveScene());
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public void TakeGold(int amount)
    {
        gold -= amount;
        if (gold < 0) gold = 0;
    }

    public int GetGold()
    {
        return gold;
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
