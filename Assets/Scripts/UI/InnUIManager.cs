using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnUIManager : MonoBehaviour
{
    public HeroList forHireList;
    public HeroList partyList;
    public Text goldText;

    private void Start()
    {
        goldText.text = PartyManager.it.GetGold() + " Gold";
    }
}
