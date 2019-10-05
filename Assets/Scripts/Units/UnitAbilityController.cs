using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityController : MonoBehaviour
{
    public bool automatic = true;

    Ability activeAbility = null;
    Ability[] abilities;

    private void Awake()
    {
        UpdateAbilityList();
    }

    public void UpdateAbilityList()
    {
        abilities = GetComponentsInChildren<Ability>();
    }

    public void PassthroughMessage(string name)
    {
        foreach(Ability ability in abilities)
        {
            ability.SendMessage(name);
        }
    }

    public void ActivateAbility(Ability ability, bool skipCheck = false)
    {
        if(skipCheck || ability.CanActivate())
        {
            activeAbility = ability;
            ability.Activate();
        }
    }

    private void Update()
    {
        if (activeAbility && !activeAbility.IsActive()) activeAbility = null;
        if (!activeAbility && automatic)
        {
            List<Ability> possibleAbilities = new List<Ability>();
            foreach(Ability ability in abilities)
            {
                if (ability.CanActivate()) possibleAbilities.Add(ability);
            }

            if (possibleAbilities.Count == 0) return;
            else if (possibleAbilities.Count == 1) ActivateAbility(possibleAbilities[0], true);
            else
            {
                Ability chosen = null;
                float longestCD = 0.0f;
                foreach(Ability ability in possibleAbilities)
                {
                    if(ability.cooldown > longestCD)
                    {
                        chosen = ability;
                        longestCD = ability.cooldown;
                    }
                }
                ActivateAbility(chosen, true);
            }
        }
    }
}
