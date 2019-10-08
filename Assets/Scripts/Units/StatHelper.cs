using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatHelper
{
    public static int rarities = 4;
    public static int maxStatValue = 25;
    public static float healthModifier = 10.0f;

    static string[] fnames = {
        "Nigel",
        "Nick",
        "Theodore",
        "Jonah",
        "Harry",
        "Peter",
        "Bartholomew",
        "Richard",
        "Larry",
        "Benedict",
        "Dirk",
        "Flipe",
        "Stephen",
        "Alex",
        "Negan",
        "Colin",
        "Aaron",
        "Arran",
        "Dickolas",
        "Bren",
        "Ollay",
        "Phillip",
        "Christopher",
        "Margaret",
        "Dank",
        "Bartholomew",
        "Jacob",
        "George",
        "The Mighty",
        "Vicktor",
        "Rick",
        "Anushka",
        "Nicolas",
        "William", 
        "Swifto",
        "Dick",
        "El",
        "Marco",
        "David", 
        "David", 
        "Ja'quaan",
        "Saquelle",
        "I",
        "Steve",
        "Richard",
        "Shalalaka",
        "Tiffany",
        "Vallery",
        "John",
        "Bumder",
        "Harry",
        "Vince",
        "Hermione",
        "Ronald",
        "Albus",
        "Dumble",
        "Draco",
        "Ur",
        "Robert",
        "Ur",
        "Bill",
        "Ben",
        "Sam",
        "Bin"
    };

    static string[] lnames = {
        "Thornberry",
        "Fury",
        "Kennedy",
        "Simpson",
        "Styles",
        "Jameson",
        "Hardwick",
        "Johnson",
        "Drinkwater",
        "Cumberbatch",
        "Thrustcore",
        "Brindunk",
        "Dunkmeister",
        "Coxgood",
        "Goldstein",
        "Funkbuster",
        "Aarons",
        "Pierce",
        "Closel",
        "Sandwich",
        "Yarns",
        "Jamstar",
        "Vertigan",
        "Hatcher",
        "Mes",
        "Winklestien",
        "Hog",
        "Best",
        "Ball",
        "Slimy",
        "Gobbler",
        "Longstaff",
        "Reynolds",
        "VanWinkle",
        "Dyke",
        "Matador",
        "Polo",
        "Cameron",
        "EnterprisingDwarf",
        "Han",
        "Batman",
        "Fanny",
        "Whelpmesiter",
        "Gurenburg",
        "Windabringah",
        "Smith",
        "Cumbersnatch",
        "Potter",
        "Spaghetti",
        "Granger",
        "Weasly",
        "Dumbledore",
        "Dies",
        "Malfoy",
        "Mum",
        "Girlyman",
        "Dad",
        "Pot",
        "Pot",
        "Buca",
        "Diesel"
    };

    static string[] titles = {
        "The Strong",
        "The Incandescent",
        "The Well Endowed",
        "The Problematic",
        "The Unpronouncable",
        "The Moist",
        "or is it?",
        "Clone Number #420",
        "The [REDACTED]",
        "The Undefeatable",
        "Last of their House",
        "IV",
        "Can't read maps",
        "is today's winner",
        "is today's loser",
        "five times grand champion",
        "The Weak and Pathetic",
        "Owner of the 12 Meter Sword",
        "hates everything you like",
        "Bus Wanker",
        "Winner of over 5 Eating Competitions",
        "Game Developer ",
        "Devourer of Small Sandwiches",
        "©",
        "™",
        "The Underdeveloped",
        "Owes you money",
        "The SICK RADICAL",
        "Patent Pending",
        "Literally who?",
        "AAAAAAAAAAAAAA",
        "Jacks it on long driver",
        "(Meth Addict)",
        "Diddles Cabbages",
        "The Invisible",
        "The Hipster",
        "Known to Inhale Soup",
        "Didn't fly so good",
        "Master Soupmaster",
        "Substance Abuser",
        "Window Licker",
        "Alien from outer space",
        "Has a new mixtape",
        "The Your Title Here",
        "Complete Psychopath",
        "The Banned",
        "The Dualist ",
        "The Overpowered",
        "Hater of Elves",
        "The Underpowered",
        "Town Racist",
        "Can't Spell",
        "The Memelord",
        "You Know It Baby",
        "Isn't Allowed Near Local Schools",
        "Yells at clouds",
        "Old Person ",
        "The Grey",
        "The White",
        "The Sober",
    };

    public static string MakeName()
    {
        string name = fnames[Random.Range(0, fnames.Length)];
        bool useSName = Random.Range(0, 1.0f) > 0.5f;
        if (useSName)
            name += " " + lnames[Random.Range(0, lnames.Length)];
        else
            name += ", " + titles[Random.Range(0, titles.Length)];
        return name;
    }

    public static int[] GenerateStatRange(int rarity)
    {
        float rarityPercentage = (float)rarity / (float)rarities;
        int minStat = (int)((maxStatValue - (maxStatValue / (rarities + 1))) * rarityPercentage);
        if (minStat <= 0) minStat = 1;
        int maxStat = minStat + (maxStatValue / (rarities + 1)) + 1;
        if (maxStat <= 0) maxStat = 1;
        return new int[] { minStat, maxStat };
    }

    public static UnitStats GenerateStats(int rarity, StatType primaryStat)
    {
        int secondaryRarity = Mathf.Clamp(rarity - 1, 0, rarity);
        int[] secondaryStatRange = GenerateStatRange(secondaryRarity);

        UnitStats stats = new UnitStats(
            Random.Range(secondaryStatRange[0], secondaryStatRange[1]),
            Random.Range(secondaryStatRange[0], secondaryStatRange[1]),
            Random.Range(secondaryStatRange[0], secondaryStatRange[1])
        );

        int[] primaryStatRange = GenerateStatRange(rarity);
        stats.SetStat(primaryStat, Random.Range(primaryStatRange[0], primaryStatRange[1]));

        return stats;
    }
}
