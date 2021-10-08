using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : BasePickup
{
    public override void Pickup(GameObject who)
    {
        base.Pickup(who);

        if (who.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.RechargeEnergy(_amount);
            this.gameObject.SetActive(false);
        }

        
    }
}
