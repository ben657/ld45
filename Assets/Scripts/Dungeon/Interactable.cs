using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float baseInteractTime = 1.0f;

    Unit interactingUnit;
    float interactTime = 0.0f;
    bool activated = false;
    HealthBar bar;

    protected virtual void Awake()
    {
        bar = GetComponentInChildren<HealthBar>();
    }

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
        bar.gameObject.SetActive(false);
        if (!interactingUnit) return;
        bar.gameObject.SetActive(true);
        interactTime += Time.deltaTime;
        if(interactTime >= baseInteractTime && !activated)
        {
            interactTime = baseInteractTime;
            StartCoroutine(Activate());
            activated = true;
        }
        bar.SetPercentage(interactTime / baseInteractTime);
    }
}
