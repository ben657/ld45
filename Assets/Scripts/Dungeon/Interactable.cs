using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float baseInteractTime = 1.0f;

    Unit interactingUnit;
    float interactTime = 0.0f;

    public void StartInteraction(Unit unit)
    {
        interactingUnit = unit;
        interactTime = 0.0f;
    }

    public void StopInteraction()
    {
        interactingUnit = null;
        interactTime = 0.0f;
    }

    protected virtual IEnumerator Activate()
    {
        yield return null;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactingUnit) return;
        interactTime += Time.deltaTime;
        if(interactTime >= baseInteractTime)
        {
            StartCoroutine(Activate());
        }
    }
}
