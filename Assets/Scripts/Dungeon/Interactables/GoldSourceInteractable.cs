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
        int coins = Mathf.Min(value, 10);
        for(int i = 0; i < coins; i++)
        {
            Instantiate(coinPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.2f);
        }

        yield return base.Activate();
    }
}
