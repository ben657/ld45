using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeroController : MonoBehaviour
{
    public Unit unit;
    public LayerMask interactableLayers;
    public Color color = Color.black;

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
    }
}
