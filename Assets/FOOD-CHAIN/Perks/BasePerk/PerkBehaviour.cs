using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBehaviour : MonoBehaviour
{
    public virtual void AddAbility()
    {
   
        Debug.LogError("activated perk");
    }

    public virtual void RemoveAbility()
    {
       
        Debug.LogError("DisabledPerk");
    }

}
