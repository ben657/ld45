using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeroController : MonoBehaviour
{
    public Unit unit;
    public LayerMask interactableLayers;
    public Color color = Color.black;
    public float interactDist = 0.0f;

    HashSet<Interactable> interactables = new HashSet<Interactable>();
    Interactable interacting;

    // Start is called before the first frame update
    void Start()
    {
        if (unit is HeroUnit) ((HeroUnit)unit).SetColor(color);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayers))
            {
                unit.GetComponent<UnitMovementController>().Move(hit.point);
            }
        }

        if(!unit)
        {
            PartyManager.it.LoadInn();
        }
    
        if(interacting)
        {
            if ((interacting.transform.position - transform.position).sqrMagnitude >= interactDist * interactDist)
                StopInteracting();
        }
        else
        {
            foreach (Interactable interactable in interactables)
            {
                if ((interactable.transform.position - transform.position).sqrMagnitude <= interactDist * interactDist)
                {
                    Debug.Log("Interacting");
                    interacting = interactable;
                    interactable.StartInteraction(unit);
                }
            }
        }
    }

    void StopInteracting()
    {
        if (!interacting) return;
        interacting.StopInteraction();
        interacting = null;
        Debug.Log("Stopped interacting");
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable)
            interactables.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable && interactables.Contains(interactable))
        {
            interactables.Remove(interactable);
            if(interacting == interactable)
            {
                StopInteracting();
            }
        }
    }
}
