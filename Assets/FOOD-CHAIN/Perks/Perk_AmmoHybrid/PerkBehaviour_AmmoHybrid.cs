using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBehaviour_AmmoHybrid : PerkBehaviour
{
    // this perk combines all ammo types together into one clump
    // creating a massive stack of ammo that the weapon can consume
    
    // track hoeveel ammo elke stack had 
    // maak daar een % van 
    // combine alles bijelkaar

    // als unequiped word
    // schuif die % door naar elke ammostack die we hadden opgeslagen om zo een even amount of ammo te burnen



    //PERK IDea
    // Increase pickup drops en laat ammo pickups naar je toe bewegen door de muur

    public override void AddAbility()
    {
        base.AddAbility();
        Debug.LogError("ACTIVATE AMMOHYBRID");
    }



    public override void RemoveAbility()
    {
        base.RemoveAbility();
        Debug.LogError("REMOVE AMMOHYBRID");
    }

}
