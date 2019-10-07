using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSourceInteractable : Interactable
{
    public Coin coinPrefab;
    public int minValue = 1;
    public int maxValue = 2;

    int value = 0;

    private void Start()
    {
        value = Random.Range(minValue, maxValue + 1);
    }

    protected override IEnumerator Activate()
    {
        PartyManager.it.AddGold(value);
        for(int i = 0; i < value; i++)
        {
            Instantiate(coinPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.2f);
        }

        base.Activate();
    }
}
