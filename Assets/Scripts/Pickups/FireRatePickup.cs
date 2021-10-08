using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRatePickup : BasePickup
{
    [SerializeField] float _duration = 5f;
    public override void Pickup(GameObject who)
    {
        base.Pickup(who);
        if (who.TryGetComponent(out PlayerShoot playerShoot))
        {
            playerShoot.MultiplyFireRate(_amount, _duration);
            this.gameObject.SetActive(false);
        }

        
    }
}
