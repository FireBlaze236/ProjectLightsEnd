using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : BasePickup
{
    public override void Pickup(GameObject who)
    {
        base.Pickup(who);
        if (who.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.Heal(_amount);
            this.gameObject.SetActive(false);
        }

        
    }
}
